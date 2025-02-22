using Confluent.Kafka;
using Microsoft.Extensions.Options;
using OrderService.Application.Interfaces;
using System.Text.Json;

namespace OrderService.Infrastructure.Messaging.Producers
{
    public class KafkaProducer<TMessage> : IKafkaProducer<TMessage>
    {
        private readonly IProducer<string, string> _producer;
        private readonly string _topic;

        public KafkaProducer(IOptions<KafkaSettings> kafkaSettings)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = kafkaSettings.Value.BootstrapServers
            };

            _producer = new ProducerBuilder<string, string>(config).Build();

            _topic = kafkaSettings.Value.Topic;
        }

        public async Task ProduceAsync(TMessage message, CancellationToken cancellationToken)
        {
            var jsonMessage = JsonSerializer.Serialize(message);

            await _producer.ProduceAsync(_topic, new Message<string, string>
            {
                Key = Guid.NewGuid().ToString(),
                Value = jsonMessage
            }, cancellationToken);
        }

        public void Dispose()
        {
            _producer.Dispose();
        }
    }
}
