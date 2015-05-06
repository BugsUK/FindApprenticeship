﻿namespace SFA.Apprenticeships.Web.Candidate.ViewModels.Applications
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Common.Constants;
    using Common.Models.Application;
    using Constants.Pages;

    [Serializable]
    public abstract class ApplicationViewModelBase : ViewModelBase
    {
        //Constants used on application form
        public string ConfirmationMessage = ApplicationPageMessages.LeavingPageMessage;
        public int CurrentYear = DateTime.Now.Year;
        public int DefaultQualificationRows = 5;
        public int DefaultWorkExperienceRows = 3;
        public string FourDigitYearRegex = Whitelists.YearRangeWhiteList.RegularExpression();
        public double SessionTimeout;
        public string WhiteListRegex = Whitelists.FreetextWhitelist.RegularExpression;

        protected ApplicationViewModelBase()
        {
        }

        protected ApplicationViewModelBase(string message, ApplicationViewModelStatus viewModelStatus) : base(message)
        {
            ViewModelStatus = viewModelStatus;
        }

        protected ApplicationViewModelBase(string message)
            : base(message)
        {
        }

        protected ApplicationViewModelBase(ApplicationViewModelStatus viewModelStatus)
        {
            ViewModelStatus = viewModelStatus;
        }

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