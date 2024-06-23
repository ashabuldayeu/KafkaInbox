namespace KafkaInbox
{
    public class InboxMessage
    {
        /// <summary>
        /// Mean smth like ext id
        /// </summary>
        public string IdempotancyKey { get; set; }
        public Guid Id { get; set; }

        public string Type { get; set; }

        public string EventContent { get; set; }

        public int Partition { get; set; }

        public string Topic { get; set; }

        public DateTime? DtComplete { get; set; }
        public DateTime DtConsumed { get; set; }
    }
}
