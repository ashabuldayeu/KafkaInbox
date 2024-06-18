namespace KafkaInbox.Handle
{
    public class InboxHandleUnit
    {
        public InboxHandleUnit(Task handle, CancellationTokenSource cancellationTokenSource, int partition)
        {
            Handle = handle;
            CancellationTokenSource = cancellationTokenSource;
            Partition = partition;
        }

        public Task Handle { get; private set; }

        public int Partition { get; private set; }

        public CancellationTokenSource CancellationTokenSource { get; private set; }
    }
}
