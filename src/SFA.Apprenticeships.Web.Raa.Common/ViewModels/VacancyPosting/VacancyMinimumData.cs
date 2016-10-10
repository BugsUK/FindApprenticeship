namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.VacancyPosting
{
    using System;

    public struct VacancyMinimumData
    {
        public string Ukprn { get; set; }
        public Guid VacancyGuid { get; set; }
        public int VacancyOwnerRelationshipId { get; set; }
        public bool IsEmployerLocationMainApprenticeshipLocation { get; set; }
        public int? NumberOfPositions { get; set; }
        public string EmployerWebsiteUrl { get; set; }
        public string EmployerDescription { get; set; }
    }
}