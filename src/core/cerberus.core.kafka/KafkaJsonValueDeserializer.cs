using System;
using System.Text;
using Confluent.Kafka;
using Newtonsoft.Json;

namespace cerberus.core.kafka
{
    public class KafkaJsonValueDeserializer<T> : IDeserializer<T>
    {
        public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(data));
        }
    }
}