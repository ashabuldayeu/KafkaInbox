using Inbox.Mongo.CommonTrash.Configs;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
namespace Inbox.Mongo.CommonTrash.Provider
{
    public abstract class MongoDbProviderBase<TDBConfig> : IMongoDbProvider where TDBConfig : IDbConfigSection
    {
        protected readonly IConfiguration _configuration;
        protected readonly MongoClient _client;
        protected MongoDbProviderBase(IConfiguration configuration, MongoClient mongoClient)
        {
            _configuration = configuration;
            _client = mongoClient;
        }

        public IMongoDatabase GetDatabase()
        {
            var config = _configuration.GetSection(WriteMongoDbConfigSection.SectionName)?.Get<WriteMongoDbConfigSection>();
            return _client.GetDatabase(config?.DatabaseName);
        }
    }
}
