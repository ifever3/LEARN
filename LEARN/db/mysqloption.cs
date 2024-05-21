namespace LEARN.db
{
    public class mysqloption
    {
        public const string MySql = nameof(MySql);
        public string ConnectionString { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
    }
}
