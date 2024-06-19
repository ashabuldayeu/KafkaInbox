using Inbox.Mongo.CommonTrash;
using Inbox.Mongo.CommonTrash.Configs;
using Microsoft.Extensions.Configuration;

namespace Inbox.Mongo.CommonTrash.Provider
{
    public class WriteMongoDbProvider : MongoDbProviderBase<WriteMongoDbConfigSection>
    {
        public WriteMongoDbProvider(IConfiguration configuration, WriteConnection writeConnection) : base(configuration, writeConnection)
        {
        }

    }
}
