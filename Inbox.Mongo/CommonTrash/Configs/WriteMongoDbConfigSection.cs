namespace Inbox.Mongo.CommonTrash.Configs
{
    public record WriteMongoDbConfigSection : IDbConfigSection
    {
        public static string SectionName => "WriteMongo";

        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }
        // other params'll be added futher, I hope:)
    }
}
