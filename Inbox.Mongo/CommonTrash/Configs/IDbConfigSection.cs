namespace Inbox.Mongo.CommonTrash.Configs
{
    public interface IDbConfigSection
    {
        public static string SectionName { get; }

        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }
    }
}
