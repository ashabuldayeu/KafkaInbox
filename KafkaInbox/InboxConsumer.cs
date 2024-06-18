using Confluent.Kafka;
using KafkaInbox.Handle;
using KafkaInbox.Persistence;
using Microsoft.Extensions.Hosting;

namespace KafkaInbox
{
    public class InboxConsumer<TKey, TContent> : BackgroundService
    {
        private readonly string _topic;
        private readonly IInboxStorage _inboxStorage;
        private readonly IConsumer<TKey, TContent> _consumer;

        public InboxConsumer(
              string topic
            , IInboxStorage inboxStorage
            , IDeserializer<TContent> deserializer
            , ConsumerConfig consConf
            , InboxJobHandle<TContent> inboxJobHandle)
        {
            _topic = topic;
            _inboxStorage = inboxStorage;

            _consumer = new ConsumerBuilder<TKey, TContent>(consConf)
                .SetValueDeserializer(deserializer)
                .SetPartitionsAssignedHandler(
                    (x, y) => inboxJobHandle.AssignPartitions(y.Select(t => t.Partition.Value).ToList()))
                .SetPartitionsLostHandler(
                    (x, y) => inboxJobHandle.StopPartitions(y.Select(t => t.Partition.Value).ToList()))
                .SetPartitionsRevokedHandler(
                    (x, y) => inboxJobHandle.StopPartitions(y.Select(t => t.Partition.Value).ToList()))
                .Build();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (true)
            {
                _consumer.Subscribe(_topic);

                try
                {
                    while (stoppingToken.IsCancellationRequested is false)
                    {
                        var consRes = _consumer.Consume(stoppingToken);

                        if (consRes is null || consRes.IsPartitionEOF)
                        {
                            continue;
                        }

                        if (consRes.Message.Value is { } @event)
                        {
                            // smth like transaction should be there?
                            await _inboxStorage.InsertAsync(
                                new InboxMessage()
                                {
                                    DtComplete = null,
                                    DtConsumed = DateTime.UtcNow,
                                    EventContent = @event,
                                    Partition = consRes.Partition.Value,
                                    Topic = _topic,
                                    Type = @event.GetType().Name,
                                }
                            , stoppingToken);
                        }

                        _consumer.Commit(consRes);
                    }
                }
                catch (OperationCanceledException)
                {
                    _consumer.Unsubscribe();
                }
                catch (ConsumeException e)
                {
                    if (e.Error.IsFatal)
                    {
                        _consumer.Unsubscribe();
                    }
                }
                catch (Exception e)
                {
                    _consumer.Unsubscribe();
                }

                await Task.Delay(5000);
            }
        }
    }
}
