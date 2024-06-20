using Inbox.Mongo.CommonTrash;
using Inbox.Mongo.CommonTrash.Provider;
using KafkaInbox;
using KafkaInbox.Persistence;
using MongoDB.Driver;
namespace Inbox.Mongo
{
    public class InboxMessageCommitHandle : MongoRepositoryBase<InboxMessage>, IInboxMessageCommitHandle
    {
        public InboxMessageCommitHandle(WriteMongoDbProvider mongoDbProvider) : base(mongoDbProvider)
        {
        }

        public Task Commit(InboxMessage message, CancellationToken cancellationToken)
        {
            return _collection.ReplaceOneAsync(x => x.Id == message.Id, message, cancellationToken: cancellationToken);
        }
    }
}
