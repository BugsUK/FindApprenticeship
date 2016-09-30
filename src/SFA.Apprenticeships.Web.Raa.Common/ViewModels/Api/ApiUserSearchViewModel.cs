namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Api
{
    using System.ComponentModel.DataAnnotations;
    using Constants.ViewModels;

    public class ApiUserSearchViewModel
    {
        [Display(Name = ApiSearchViewModelMessages.ExternalSystemId.LabelText)]
        public string ExternalSystemId { get; set; }
        [Display(Name = ApiSearchViewModelMessages.Id.LabelText)]
        public string Id { get; set; }
        [Display(Name = ApiSearchViewModelMessages.Name.LabelText)]
        public string Name { get; set; }

        public bool PerformSearch { get; set; }
    }
}