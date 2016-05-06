namespace SFA.Apprenticeships.Infrastructure.Repositories.Applications.IoC
{
    using Domain.Entities.Applications;
    using Domain.Entities.Vacancies;
    using Domain.Entities.Vacancies.Apprenticeships;
    using MongoDB.Bson.Serialization;

    /// <summary>
    /// Responsible for registering a set of Mongo class maps to ignore null (or default)
    /// field values when saving. The intention is to preserve compatibility between the
    /// Mongo- and SQL-based versions of the application.
    /// </summary>
    internal class ApplicationBsonClassMaps
    {
        public void Register()
        {
            BsonClassMap.RegisterClassMap<ApplicationDetail>(classMap =>
            {
                classMap.AutoMap();

                classMap
                    .GetMemberMap(each => each.DateLastViewed)
                    .SetIgnoreIfNull(true);

                classMap
                    .GetMemberMap(each => each.Notes)
                    .SetIgnoreIfNull(true);
            });

            BsonClassMap.RegisterClassMap<ApplicationSummary>(classMap =>
            {
                classMap.AutoMap();

                classMap
                    .GetMemberMap(each => each.CandidateDetails)
                    .SetIgnoreIfNull(true);

                classMap
                    .GetMemberMap(each => each.Notes)
                    .SetIgnoreIfNull(true);
            });

            BsonClassMap.RegisterClassMap<ApprenticeshipApplicationDetail>(classMap =>
            {
                classMap.AutoMap();

                classMap
                    .GetMemberMap(each => each.SuccessfulDateTime)
                    .SetIgnoreIfNull(true);

                classMap
                    .GetMemberMap(each => each.UnsuccessfulDateTime)
                    .SetIgnoreIfNull(true);
            });

            BsonClassMap.RegisterClassMap<ApprenticeshipSummary>(classMap =>
            {
                classMap.AutoMap();

                classMap
                    .GetMemberMap(each => each.WageUnit)
                    .SetDefaultValue(WageUnit.Unknown)
                    .SetIgnoreIfDefault(true);
            });
        }
    }
}
