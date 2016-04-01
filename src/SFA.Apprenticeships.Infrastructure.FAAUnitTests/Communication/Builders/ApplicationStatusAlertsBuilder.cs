namespace SFA.Apprenticeships.Infrastructure.FAAUnitTests.Communication.Builders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Applications;
    using Domain.Entities.Communication;
    using Ploeh.AutoFixture;

    public class ApplicationStatusAlertsBuilder
    {
        private List<ApplicationStatusAlert> _alerts;

        public ApplicationStatusAlertsBuilder()
        {
            _alerts = new List<ApplicationStatusAlert>();
        }

        public ApplicationStatusAlertsBuilder WithApplicationStatusAlerts(int noOfAlerts, ApplicationStatuses applicationStatuses)
        {
            _alerts = new Fixture().Build<ApplicationStatusAlert>()
                .With(a => a.Status, applicationStatuses)
                .With(a => a.DateUpdated, new DateTime(2015, 01, 31))
                .CreateMany(noOfAlerts)
                .OrderBy(p => p.DateUpdated)
                .ToList();

            return this;
        }

        public ApplicationStatusAlertsBuilder WithSpecialCharacterApplicationStatusAlerts(int noOfAlerts, ApplicationStatuses applicationStatuses)
        {
            _alerts = new Fixture().Build<ApplicationStatusAlert>()
                .With(a => a.Status, applicationStatuses)
                .With(a => a.Title, "Tit|e {with} sp~ci(al) ch@r$ in \"t")
                .With(a => a.EmployerName, "\"Emp|ov~r <N@m€>\"")
                .With(a => a.DateUpdated, new DateTime(2015, 01, 31))
                .CreateMany(noOfAlerts)
                .OrderBy(p => p.DateUpdated)
                .ToList();

            return this;
        }

        public List<ApplicationStatusAlert> Build()
        {
            return _alerts;
        }
    }
}