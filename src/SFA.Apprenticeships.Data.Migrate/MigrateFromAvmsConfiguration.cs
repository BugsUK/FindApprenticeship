using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.Apprenticeships.Data.Migrate
{
    public interface IMigrateConfiguration
    {
        string SourceConnectionString { get; }
        string TargetConnectionString { get; }
        int RecordBatchSize { get; }
    }

    public class MigrateFromAvmsConfiguration : IMigrateConfiguration
    {
        public string SourceConnectionString { get; set; }
        public string TargetConnectionString { get; set; }
        public int    RecordBatchSize { get; set; }
    }
}
