namespace SFA.Apprenticeship.Api.AvService.Mappers.Version51
{
    using System;
    using Apprenticeships.Domain.Interfaces.Queries;
    using DataContracts.Version51;

    public class ApprenticeshipVacancyQueryMapper
    {
        public const int DefaultPageSize = 10;

        public static ApprenticeshipVacancyQuery MapToApprenticeshipVacancyQuery(VacancySearchData criteria)
        {
            // TODO: API: support more query parameters.
            return new ApprenticeshipVacancyQuery
            {
                // OccupationCode = criteria.OccupationCode,
                FrameworkCodeName = criteria.FrameworkCode,
                // VacancyLocationType = criteria.VacancyLocationType,
                // CountyCode = criteria.CountyCode,
                // Town = criteria.Town,
                // RegionCode = criteria.RegionCode,
                // LiveDate = criteria.VacancyPublishedDate,
                CurrentPage = Math.Max(1, criteria.PageIndex),
                PageSize = DefaultPageSize
            };
        }
    }
}
