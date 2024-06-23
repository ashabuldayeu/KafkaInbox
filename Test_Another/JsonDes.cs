using System.Text.Json;
using System.Text;
using Events;
using Confluent.Kafka;

namespace Test_Another
{
    public class JsonDes : IDeserializer<Event>
    {
        public Event Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            return JsonSerializer.Deserialize<Event>(Encoding.UTF8.GetString(data));
        }
    }
}
