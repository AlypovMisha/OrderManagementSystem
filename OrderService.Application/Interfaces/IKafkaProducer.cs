namespace OrderService.Application.Interfaces
{
    public interface IKafkaProducer<in TMessage> : IDisposable
    {
        Task ProduceAsync(TMessage message, CancellationToken cancellationToken);
    }
}
