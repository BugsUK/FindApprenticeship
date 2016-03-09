namespace SFA.Apprenticeships.Data.Migrate
{
    public interface IMigrateConfiguration
    {
        string SourceConnectionString { get; }
        string TargetConnectionString { get; }
        int RecordBatchSize { get; }
        int? MaxNumberOfChangesToDetailPerTable { get; }
        bool AnonymiseData { get; }
    }

    public class MigrateFromAvmsConfiguration : IMigrateConfiguration
    {
        public string SourceConnectionString { get; set; }
        public string TargetConnectionString { get; set; }
        public int    RecordBatchSize { get; set; }
        public int? MaxNumberOfChangesToDetailPerTable { get; set; }
        public bool AnonymiseData { get; set; }
    }
}
