namespace SFA.Apprenticeships.Web.Common.ViewModels.Applications
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Candidate;
    using Constants;
    using Models.Application;
    using Constants.Pages;

    [Serializable]
    public abstract class ApplicationViewModelBase<TCandidateViewModel> : ViewModelBase
        where TCandidateViewModel : CandidateViewModelBase
    {
        public readonly string ConfirmationMessage = ApplicationPageMessages.LeavingPageMessage;
        public readonly int CurrentYear = DateTime.UtcNow.Year;
        public readonly string FourDigitYearRegex = Whitelists.YearRangeWhiteList.RegularExpression();
        public readonly string WhiteListRegex = Whitelists.FreetextWhitelist.RegularExpression;

        public double SessionTimeout;

        protected ApplicationViewModelBase()
            : this(null)
        {
        }

        protected ApplicationViewModelBase(ApplicationViewModelStatus viewModelStatus) :
            this(null, viewModelStatus)
        {
        }

        protected ApplicationViewModelBase(string message, ApplicationViewModelStatus viewModelStatus)
            : this(message)
        {
            ViewModelStatus = viewModelStatus;
        }

        protected ApplicationViewModelBase(string message)
            : base(message)
        {
            DefaultQualificationRows = 5;
            DefaultWorkExperienceRows = 3;
            DefaultTrainingCourseRows = 3;
        }

        public TCandidateViewModel Candidate { get; set; }

        public int DefaultQualificationRows { get; set; }

        public int DefaultWorkExperienceRows { get; set; }

        public int DefaultTrainingCourseRows { get; set; }

        public IEnumerable<SelectListItem> Months
        {
            get
            {
                var list = new List<SelectListItem>
                {
                    new SelectListItem {Selected = false, Text = "Jan", Value = "1"},
                    new SelectListItem {Selected = false, Text = "Feb", Value = "2"},
                    new SelectListItem {Selected = false, Text = "Mar", Value = "3"},
                    new SelectListItem {Selected = false, Text = "Apr", Value = "4"},
                    new SelectListItem {Selected = false, Text = "May", Value = "5"},
                    new SelectListItem {Selected = false, Text = "June", Value = "6"},
                    new SelectListItem {Selected = false, Text = "July", Value = "7"},
                    new SelectListItem {Selected = false, Text = "Aug", Value = "8"},
                    new SelectListItem {Selected = false, Text = "Sept", Value = "9"},
                    new SelectListItem {Selected = false, Text = "Oct", Value = "10"},
                    new SelectListItem {Selected = false, Text = "Nov", Value = "11"},
                    new SelectListItem {Selected = false, Text = "Dec", Value = "12"}
                };

                return new SelectList(list, "Value", "Text");
            }
        }

        public DateTime? DateUpdated { get; set; }

        public int VacancyId { get; set; }

        public bool IsJavascript { get; set; }

        public ApplicationViewModelStatus ViewModelStatus { get; set; }

        public DateTime DateApplied { get; set; }
    }
}