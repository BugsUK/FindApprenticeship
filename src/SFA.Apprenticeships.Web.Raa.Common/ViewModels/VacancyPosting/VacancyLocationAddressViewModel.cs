namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.VacancyPosting
{
    using FluentValidation.Attributes;
    using Recruit.Validators.VacancyPosting;
    using Web.Common.ViewModels.Locations;

    [Validator(typeof(VacancyLocationAddressViewModelValidator))]
    public class VacancyLocationAddressViewModel
    {
        public VacancyLocationAddressViewModel()
        {
            Address = new AddressViewModel();
        }

        public AddressViewModel Address { get; set; } 

        public int? NumberOfPositions { get; set; }
    }
}