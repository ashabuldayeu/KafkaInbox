using Inbox.Mongo.CommonTrash.Provider;
using MongoDB.Driver;

namespace Inbox.Mongo.CommonTrash
{
    public abstract class MongoRepositoryBase<TEntity>
    {
        protected readonly IMongoCollection<TEntity> _collection;
        protected MongoRepositoryBase(IMongoDbProvider mongoDbProvider)
        {
            _collection = mongoDbProvider.GetDatabase().GetCollection<TEntity>(typeof(TEntity).Name);
        }

        // add some base operations
    }
}
