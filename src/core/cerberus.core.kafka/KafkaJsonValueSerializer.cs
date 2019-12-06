using System.Text;
using Confluent.Kafka;
using Newtonsoft.Json;

namespace cerberus.core.kafka
{
    public class KafkaJsonValueSerializer<T> : ISerializer<T>
    {
        public byte[] Serialize(T data, SerializationContext context)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
        }
    }
}