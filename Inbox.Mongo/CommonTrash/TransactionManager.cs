using MongoDB.Driver;

namespace Inbox.Mongo.CommonTrash
{
    public class TransactionManager
    {
        private object _lock = new object();
        private IClientSessionHandle clientSessionHandle;
        //public IClientSessionHandle GetClientSession
        //{
        //    get
        //    {
        //        if(clientSessionHandle == null)
        //        {
        //            lock (_lock)
        //            {
        //                if(clientSessionHandle is null)
        //                {
        //                    clientSessionHandle = new IClientSessionHandle();
        //                }
        //            }
        //        }

        //        return clientSessionHandle;
        //    }
        //}
    }
}
