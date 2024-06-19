using Inbox.Mongo.CommonTrash;
using Inbox.Mongo.CommonTrash.Provider;
using KafkaInbox;
using KafkaInbox.Persistence;
using KafkaInbox.Persistence.Transaction;
using MongoDB.Driver;

namespace Inbox.Mongo
{
    public class InboxStorage : MongoRepositoryBase<InboxMessage>, IInboxStorage
    {
        public InboxStorage(IMongoDbProvider mongoDbProvider) : base(mongoDbProvider)
        {
        }

        public Task<InboxMessage> EarliestAsync(string topic, int partition, CancellationToken cancellationToken)
        {
            return _collection.Find(x => 
                       x.Topic == topic 
                    && x.Partition == partition 
                    && x.DtComplete == null)
                .FirstOrDefaultAsync();
        }

        public Task InsertAsync(InboxMessage inboxMessage, CancellationToken cancellationToken)
        {
            return _collection.InsertOneAsync(inboxMessage);
        }

        public Task UpdateAsync(InboxMessage inboxMessage, IInboxTransaction transaction, CancellationToken cancellationToken)
        {
            throw new();
        }
    }
}
