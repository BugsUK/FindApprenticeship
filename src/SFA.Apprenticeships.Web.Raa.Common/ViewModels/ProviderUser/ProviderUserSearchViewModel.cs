namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.ProviderUser
{
    using System.ComponentModel.DataAnnotations;
    using Constants.ViewModels;

    public class ProviderUserSearchViewModel
    {
        [Display(Name = ProviderUserSearchViewModelMessages.Username.LabelText)]
        public string Username { get; set; }
        [Display(Name = ProviderUserSearchViewModelMessages.Name.LabelText)]
        public string Name { get; set; }
        [Display(Name = ProviderUserSearchViewModelMessages.Email.LabelText)]
        public string Email { get; set; }
        [Display(Name = ProviderUserSearchViewModelMessages.AllUnverifiedEmails.LabelText)]
        public bool AllUnverifiedEmails { get; set; }
        public bool PerformSearch { get; set; }
    }
}