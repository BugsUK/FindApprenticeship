namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Api
{
    using System.ComponentModel.DataAnnotations;
    using Constants.ViewModels;

    public class ApiUserSearchViewModel
    {
        [Display(Name = ApiUserSearchViewModelMessages.ExternalSystemId.LabelText)]
        public string ExternalSystemId { get; set; }
        [Display(Name = ApiUserSearchViewModelMessages.Id.LabelText)]
        public string Id { get; set; }
        [Display(Name = ApiUserSearchViewModelMessages.Name.LabelText)]
        public string Name { get; set; }

        public bool PerformSearch { get; set; }
    }
}