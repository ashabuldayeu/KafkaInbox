using Inbox.Mongo.CommonTrash;
using Inbox.Mongo.CommonTrash.Configs;
using Microsoft.Extensions.Configuration;

namespace Inbox.Mongo.CommonTrash.Provider
{
    public class ReadOnlyMongoDbProvider : MongoDbProviderBase<ReadMongoDbConfigSection>
    {
        public ReadOnlyMongoDbProvider(IConfiguration configuration, ReadOnlyConnection readOnlyConnection)
            : base(configuration, readOnlyConnection)
        {
        }
    }
}
