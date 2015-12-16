namespace SFA.Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship
{
    using System;
    using System.Collections.Generic;
    using Locations;

    public class ApprenticeshipVacancy : Vacancy
    {
        public TrainingType TrainingType { get; set; }

        public ApprenticeshipLevel ApprenticeshipLevel { get; set; }

        public string ApprenticeshipLevelComment { get; set; }

        public string FrameworkCodeName { get; set; }

        public string FrameworkCodeNameComment { get; set; }

        public int? StandardId { get; set; }

        public string StandardIdComment { get; set; }

        public ProviderVacancyStatuses Status { get; set; }
        public string WageComment { get; set; }
        public string ClosingDateComment { get; set; }
        public string DurationComment { get; set; }
        public string LongDescriptionComment { get; set; }
        public string PossibleStartDateComment { get; set; }
        public string WorkingWeekComment { get; set; }
        public string FirstQuestionComment { get; set; }
        public string SecondQuestionComment { get; set; }

        public string AdditionalLocationInformation { get; set; }

        public List<VacancyLocationAddress> LocationAddresses { get; set; }
        public bool? IsEmployerLocationMainApprenticeshipLocation { get; set; }

        public int? NumberOfPositions { get; set; }
        public string EmployerDescriptionComment { get; set; }
        public string EmployerWebsiteUrlComment { get; set; }
        public string LocationAddressesComment { get; set; }
        public string NumberOfPositionsComment { get; set; }
    }
}
