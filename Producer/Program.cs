// See https://aka.ms/new-console-template for more information
using Confluent.Kafka;
using Events;
using System.Text.Json;

const string topic = "testing";
var config = new ProducerConfig
{
    // User-specific properties that you must set
    BootstrapServers = "localhost:60704",

    // Fixed properties
    Acks = Acks.All
};

using (var producer = new ProducerBuilder<string, string>(config).Build())
{
    var numProduced = 0;
    Random rnd = new Random();
    const int numMessages = 2000;
    for (int i = 0; i < numMessages; ++i)
    {
        var key = rnd.Next(1, 5).ToString();
        var value = JsonSerializer.Serialize(
            new Event()
            {
                Name = "test_" + i
            });
        producer.Produce(topic, new Message<string, string> { Key = key, Value = value },

            (deliveryReport) =>
            {
                if (deliveryReport.Error.Code != ErrorCode.NoError)
                {
                    Console.WriteLine($"Failed to deliver message: {deliveryReport.Error.Reason}");
                }
                else
                {
                    Console.WriteLine($"Produced event to topic {topic}: key = {key} value = {value}");
                    numProduced += 1;
                }
            });
    }

    producer.Flush(TimeSpan.FromSeconds(10));
    Console.WriteLine($"{numProduced} messages were produced to topic {topic}");
}