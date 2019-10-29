namespace ElectionResults.Core.Storage
{
    public class AppConfig
    {
        public string TableName { get; set; }
        public string BucketName { get; set; }
        public string JobTimer { get; set; }
        public string ElectionsConfig { get; set; }
    }
}