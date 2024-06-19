using Inbox.Mongo.CommonTrash;
using KafkaInbox;
using KafkaInbox.Persistence.Transaction;

namespace Inbox.Mongo
{
    /// <summary>
    /// Wrapper under mongo's IClientHandle
    /// </summary>
    public class MongoInboxTransaction : IInboxTransaction
    {
        private readonly TransactionManager _transactionManager;

        public MongoInboxTransaction(TransactionManager transactionManager)
        {
            _transactionManager = transactionManager;
        }

        public async Task Commit(InboxMessage inboxMessage, CancellationToken cancellationToken)
        {
            try
            {
                await _transactionManager.GetClientSession.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await _transactionManager.GetClientSession.AbortTransactionAsync();
                throw;
            }
        }

        public void Dispose()
        {
            _transactionManager.GetClientSession.Dispose();
        }

        public async Task Rollback(CancellationToken cancellationToken)
        {
            if(_transactionManager.GetClientSession.)
          await   _transactionManager.GetClientSession.AbortTransactionAsync();
        }

        public Task Start(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
