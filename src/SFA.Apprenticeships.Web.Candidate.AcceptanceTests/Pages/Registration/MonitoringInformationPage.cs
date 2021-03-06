﻿namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Pages.Registration
{
    using global::SpecBind.Pages;
    using OpenQA.Selenium;

    [PageNavigation("/tellusmore")]
    [PageAlias("MonitoringInformationPage")]
    public class MonitoringInformationPage : BaseValidationPage
    {
        public MonitoringInformationPage(ISearchContext context) : base(context)
        {
        }

        #region Monitoring Information

        #region Gender

        [ElementLocator(Id = "gender-male")]
        public IWebElement GenderMale { get; set; }

        [ElementLocator(Id = "gender-female")]
        public IWebElement GenderFemale { get; set; }

        [ElementLocator(Id = "gender-other")]
        public IWebElement GenderOther { get; set; }

        [ElementLocator(Id = "gender-prefer-not-to-say")]
        public IWebElement GenderPreferNotToSay { get; set; }

        #endregion

        #region Disability

        [ElementLocator(Id = "disability-yes")]
        public IWebElement DisabilityYes { get; set; }

        [ElementLocator(Id = "disability-no")]
        public IWebElement DisabilityNo { get; set; }

        [ElementLocator(Id = "disability-prefer-not-to-say")]
        public IWebElement DisabilityPreferNotToSay { get; set; }

        [ElementLocator(Id = "disability-support-yes")]
        public IWebElement WantSupportInInterview { get; set; }

        [ElementLocator(Id = "disability-support-no")]
        public IWebElement DontWantSupportInInterview { get; set; }

        [ElementLocator(Id = "AnythingWeCanDoToSupportYourInterview")]
        public IWebElement SupportDetails { get; set; }

        #endregion

        #region Ethnicity

        [ElementLocator(Id = "Ethnicity")]
        public IWebElement EthnicDropdown { get; set; }

        #endregion

        #endregion

        [ElementLocator(Id = "save-continue-button")]
        public IWebElement SaveAndContinueButton { get; set; }

        [ElementLocator(Id = "skip-link")]
        public IWebElement SkipLink { get; set; }
    }
}