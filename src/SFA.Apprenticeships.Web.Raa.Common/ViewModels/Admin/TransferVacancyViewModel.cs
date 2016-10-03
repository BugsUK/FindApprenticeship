namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Admin
{
    public class TransferVacancyViewModel
    {
        public bool Selected { get; set; }
        public int VacancyReferenceNumber { get; set; }
        public int ContractOwnerId { get; set; }
        public string ProviderName { get; set; }
        public int? VacancyManagerId { get; set; }
        public string ProviderSiteName { get; set; }
        public int VacancyOwnerRelationShipId { get; set; }
        public int? DeliveryOrganisationId { get; set; }
    }
}