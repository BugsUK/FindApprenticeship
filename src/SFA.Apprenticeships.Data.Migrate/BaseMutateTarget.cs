namespace SFA.Apprenticeships.Data.Migrate
{
    using SFA.Infrastructure.Interfaces;
    using System;

    using SFA.Apprenticeships.Application.Interfaces;

    public abstract class BaseMutateTarget
    {
        protected ILogService _log;
        protected ITableDetails _tableDetails;

        public int NumberOfInserts { get; protected set; }
        public int NumberOfUpdates { get; protected set; }
        public int NumberOfDeletes { get; protected set; }
        public int NumberUnchanged { get; protected set; }

        public BaseMutateTarget(ILogService log, ITableDetails tableDetails)
        {
            _log = log;
            _tableDetails = tableDetails;

            NumberOfInserts = 0;
            NumberOfUpdates = 0;
            NumberOfDeletes = 0;
            NumberUnchanged = 0;
        }

        protected string SummaryText
        {
            get
            {
                return $"Summary for {_tableDetails.Name}: {NumberOfInserts} inserts, {NumberOfUpdates} updates, {NumberOfDeletes} deletes, {NumberUnchanged} unchanged";
            }
        }

        protected string SummaryTextDeletes
        {
            get
            {
                return $"{_tableDetails.Name}: {NumberOfDeletes} deletes";
            }
        }

    }
}
