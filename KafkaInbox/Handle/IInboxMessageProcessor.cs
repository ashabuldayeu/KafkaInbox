
namespace KafkaInbox.Handle
{
    public interface IInboxMessageProcessor<TEvent>
    {
        Task Handle(InboxMessage inboxMessage, CancellationToken cancellationToken);
    }
}