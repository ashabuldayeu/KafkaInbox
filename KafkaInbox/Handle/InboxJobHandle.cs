using KafkaInbox.Persistence;

namespace KafkaInbox.Handle
{
    public class InboxJobHandle<TContent>
    {
        // NO EF CORE IN SUCH CASE OR THINK OF CUSTOM LOGIC
        private int cons_index_test;
        private readonly IInboxStorage _inboxStorage;
        private readonly string _topic;
        protected readonly IInboxMessageProcessor<TContent> _inboxMessageHandler;
        // here should be unique stopping token for each task
        private Dictionary<int, InboxHandleUnit> _partitionsHandlers = [];

        public InboxJobHandle(IInboxStorage inboxStorage, string topic, IInboxMessageProcessor<TContent> inboxMessageHandler, int index)
        {
            _inboxStorage = inboxStorage;
            _topic = topic;
            _inboxMessageHandler = inboxMessageHandler;
            cons_index_test = index;
        }

        public void AssignPartitions(IEnumerable<int> partitions)
        {
            // for stopping - cancellation / wait until cancellation?

            var partitionsSet = partitions.ToHashSet();

            var stoppingUnits = _partitionsHandlers
                .Where(x => partitionsSet.Contains(x.Key))
                .Select(x => x.Value);

            StopHandling(stoppingUnits);

            var newPartitions = partitionsSet.Where(x => _partitionsHandlers.ContainsKey(x) is false);

            foreach (var partition in newPartitions)
            {
                // Run new partition
                RegisterNewPartition(partition);
            }
        }

        public void StopPartitions(IEnumerable<int> stoppingPartitions)
        {
            // cancellation for the all stopping partitions

            StopHandling(_partitionsHandlers
                .Where(x => stoppingPartitions.Contains(x.Key))
                .Select(x => x.Value));
        }

        private void StopHandling(IEnumerable<InboxHandleUnit> stoppingUnits)
        {
            foreach (var unit in stoppingUnits)
            {
                // stop now working partitions
                unit.CancellationTokenSource.Cancel();

                // remove from task dictionary and get rib of such partitions
                _partitionsHandlers.Remove(unit.Partition);
            }
        }

        private void RegisterNewPartition(int partition)
        {
            if (_partitionsHandlers.ContainsKey(partition))
            {
                // error?
                return; 
            }

            var cancellationSource = new CancellationTokenSource();

            // go consuming messages from db 
            var consumingTask = Task.Factory.StartNew(
                  () => EndlessConsumingLoop(partition, cancellationSource.Token)
                , cancellationSource.Token
                , TaskCreationOptions.LongRunning
                , TaskScheduler.Current);

            var unit = new InboxHandleUnit(consumingTask, cancellationSource, partition);

            _partitionsHandlers.Add(partition, unit);
            Console.WriteLine($"Added cons partition {partition} , index of cons {cons_index_test}");
        }

        protected async Task EndlessConsumingLoop(int partition, CancellationToken cancellationToken)
        {
            try
            {
                while (cancellationToken.IsCancellationRequested is false)
                {
                    // rate limiter?
                    await SendEarliestMessageToHandle(partition, cancellationToken);
                    await Task.Delay(50);
                }
            }
            catch (TaskCanceledException)
            {
                // mean it "good" and expected exception so no worries, just can be logged
                return;
            }
        }

        // should be "endless"
        private async Task SendEarliestMessageToHandle(int partition, CancellationToken cancellationToken)
        {
            var inboxMessage = await _inboxStorage.EarliestAsync(_topic, partition, cancellationToken);

            if(inboxMessage is null)
            {
                return;
            }

            await _inboxMessageHandler.Handle(inboxMessage, cancellationToken);
        }
    }
}
