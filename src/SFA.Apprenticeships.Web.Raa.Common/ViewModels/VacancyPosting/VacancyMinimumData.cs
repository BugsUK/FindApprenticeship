namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.VacancyPosting
{
    using Domain.Entities.Raa.Vacancies;
    using System;

    public struct VacancyMinimumData
    {
        public string Ukprn { get; set; }
        public Guid VacancyGuid { get; set; }
        public int VacancyOwnerRelationshipId { get; set; }
        public VacancyLocationType VacancyLocationType { get; set; }
        public int? NumberOfPositions { get; set; }
        public string EmployerWebsiteUrl { get; set; }
        public string EmployerDescription { get; set; }
        public string AnonymousAboutTheEmployer { get; set; }
        public string AnonymousEmployerDescription { get; set; }
        public string AnonymousEmployerReason { get; set; }
        public bool IsAnonymousEmployer { get; set; }

    }
}