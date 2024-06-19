using MongoDB.Driver;

namespace Inbox.Mongo.CommonTrash
{
    public class ReadOnlyConnection : MongoClient
    {
        public ReadOnlyConnection(MongoClientSettings settings) : base(settings)
        {
        }

        public ReadOnlyConnection(MongoUrl url) : base(url)
        {
        }

        public ReadOnlyConnection(string connectionString) : base(connectionString)
        {
        }
    }
}
