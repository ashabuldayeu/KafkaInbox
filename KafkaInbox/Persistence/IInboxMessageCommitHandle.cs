namespace KafkaInbox.Persistence
{
    public interface IInboxMessageCommitHandle
    {
        Task Commit(InboxMessage message, CancellationToken cancellationToken);
    }
}
