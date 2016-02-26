namespace SFA.Apprenticeships.Web.Candidate.ViewModels.Account
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Security.Policy;
    using Common.ViewModels;
    using Constants.ViewModels;

    public class SavedSearchViewModel : ViewModelBase
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Url SearchUrl { get; set; }
        
        [Display(Name = SettingsViewModelMessages.SavedSearch.AlertsEnabledLabelText)]
        public bool AlertsEnabled { get; set; }
        
        public string ApprenticeshipLevel { get; set; }

        public string SubCategoriesFullNames { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateProcessed { get; set; }
    }
}