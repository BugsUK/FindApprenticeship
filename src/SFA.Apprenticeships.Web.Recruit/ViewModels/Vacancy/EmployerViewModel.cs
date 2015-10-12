namespace SFA.Apprenticeships.Web.Recruit.ViewModels.Vacancy
{
    using System.ComponentModel.DataAnnotations;
    using Common.ViewModels.Locations;
    using Constants.ViewModels;
    using FluentValidation.Attributes;
    using Validators.Vacancy;

    [Validator(typeof(EmployerViewModelValidator))]
    public class EmployerViewModel
    {
        public string ProviderSiteErn { get; set; }
        public string Ern { get; set; }
        public string Name { get; set; }
        [Display(Name = EmployerViewModelMessages.Description.LabelText)]
        public string Description { get; set; }
        public string WebsiteUrl { get; set; }
        public bool IsWebsiteUrlWellFormed { get; set; }
        public AddressViewModel Address { get; set; }
    }
}