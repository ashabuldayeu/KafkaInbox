using Confluent.Kafka;
using Events;
using System.Text;
using System.Text.Json;

namespace Test
{
    public class JsonDes : IDeserializer<Event>
    {
        public Event Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            return JsonSerializer.Deserialize<Event>(Encoding.UTF8.GetString(data));
        }
    }
}
