using System.Diagnostics;
using System.IO;
using System.Reflection;
using log4net;
using log4net.Config;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Formatter;
using MQTTnet.Protocol;
using Nett;



namespace ThingsOn_MQTT_Bench
{
    internal class Program
    {

        // Entry point of the program
        private static async Task Main()
        {

            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            ILog log = log4net.LogManager.GetLogger(typeof(Program));

            //Check if fully autonomous operation is prefered (e.g. as a Docker container)

            bool isContianerized = Environment.GetEnvironmentVariable("ThingsOnMQTTBenchContainerized") == "true";

            if (!isContianerized)

            {
                // Prompt the user to start a new benchmark
                Console.Write("Do you want to start a new benchmark? (y/n): ");
                var answer = Console.ReadLine();

                // Run the benchmark if the user answers "y"
                if (answer?.ToLower() != "y") { return; }
            }

            try
            {
                log.Info("Benchmark started");
                await RunBenchmark(isContianerized);
            }
            catch (Exception ex)
            {
                log.Error($"An error occurred: {ex.Message}");
            }


            // Main logic of the benchmark
            async Task RunBenchmark(bool isContainerized = false)
            {

                // Load benchmark settings
                var (serverUri, port, cleanSession, userName, password, keepAlivePeriod, connectionTimeOut,
                     mqttVersion, clientCount, messageCount, messageSize, qos, retain) = LoadSettings(isContainerized, log);

                // Create MQTT clients
                var factory = new MqttFactory();
                var clients = Enumerable.Range(0, clientCount).Select(_ => factory.CreateMqttClient()).ToList();

                // Connect to MQTT broker and measure connect time
                var stopwatchConnect = Stopwatch.StartNew();
                var connectTasks = clients.Select(c => c.ConnectAsync(CreateMqttClientOptions(serverUri, port, cleanSession, userName, password, keepAlivePeriod, connectionTimeOut, mqttVersion))).ToList();
                await Task.WhenAll(connectTasks);
                stopwatchConnect.Stop();
                var elapsedConnect = stopwatchConnect.Elapsed;

                // Generate messages
                var messages = Enumerable.Range(1, messageCount)
                    .Select(i => new MqttApplicationMessageBuilder()
                        .WithTopic($"test/{i}")
                        .WithPayload(new byte[messageSize])
                        .WithQualityOfServiceLevel((MqttQualityOfServiceLevel)qos)
                        .WithRetainFlag(retain)
                        .Build())
                    .ToList();

                // Start benchmark
                var stopwatch = Stopwatch.StartNew();

                // Send messages
                var totalMessagesSent = clientCount * messageCount;
                var sent = 0;
                var progressReport = 0;
                var sendTasks = clients.Select(client => Task.Run(async () =>
                {
                    var results = new List<MqttClientPublishResult>();
                    foreach (var message in messages)
                    {
                        // Publish message and store the result
                        var result = await client.PublishAsync(message);
                        results.Add(result);

                        // Update progress
                        sent++;
                        var newProgressReport = sent * 100 / totalMessagesSent;
                        if (!isContainerized)
                            if (newProgressReport >= progressReport + 10)
                            {
                                progressReport = newProgressReport / 10 * 10;
                                Console.CursorLeft = 0;
                                Console.Write($"\rProgress: {progressReport}%");
                            }
                    }

                    return results;
                })).ToList();

                // Wait for all messages to be sent
                var sendResults = await Task.WhenAll(sendTasks);
                if (!isContainerized)
                {
                    Console.Write("\rProgress: 100%");
                    Console.WriteLine();
                    Console.WriteLine();
                }

                // End benchmark
                stopwatch.Stop();
                var elapsed = stopwatch.Elapsed;
                var messagesSent = clientCount * messageCount;
                var throughput = messagesSent / elapsed.TotalSeconds;
                var lossRate = (double)sendResults.SelectMany(r => r).Count(r => r.ReasonCode != MqttClientPublishReasonCode.Success) / messagesSent;
                var successRate = 1.0 - lossRate;

                // Disconnect from MQTT broker and measure disconnect time
                var stopwatchDisconnect = Stopwatch.StartNew();
                var disconnectTasks = clients.Select(c => c.DisconnectAsync()).ToList();
                await Task.WhenAll(disconnectTasks);
                stopwatchDisconnect.Stop();
                var elapsedDisconnect = stopwatchDisconnect.Elapsed;

                // Print results

                log.Info("Benchmark completed 🚀");
                log.Info("=======================================");
                log.Info($"{"Messages sent",-20} {messagesSent:N0}");
                log.Info($"{"Elapsed time",-20} {elapsed.TotalSeconds:F3}");
                log.Info($"{"Throughput",-20} {throughput:N0} messages/second");
                log.Info($"{"Connect time",-20} {elapsedConnect.TotalSeconds:F3} seconds");
                log.Info($"{"Disconnect time",-20} {elapsedDisconnect.TotalSeconds:F3} seconds");
                log.Info($"{"Success rate",-20} {successRate:P0}");
                log.Info($"{"Loss rate",-20} {lossRate:P0}");

                // Convert data size to appropriate units
                var dataSize = (double)(totalMessagesSent * messageSize);
                var dataUnits = new[] { "B", "KB", "MB", "GB", "TB" };
                var dataUnitIndex = 0;
                while (dataSize >= 1024 && dataUnitIndex < dataUnits.Length - 1)
                {
                    dataSize /= 1024;
                    dataUnitIndex++;
                }

                log.Info($"{"Data sent",-20} {dataSize:N3} {dataUnits[dataUnitIndex]}");
                log.Info("=======================================");


                // Prompt the user to run the benchmark again
                await Main();
            }
        }


