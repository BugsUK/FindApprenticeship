namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Raa.Vacancies;
    using VacancyPosting;
    using Web.Common.ViewModels;
    using Web.Common.ViewModels.Locations;

    public class VacancySummaryViewModel
    {
        public int VacancyId { get; set; }
        public int VacancyReferenceNumber { get; set; }
        public VacancyType VacancyType { get; set; }
        public VacancyStatus Status { get; set; }
        public string Title { get; set; }
        public int OwnerPartyId { get; set; }
        public string ProviderName { get; set; }
        public string EmployerName { get; set; }
        public AddressViewModel Location { get; set; }
        public bool? OfflineVacancy { get; set; }
        public int ApplicationCount { get; set; }
        public int OfflineApplicationClickThroughCount { get; set; }
        public DateViewModel ClosingDate { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public int SubmissionCount { get; set; }
        public bool? IsEmployerLocationMainApprenticeshipLocation { get; set; }
        public List<VacancyLocationAddressViewModel> LocationAddresses { get; set; }
        public int? ParentVacancyId { get; set; }
    }
}