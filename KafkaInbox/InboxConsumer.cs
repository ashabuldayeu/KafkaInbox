using Confluent.Kafka;
using KafkaInbox.Handle;
using KafkaInbox.Persistence;
using Microsoft.Extensions.Hosting;
using System.Text.Json;

namespace KafkaInbox
{
    public class InboxConsumer<TKey, TContent> : BackgroundService
    {
        private readonly string _topic;
        private readonly IInboxStorage _inboxStorage;
        private readonly IConsumer<TKey, TContent> _consumer;
        private int _cons_index;
        public InboxConsumer(
              string topic
            , IInboxStorage inboxStorage
            , IDeserializer<TContent> deserializer
            , ConsumerConfig consConf
            , InboxJobHandle<TContent> inboxJobHandle
            , int cons_index)
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
            _cons_index = cons_index;
        }

        public override void Dispose()
        {
            try
            {
                _consumer?.Unsubscribe();
            }
            catch (Exception)
            {

            }
            finally
            {
                base.Dispose();
            }
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
                                    EventContent = JsonSerializer.Serialize(@event),
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
