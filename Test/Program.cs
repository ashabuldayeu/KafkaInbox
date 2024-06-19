using Confluent.Kafka;
using Events;
using KafkaInbox;
using KafkaInbox.Handle;
using KafkaInbox.Persistence;
using Test;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHostedService(s => new InboxConsumer<string, Event>(
      "testing"
    , s.GetRequiredService<IInboxStorage>()
    , new JsonDes()
    , new Confluent.Kafka.ConsumerConfig()
    {
        Acks = Acks.Leader,
        AllowAutoCreateTopics = false,
        BootstrapServers = "localhost:60704",
        GroupId = "api-1",
    }
    , new InboxJobHandle<Event>(
          s.GetRequiredService<IInboxStorage>()
        , "testing"
        , s.GetRequiredService<IInboxMessageProcessor<Event>>())
    ) );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