        private static (string serverUri, int port, bool cleanSession, string userName, string password, TimeSpan keepAlivePeriod, TimeSpan connectionTimeOut, MqttProtocolVersion mqttVersion, int clientCount, int messageCount, int messageSize, int qos, bool retain) LoadSettings(bool IsContainerized, ILog log)
        {

            if (!IsContainerized)
            {
                // Load settings from config.toml
                var settings = Toml.ReadFile(Path.Combine(Directory.GetCurrentDirectory(), "config.toml"));

                // Parse benchmark settings
                var serverUri = settings.Get<string>("ServerUri");
                var port = settings.Get<int>("Port");
                var cleanSession = settings.Get<bool>("CleanSession");
                var userName = settings.Get<string>("Username");
                var password = settings.Get<string>("Password");
                var keepAlivePeriod = TimeSpan.FromSeconds(settings.Get<int>("KeepAlivePeriod"));
                var connectionTimeOut = TimeSpan.FromSeconds(settings.Get<int>("ConnectionTimeOut"));
                var mqttVersion = ParseMqttProtocolVersion(settings.Get<string>("MqttVersion"));
                var clientCount = settings.Get<int>("ClientCount");
                var messageCount = settings.Get<int>("MessageCount");
                var messageSize = settings.Get<int>("MessageSize");
                var qos = settings.Get<int>("Qos");
                var retain = settings.Get<bool>("Retain");

                // Print benchmark settings

                log.Info($"Benchmark settings: {clientCount} clients, {messageCount} messages/client, {messageSize:N0} bytes/message, QoS {qos}, retain {retain}, MQTT version {mqttVersion}.");
                log.Info($"Server URI: {serverUri}");
                log.Info($"Port: {port}");
                log.Info($"Clean session: {cleanSession}");
                log.Info($"User name: {userName}");
                log.Info(!string.IsNullOrEmpty(password) ? $"Password: {password[0]}{new string('*', password.Length - 1)}" : "Password: (empty)");
                log.Info($"Keep alive period: {keepAlivePeriod}");
                log.Info($"Connection timeout: {connectionTimeOut}");

                return (serverUri, port, cleanSession, userName, password, keepAlivePeriod, connectionTimeOut, mqttVersion, clientCount, messageCount, messageSize, qos, retain);
            }

            else
            {

                // Parse benchmark settings
                var serverUri = Environment.GetEnvironmentVariable("ServerUri");
                var port = Int32.Parse(Environment.GetEnvironmentVariable("Port"));
                var cleanSession = Boolean.Parse(Environment.GetEnvironmentVariable("CleanSession"));
                var userName = Environment.GetEnvironmentVariable("Username");
                var password = Environment.GetEnvironmentVariable("Password");
                var keepAlivePeriod = TimeSpan.FromSeconds(Int32.Parse(Environment.GetEnvironmentVariable("KeepAlivePeriod")));
                var connectionTimeOut = TimeSpan.FromSeconds(Int32.Parse(Environment.GetEnvironmentVariable("ConnectionTimeOut")));
                var mqttVersion = ParseMqttProtocolVersion(Environment.GetEnvironmentVariable("MqttVersion"));
                var clientCount = Int32.Parse(Environment.GetEnvironmentVariable("ClientCount"));
                var messageCount = Int32.Parse(Environment.GetEnvironmentVariable("MessageCount"));
                var messageSize = Int32.Parse(Environment.GetEnvironmentVariable("MessageSize"));
                var qos = Int32.Parse(Environment.GetEnvironmentVariable("Qos"));
                var retain = Boolean.Parse(Environment.GetEnvironmentVariable("Retain"));

                //Print benchmark settings

                log.Info($"Benchmark settings: {clientCount} clients, {messageCount} messages/client, {messageSize:N0} bytes/message, QoS {qos}, retain {retain}, MQTT version {mqttVersion}.");
                log.Info($"Server URI: {serverUri}");
                log.Info($"Port: {port}");
                log.Info($"Clean session: {cleanSession}");
                log.Info($"User name: {userName}");
                log.Info(!string.IsNullOrEmpty(password) ? $"Password: {password[0]}{new string('*', password.Length - 1)}" : "Password: (empty)");
                log.Info($"Keep alive period: {keepAlivePeriod}");
                log.Info($"Connection timeout: {connectionTimeOut}");

                return (serverUri, port, cleanSession, userName, password, keepAlivePeriod, connectionTimeOut, mqttVersion, clientCount, messageCount, messageSize, qos, retain);

            }
        }

        // Create MQTT client options from parsed settings
        private static MqttClientOptions CreateMqttClientOptions(string serverUri, int port, bool cleanSession, string userName, string password, TimeSpan keepAlivePeriod, TimeSpan connectionTimeOut, MqttProtocolVersion mqttVersion)
        {
            var options = new MqttClientOptionsBuilder()
                .WithClientId(Guid.NewGuid().ToString())
                .WithTcpServer(serverUri, port)
                .WithCleanSession(cleanSession)
                .WithCredentials(userName, password)
                .WithKeepAlivePeriod(keepAlivePeriod)
                .WithTimeout(connectionTimeOut)
                .WithProtocolVersion(mqttVersion)
                .Build();
            return options;
        }

        // Parse MQTT protocol version from a string
        private static MqttProtocolVersion ParseMqttProtocolVersion(string value)
        {
            switch (value.ToLowerInvariant())
            {
                case "v310":
                    return MqttProtocolVersion.V310;
                case "v311":
                    return MqttProtocolVersion.V311;
                case "v500":
                    return MqttProtocolVersion.V500;
                default:
                    throw new ArgumentException($"Invalid MQTT version '{value}'.");
            }
        }

    }
}