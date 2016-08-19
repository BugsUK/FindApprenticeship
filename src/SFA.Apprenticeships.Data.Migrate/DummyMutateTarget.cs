namespace SFA.Apprenticeships.Data.Migrate
{
    using SFA.Infrastructure.Interfaces;
    using System;

    using SFA.Apprenticeships.Application.Interfaces;

    public class DummyMutateTarget : BaseMutateTarget, IMutateTarget
    {
        public DummyMutateTarget(ILogService log, ITableDetails table) : base(log, table)
        {
        }

        public void Insert(dynamic record)
        {
            if (NumberOfInserts++ == 0)
                _log.Info($"First insert into {_tableDetails.Name}: {record}");
        }

        public void Update(dynamic record)
        {
            //if (NumberOfUpdates == 0)
            //  _log.Info($"First update of {_tableDetails.Name}: {record}");
            NumberOfUpdates++;
        }

        public void Delete(dynamic record)
        {
            if (NumberOfDeletes++ == 0)
                _log.Info($"First delete of {_tableDetails.Name}: {record}");
        }

        public void NoChange(dynamic record)
        {
            // if (NumberUnchanged == 0)
            //  _log.Info($"First no change of {_tableDetails.Name}: {record}");
            NumberUnchanged++;
        }


        public void FlushInsertsAndUpdates()
        {
            _log.Info(SummaryText);
        }

        public void FlushDeletes()
        {
            _log.Info(SummaryTextDeletes);
        }
    }
}
