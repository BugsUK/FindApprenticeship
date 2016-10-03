namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Admin
{
    using System.Collections.Generic;

    public class TransferVacanciesResultsViewModel
    {
        public TransferVacanciesViewModel TransferVacanciesViewModel { get; set; }
        public IList<TransferVacancyViewModel> VacanciesToBeTransferredVm { get; set; }
    }
}