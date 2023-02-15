# ThingsOn MQTT Bench

ThingsOn MQTT Bench is a sipmle Cross-platform .NET Core benchmark tool for MQTT brokers. It measures the maximum number of messages that can be sent to the broker in a specified amount of time.

![ThingsOnMQTTBenchmin.gif](https://github.com/volkanalkilic/ThingsOn.MQTT.Bench/blob/main/ThingsOn.MQTT.Bench-min.gif)

## Features

* Measures the maximum number of messages that can be sent to an MQTT broker in a specified amount of time.
* Supports MQTT brokers running MQTT protocol versions 3.1.1 (MQTTv3.1.1) and 5.0 (MQTTv5.0).
* Allows customization of the following benchmark settings:
  * Number of MQTT clients to use for benchmarking
  * Number of messages to send per client
  * Message size in bytes
  * Quality of Service (QoS) level for messages (0, 1, or 2)
  * Whether messages should be retained by the broker
* Outputs progress information to the console during benchmarking.
* Outputs the following information upon benchmark completion:
  * Number of messages sent
  * Total time taken to send messages
  * Message throughput (messages per second)
  * Time taken to connect to the MQTT broker
  * Time taken to disconnect from the MQTT broker
  * Success rate (percentage of messages that were successfully sent)
  * Loss rate (percentage of messages that were not successfully sent)

## Usage

To use ThingsOn MQTT Bench, first make sure that you have .NET 7 installed on your system. Then, download the latest release of ThingsOn MQTT Bench for your operating system from the [releases page](https://github.com/volkanalkilic/ThingsOn.MQTT.Bench/releases).

Once you have downloaded the tool, you can run it from the command line by navigating to the directory where the tool is located and running the following command:

```bash
dotnet ThingsOn.MQTT.Bench.dll

```

By default, the tool will read its settings from a TOML file named `config.toml` in the same directory as the tool. You can customize the settings by editing this file.

## Settings

The following settings are available in the `config.toml` file:

* `Mqtt`: MQTT broker settings
  * `ServerUri`: URI of the MQTT broker
  * `Port`: Port number of the MQTT broker
  * `MqttVersion`: MQTT protocol version (either "V311" or "V500")
  * `Username`: Username for connecting to the MQTT broker
  * `Password`: Password for connecting to the MQTT broker
  * `KeepAlivePeriod`: Keep-alive period in seconds
  * `CleanSession`: Whether to use a clean session when connecting to the MQTT broker
* `Benchmark`: Benchmark settings
  * `ClientCount`: Number of MQTT clients to use for benchmarking
  * `MessageCount`: Number of messages to send per client
  * `MessageSize`: Size of messages in bytes
  * `Qos`: Quality of service level for messages (0, 1, or 2)
  * `Retain`: Whether messages should be retained by the broker

## Building from Code

To build ThingsOn MQTT Bench from code, follow these steps:

1. Install [.NET 7](https://dotnet.microsoft.com/download/dotnet/7.0).
2. Clone the GitHub repository:

```bash
git clone https://github.com/volkanalkilic/ThingsOn.MQTT.Bench.git

```

3. Navigate to the cloned repository directory:
4. Build the project using the following command:

```bash
dotnet build

```

5. Once the project has built successfully, you can run the benchmark using the following command:

```bash
dotnet run --project ThingsOn.MQTT.Bench

```

## Contributing

Contributions to ThingsOn MQTT Bench are welcome! If you find a bug or would like to suggest a new feature, please open an issue on the [GitHub repository](https://github.com/volkanalkilic/ThingsOn.MQTT.Bench).

If you would like to contribute code to ThingsOn MQTT Bench, please fork the repository and submit a pull request.

## Built With

* [.NET Core](https://dotnet.microsoft.com/) - The .NET framework used
* [MQTTnet](https://github.com/chkr1011/MQTTnet) - The MQTT library used
* [Nett](https://github.com/paiden/Nett) - The TOML library used

## License

This project is licensed under the MIT License - see the [LICENSE.md](https://github.com/volkanalkilic/Mqtt-File-Uploader/blob/main/LICENSE.md) file for details.
