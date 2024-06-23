using Inbox.Mongo.CommonTrash;
using KafkaInbox;
using KafkaInbox.Persistence;
using KafkaInbox.Persistence.Transaction;

namespace Inbox.Mongo
{
    /// <summary>
    /// Wrapper under mongo's IClientHandle
    /// </summary>
    public class MongoInboxTransaction : IInboxTransaction
    {
        private readonly TransactionManager _transactionManager;
        private readonly IInboxMessageCommitHandle inboxMessageCommitHandle;
        public MongoInboxTransaction(TransactionManager transactionManager, IInboxMessageCommitHandle inboxMessageCommitHandle)
        {
            _transactionManager = transactionManager;
            this.inboxMessageCommitHandle = inboxMessageCommitHandle;
        }

        public async Task Commit(InboxMessage inboxMessage, CancellationToken cancellationToken)
        {
            try
            {
                inboxMessage.DtComplete = DateTime.UtcNow;
                await inboxMessageCommitHandle.Commit(inboxMessage, cancellationToken);
                //await _transactionManager.Commit(cancellationToken);
            }
            catch (Exception)
            {
                //await _transactionManager.Abort(cancellationToken);
                throw;
            }
        }

        public void Dispose()
        {
            //_transactionManager.GetClientSession.Dispose();
        }

        public Task Rollback(CancellationToken cancellationToken)
        {
            //  if(_transactionManager.GetClientSession.)
            //return _transactionManager.Abort(cancellationToken);
            return Task.CompletedTask;

        }

        public Task Start(CancellationToken cancellationToken)
        {
            //return _transactionManager.StartTransaction(cancellationToken);
            return Task.CompletedTask;
        }
    }
}
