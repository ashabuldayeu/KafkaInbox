namespace Inbox.Mongo.CommonTrash.Configs
{
    public record ReadMongoDbConfigSection : IDbConfigSection
    {
        public static string SectionName => "ReadMongo";

        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }
    }
}
