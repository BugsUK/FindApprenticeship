namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Employer
{
    using Constants.ViewModels;
    using Domain.Entities.Raa.Parties;
    using System.ComponentModel.DataAnnotations;
    using Web.Common.ViewModels.Locations;

    public class EmployerViewModel
    {
        public int EmployerId { get; set; }
        public string EdsUrn { get; set; }
        [Display(Name = EmployerViewModelMessages.FullName.LabelText)]
        public string FullName { get; set; }
        [Display(Name = EmployerViewModelMessages.TradingName.LabelText)]
        public string TradingName { get; set; }
        public AddressViewModel Address { get; set; }
        [Display(Name = EmployerViewModelMessages.Status.LabelText)]
        public EmployerTrainingProviderStatuses Status { get; set; }

        public string OriginalFullName { get; set; }
        public bool IsAnonymousEmployer { get; set; }
    }
}