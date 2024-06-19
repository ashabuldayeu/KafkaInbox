using Events;
using KafkaInbox.Handle;

namespace Test
{
    public class EventInboxProc : InboxMessageProcessor<Event>
    {
        public EventInboxProc(IServiceScopeFactory scopeFactory) : base(scopeFactory)
        {
        }

        protected async override Task Execute(Event @event, CancellationToken cancellationToken)
        {
            Console.WriteLine(@event.Name);
        }
    }
}
