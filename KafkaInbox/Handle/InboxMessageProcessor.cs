using KafkaInbox.Persistence;
using KafkaInbox.Persistence.Transaction;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace KafkaInbox.Handle
{
    public abstract class InboxMessageProcessor<TEvent> : IInboxMessageProcessor<TEvent>
    {
        private readonly IServiceScopeFactory _scopeFactory;

        protected InboxMessageProcessor(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        /// <summary>
        /// Cancellation token is very important here!!!!!!
        /// Should be passed throught all the stack to stop correctly event handling and prevent duplication
        /// </summary>
        /// <param name="event"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Handle(InboxMessage inboxMessage, CancellationToken cancellationToken)
        {
            using var scope = _scopeFactory.CreateScope();
            using IInboxTransaction inboxTransaction = scope.ServiceProvider.GetRequiredService<IInboxTransaction>();
            try
            {
                await inboxTransaction.Start(cancellationToken);
                // 
                await Execute(JsonSerializer.Deserialize<TEvent>(inboxMessage.EventContent), cancellationToken);
                await inboxTransaction.Commit(inboxMessage, cancellationToken);
            }
            catch (Exception e)
            {
                // чет типа насрали в штанцы)
                await inboxTransaction!.Rollback(cancellationToken);
                throw;
            }
        }

        protected abstract Task Execute(TEvent @event, CancellationToken cancellationToken);

    }
}
