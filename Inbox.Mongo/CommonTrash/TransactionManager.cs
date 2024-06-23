using Confluent.Kafka;
using Inbox.Mongo.CommonTrash.Provider;
using MongoDB.Driver;

namespace Inbox.Mongo.CommonTrash
{
    public class TransactionManager
    {
        private readonly WriteMongoDbProvider _writeMongoDbProvider;

        public TransactionManager(WriteMongoDbProvider writeMongoDbProvider)
        {
            _writeMongoDbProvider = writeMongoDbProvider;
        }
        public IClientSessionHandle Transaction { get; private set; }

        public async Task StartTransaction(CancellationToken cancellationToken)
        {
            if (Transaction is { })
                return;

            Transaction = await _writeMongoDbProvider.Client.StartSessionAsync(cancellationToken: cancellationToken);

            Transaction.StartTransaction();
        }
       
        public async Task Commit(CancellationToken cancellationToken)
        {
            if (Transaction is null)
                return;

            await Transaction.CommitTransactionAsync(cancellationToken);
            Transaction = null;
        }

        public async Task Abort(CancellationToken cancellationToken)
        {
            if(Transaction is null) return;

            await Transaction.AbortTransactionAsync(cancellationToken);
            Transaction = null;
        }
    }
}
