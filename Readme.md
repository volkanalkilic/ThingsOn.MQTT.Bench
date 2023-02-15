# ThingsOn MQTT Bench

ThingsOn MQTT Bench is a benchmark tool for MQTT brokers. It measures the maximum number of messages that can be sent by the broker in a specified amount of time.

## Features

* Measures the maximum number of messages that can be sent by an MQTT broker in a specified amount of time.
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

To use ThingsOn MQTT Bench, first make sure that you have .NET 7 installed on your system. Then, download the latest release of ThingsOn MQTT Bench for your operating system from the [releases page](https://github.com/yourusername/ThingsOn-MQTT-Bench/releases).

Once you have downloaded the tool, you can run it from the command line by navigating to the directory where the tool is located and running the following command:

<pre><div class="bg-black mb-4 rounded-md"><div class="flex items-center relative text-gray-200 bg-gray-800 px-4 py-2 text-xs font-sans"><button class="flex ml-auto gap-2"><svg stroke="currentColor" fill="none" stroke-width="2" viewBox="0 0 24 24" stroke-linecap="round" stroke-linejoin="round" class="h-4 w-4" height="1em" width="1em" xmlns="http://www.w3.org/2000/svg"><path d="M16 4h2a2 2 0 0 1 2 2v14a2 2 0 0 1-2 2H6a2 2 0 0 1-2-2V6a2 2 0 0 1 2-2h2"></path><rect x="8" y="2" width="8" height="4" rx="1" ry="1"></rect></svg>Copy code</button></div><div class="p-4 overflow-y-auto"><code class="!whitespace-pre hljs">dotnet ThingsOnMqttBench.dll
</code></div></div></pre>

By default, the tool will read its settings from a TOML file named `config.toml` in the same directory as the tool. You can customize the settings by editing this file.

## Settings

The following settings are available in the `config.toml` file:

* `Mqtt`: MQTT broker settings
  * `ServerUri`: URI of the MQTT broker
  * `Port`: Port number of the MQTT broker
  * `MqttVersion`: MQTT protocol version (either "V311" or "V500")
  * `Username`: Username for connecting to the MQTT broker
  * `Password`: Password for connecting to the MQTT broker
  * `ClientIdPrefix`: Prefix for MQTT client IDs
  * `KeepAlivePeriod`: Keep-alive period in seconds
  * `CleanSession`: Whether to use a clean session when connecting to the MQTT broker
  * `MaximumQoS`: Maximum quality of service level for messages
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

<pre><div class="bg-black mb-4 rounded-md"><div class="flex items-center relative text-gray-200 bg-gray-800 px-4 py-2 text-xs font-sans"><span class="">bash</span><button class="flex ml-auto gap-2"><svg stroke="currentColor" fill="none" stroke-width="2" viewBox="0 0 24 24" stroke-linecap="round" stroke-linejoin="round" class="h-4 w-4" height="1em" width="1em" xmlns="http://www.w3.org/2000/svg"><path d="M16 4h2a2 2 0 0 1 2 2v14a2 2 0 0 1-2 2H6a2 2 0 0 1-2-2V6a2 2 0 0 1 2-2h2"></path><rect x="8" y="2" width="8" height="4" rx="1" ry="1"></rect></svg>Copy code</button></div><div class="p-4 overflow-y-auto"><code class="!whitespace-pre hljs language-bash">git clone https://github.com/your-username/ThingsOn-MQTT-Bench.git
</code></div></div></pre>

3. Navigate to the cloned repository directory:

<pre><div class="bg-black mb-4 rounded-md"><div class="flex items-center relative text-gray-200 bg-gray-800 px-4 py-2 text-xs font-sans"><span class="">bash</span><button class="flex ml-auto gap-2"><svg stroke="currentColor" fill="none" stroke-width="2" viewBox="0 0 24 24" stroke-linecap="round" stroke-linejoin="round" class="h-4 w-4" height="1em" width="1em" xmlns="http://www.w3.org/2000/svg"><path d="M16 4h2a2 2 0 0 1 2 2v14a2 2 0 0 1-2 2H6a2 2 0 0 1-2-2V6a2 2 0 0 1 2-2h2"></path><rect x="8" y="2" width="8" height="4" rx="1" ry="1"></rect></svg>Copy code</button></div><div class="p-4 overflow-y-auto"><code class="!whitespace-pre hljs language-bash">cd ThingsOn-MQTT-Bench
</code></div></div></pre>

4. Build the project using the following command:

<pre><div class="bg-black mb-4 rounded-md"><div class="flex items-center relative text-gray-200 bg-gray-800 px-4 py-2 text-xs font-sans"><button class="flex ml-auto gap-2"><svg stroke="currentColor" fill="none" stroke-width="2" viewBox="0 0 24 24" stroke-linecap="round" stroke-linejoin="round" class="h-4 w-4" height="1em" width="1em" xmlns="http://www.w3.org/2000/svg"><path d="M16 4h2a2 2 0 0 1 2 2v14a2 2 0 0 1-2 2H6a2 2 0 0 1-2-2V6a2 2 0 0 1 2-2h2"></path><rect x="8" y="2" width="8" height="4" rx="1" ry="1"></rect></svg>Copy code</button></div><div class="p-4 overflow-y-auto"><code class="!whitespace-pre hljs">dotnet build
</code></div></div></pre>

5. Once the project has built successfully, you can run the benchmark using the following command:

<pre><div class="bg-black mb-4 rounded-md"><div class="flex items-center relative text-gray-200 bg-gray-800 px-4 py-2 text-xs font-sans"><span class="">css</span><button class="flex ml-auto gap-2"><svg stroke="currentColor" fill="none" stroke-width="2" viewBox="0 0 24 24" stroke-linecap="round" stroke-linejoin="round" class="h-4 w-4" height="1em" width="1em" xmlns="http://www.w3.org/2000/svg"><path d="M16 4h2a2 2 0 0 1 2 2v14a2 2 0 0 1-2 2H6a2 2 0 0 1-2-2V6a2 2 0 0 1 2-2h2"></path><rect x="8" y="2" width="8" height="4" rx="1" ry="1"></rect></svg>Copy code</button></div><div class="p-4 overflow-y-auto"><code class="!whitespace-pre hljs language-css">dotnet run --project ThingsOnMqttBench
</code></div></div></pre>


## Contributing

Contributions to ThingsOn MQTT Bench are welcome! If you find a bug or would like to suggest a new feature, please open an issue on the [GitHub repository](https://github.com/yourusername/ThingsOn-MQTT-Bench).

If you would like to contribute code to ThingsOn MQTT Bench, please fork the repository and submit a pull request.

## License

ThingsOn MQTT Bench is licensed under the MIT license. See the [LICENSE](https://chat.openai.com/chat/LICENSE) file for more information.
