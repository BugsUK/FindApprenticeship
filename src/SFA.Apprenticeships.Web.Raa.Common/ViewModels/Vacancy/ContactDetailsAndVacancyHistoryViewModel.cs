namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy
{
    using System;

    public class ContactDetailsAndVacancyHistoryViewModel
    {
        public string FullName { get; set; }

        public string ProviderName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime? DateFirstSubmitted { get; set; }

        public DateTime? DateLastUpdated { get; set; }
    }
}