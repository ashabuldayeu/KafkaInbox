using MongoDB.Driver;

namespace Inbox.Mongo.CommonTrash
{
    public class WriteConnection : MongoClient
    {
        public WriteConnection(MongoClientSettings settings) : base(settings)
        {
        }

        public WriteConnection(MongoUrl url) : base(url)
        {
        }

        public WriteConnection(string connectionString) : base(connectionString)
        {
        }
    }
}
