using MongoDB.Driver;

namespace Inbox.Mongo.CommonTrash.Provider
{
    public interface IMongoDbProvider
    {
        IMongoDatabase GetDatabase();
    }
}
