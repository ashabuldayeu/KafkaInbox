using Inbox.Mongo.CommonTrash;
using Inbox.Mongo.CommonTrash.Provider;
using KafkaInbox;
using KafkaInbox.Persistence;

namespace Inbox.Mongo
{
    public class InboxMessageCommitHandle : MongoRepositoryBase<InboxMessage>, IInboxMessageCommitHandle
    {
        public InboxMessageCommitHandle(IMongoDbProvider mongoDbProvider) : base(mongoDbProvider)
        {
        }

        public Task Commit(InboxMessage message, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
