namespace SFA.Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Apprenticeships;
    using ReferenceData;

    public static class ApprenticeshipExtensions
    {
        public static Apprenticeships.ApprenticeshipLevel GetApprenticeshipLevel(this ApprenticeshipLevel apprenticeshipLevel)
        {
            switch (apprenticeshipLevel)
            {
                case ApprenticeshipLevel.Unknown:
                    return Apprenticeships.ApprenticeshipLevel.Unknown;
                case ApprenticeshipLevel.Intermediate:
                    return Apprenticeships.ApprenticeshipLevel.Intermediate;
                case ApprenticeshipLevel.Advanced:
                    return Apprenticeships.ApprenticeshipLevel.Advanced;
                case ApprenticeshipLevel.Higher:
                    return Apprenticeships.ApprenticeshipLevel.Higher;
                case ApprenticeshipLevel.FoundationDegree:
                case ApprenticeshipLevel.Degree:
                case ApprenticeshipLevel.Masters:
                    //TODO: Check what degree should map to
                    return Apprenticeships.ApprenticeshipLevel.Higher;
                default:
                    throw new ArgumentException("Apprenticeship Level: " + apprenticeshipLevel + " was not recognized");
            }
        }
    }
}