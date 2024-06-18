namespace KafkaInbox.Persistence.Transaction
{
    public interface IInboxTransaction : IDisposable
    {
        Task Start(CancellationToken cancellationToken);
        Task Commit(CancellationToken cancellationToken);
        Task Rollback(CancellationToken cancellationToken);
    }
}
