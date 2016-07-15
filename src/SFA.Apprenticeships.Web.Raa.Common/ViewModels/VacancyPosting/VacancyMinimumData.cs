namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.VacancyPosting
{
    using System;

    public struct VacancyMinimumData
    {
        public string Ukprn { get; set; }
        public Guid VacancyGuid { get; set; }
        public int VacancyPartyId { get; set; }
        public bool IsEmployerLocationMainApprenticeshipLocation { get; set; }
        public int? NumberOfPosition { get; set; }
    }
}