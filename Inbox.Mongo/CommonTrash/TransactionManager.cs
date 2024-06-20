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

        public async Task StartTransaction()
        {
            if (Transaction is { })
                return;

            Transaction = await _writeMongoDbProvider.Client.StartSessionAsync();

            Transaction.StartTransaction();
        }
       
        public async Task Commit()
        {
            if (Transaction is null)
                return;

            await Transaction.CommitTransactionAsync();
            Transaction = null;
        }

        public async Task Abort()
        {
            if(Transaction is null) return;

            await Transaction.AbortTransactionAsync();
            Transaction = null;
        }
    }
}
