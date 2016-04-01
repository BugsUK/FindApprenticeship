namespace SFA.Apprenticeships.Infrastructure.FAAUnitTests.Communication.Builders
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Communication;
    using Ploeh.AutoFixture;

    public class SavedSearchAlertsBuilder
    {
        private List<SavedSearchAlert> _savedSearchAlerts;

        public SavedSearchAlertsBuilder()
        {
            _savedSearchAlerts = new List<SavedSearchAlert>();
        }

        public SavedSearchAlertsBuilder SavedSearchAlerts(int savedSearchAlertCount)
        {
            _savedSearchAlerts = new Fixture().Build<SavedSearchAlert>()
                .CreateMany(savedSearchAlertCount)
                .ToList();

            return this;
        }

        public List<SavedSearchAlert> Build()
        {
            return _savedSearchAlerts;
        }
    }
}