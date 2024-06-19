using KafkaInbox.Persistence.Transaction;

namespace KafkaInbox.Persistence
{
    public interface IInboxStorage
    {
        /// <summary>
        /// SHOULD BE CHECKED IDEMPOTANCY_KEY PROPERTY, SHOULD BE UNIQUE INDEX + TOPIC?
        /// </summary>
        /// <param name="inboxMessage"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task InsertAsync(InboxMessage inboxMessage, CancellationToken cancellationToken);

        /// <summary>
        /// SHOULD BE CHECKED WITH DT_COMPLETE IS NULL!!! (IDEMPOTANCY MAT' EE)
        /// </summary>
        /// <param name="inboxMessage"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task UpdateAsync(InboxMessage inboxMessage, IInboxTransaction transaction, CancellationToken cancellationToken);

        Task<InboxMessage> EarliestAsync(string topic, int partition, CancellationToken cancellationToken);
    }
}
