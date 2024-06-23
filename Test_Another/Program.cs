using Confluent.Kafka;
using Events;
using Inbox.Mongo.CommonTrash.Configs;
using Inbox.Mongo.CommonTrash.Provider;
using Inbox.Mongo.CommonTrash;
using Inbox.Mongo;
using KafkaInbox.Handle;
using KafkaInbox.Persistence.Transaction;
using KafkaInbox.Persistence;
using KafkaInbox;
using Test_Another;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton(x => new WriteConnection(builder.Configuration.GetSection(WriteMongoDbConfigSection.SectionName).Get<WriteMongoDbConfigSection>().ConnectionString));
builder.Services.AddSingleton<WriteMongoDbProvider>();
builder.Services.AddSingleton<IInboxStorage, InboxStorage>();

builder.Services.AddScoped<IInboxMessageCommitHandle, InboxMessageCommitHandle>();
builder.Services.AddScoped<IInboxTransaction, MongoInboxTransaction>();
builder.Services.AddScoped<TransactionManager>();
builder.Services.AddSingleton<IInboxMessageProcessor<Event>, EventInboxProc>();
builder.Services.AddHostedService(s => new InboxConsumer<string, Event>(
      "testing1"
    , s.GetRequiredService<IInboxStorage>()
    , new JsonDes()
    , new Confluent.Kafka.ConsumerConfig()
    {
        AllowAutoCreateTopics = false,
        BootstrapServers = "localhost:60704",
        GroupId = "api-1",
        AutoOffsetReset = AutoOffsetReset.Earliest,
        EnableAutoCommit = false
    }
    , new InboxJobHandle<Event>(
          s.GetRequiredService<IInboxStorage>()
        , "testing1"
        , s.GetRequiredService<IInboxMessageProcessor<Event>>()
        , 1)
    , 1
    ));
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
