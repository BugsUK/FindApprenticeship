﻿namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Pages.TraineeshipApplication
{
    using System.Globalization;
    using System.Linq;
    using Application;
    using ApprenticeshipApplication;
    using OpenQA.Selenium;
    using SpecBind.Pages;
    using SpecBind.Selenium;

    [PageNavigation("/traineeship/apply/[0-9]+")]
    [PageAlias("TraineeshipApplicationPage")]
    public class TraineeshipApplicationPage : BaseValidationPage
    {
        public TraineeshipApplicationPage(ISearchContext context)
            : base(context)
        {
        }

        #region Save and apply

        [ElementLocator(Id = "applicationSavedTopMessage")]
        public IWebElement ApplicationSavedMessage { get; set; }

        [ElementLocator(Id = "apply-button")]
        public IWebElement ApplyButton { get; set; }

        [ElementLocator(Id = "save-button")]
        public IWebElement SaveButton { get; set; }

        #endregion

        #region General information

        [ElementLocator(Id = "vacancy-title")]
        public IWebElement VacancyTitle { get; set; }

        [ElementLocator(Id = "vacancy-sub-title")]
        public IWebElement VacancySubTitle { get; set; }

        [ElementLocator(Id = "vacancy-summary")]
        public IWebElement VacancySummary { get; set; }

        [ElementLocator(Id = "Fullname")]
        public IWebElement Fullname { get; set; }

        [ElementLocator(Id = "candidate-fullname")]
        public IWebElement FullnameReadOnly { get; set; }

        [ElementLocator(Id = "candidate-email")]
        public IWebElement EmailReadOnly { get; set; }

        [ElementLocator(Id = "candidate-dob")]
        public IWebElement DobReadOnly { get; set; }

        [ElementLocator(Id = "candidate-phone")]
        public IWebElement PhoneReadOnly { get; set; }

        [ElementLocator(Id = "candidate-address-line1")]
        public IWebElement AddressLine1ReadOnly { get; set; }

        [ElementLocator(Id = "candidate-address-line2")]
        public IWebElement AddressLine2ReadOnly { get; set; }

        [ElementLocator(Id = "candidate-address-line3")]
        public IWebElement AddressLine3ReadOnly { get; set; }

        [ElementLocator(Id = "candidate-address-line4")]
        public IWebElement AddressLine4ReadOnly { get; set; }

        [ElementLocator(Id = "candidate-address-postcode")]
        public IWebElement AddressPostcodeReadOnly { get; set; }

        [ElementLocator(Id = "EmailAddress")]
        public IWebElement EmailAddress { get; set; }

        [ElementLocator(Id = "DateOfBirth")]
        public IWebElement DataOfBirth { get; set; }

        [ElementLocator(Id = "PhoneNumber")]
        public IWebElement PhoneNumber { get; set; }

        [ElementLocator(Id = "Address")]
        public IWebElement Address { get; set; }

        #endregion

        #region Qualifications

        [ElementLocator(Id = "qualifications-yes-label")]
        public IWebElement QualificationsYes { get; set; }

        [ElementLocator(Id = "qualifications-no-label")]
        public IWebElement QualificationsNo { get; set; }

        [ElementLocator(Id = "qualification-validation-summary")]
        public IWebElement QualificationValidationSummary { get; set; }

        [ElementLocator(Id = "saveQualification")]
        public IWebElement SaveQualification { get; set; }

        [ElementLocator(Id = "qual-type")]
        public IElementList<IWebElement, TraineeshipQualificationTypeDropdownItem> QualificationTypeDropdown { get; set;
        }

        [ElementLocator(Id = "subject-year")]
        public IWebElement SubjectYear { get; set; }

        [ElementLocator(Id = "subject-name")]
        public IWebElement SubjectName { get; set; }

        [ElementLocator(Id = "subject-grade")]
        public IWebElement SubjectGrade { get; set; }

        [ElementLocator(Id = "qualifications-summary")]
        public IWebElement QualificationsSummary { get; set; }

        [ElementLocator(Id = "qualifications-summary")]
        public IElementList<IWebElement, QualificationSummaryItem> QualificationsSummaryItems { get; set; }

        [ElementLocator(Class = "error-message")]
        public IWebElement FieldValidationError { get; set; }

        [ElementLocator(Id = "qualification-save-warning")]
        public IWebElement QualificationSaveWarning { get; set; }

        public string QualificationsSummaryCount
        {
            get
            {
                return QualificationsSummaryItems.Count().ToString(CultureInfo.InvariantCulture);
            }
        }

        #endregion

        #region Work Experience

        [ElementLocator(Id = "workexp-yes-label")]
        public IWebElement WorkExperienceYes { get; set; }

        [ElementLocator(Id = "workexp-no-label")]
        public IWebElement WorkExperienceNo { get; set; }

        [ElementLocator(Id = "addWorkBtn")]
        public IWebElement SaveWorkExperience { get; set; }

        [ElementLocator(Id = "work-employer")]
        public IWebElement WorkEmployer { get; set; }

        [ElementLocator(Id = "work-title")]
        public IWebElement WorkTitle { get; set; }

        [ElementLocator(Id = "work-role")]
        public IWebElement WorkRole { get; set; }

        [ElementLocator(Id = "work-from-year")]
        public IWebElement WorkFromYear { get; set; }

        [ElementLocator(Id = "work-to-year")]
        public IWebElement WorkToYear { get; set; }

        [ElementLocator(Id = "work-experience-summary")]
        public IWebElement WorkExperienceSummary { get; set; }

        [ElementLocator(Id = "work-experience-summary")]
        public IElementList<IWebElement, WorkExperienceSummaryItem> WorkExperienceSummaryItems { get; set; }

        public string WorkExperiencesCount
        {
            get { return WorkExperienceSummaryItems.Count().ToString(CultureInfo.InvariantCulture); }
        }

        [ElementLocator(Id = "qualifications-panel")]
        public WorkExperiencePanel QualificationsPanel { get; set; }

        public string QualificationsValidationErrorsCount
        {
            get { return QualificationsPanel.ValidationErrorsCount; }
        }

        [ElementLocator(Id = "workexperience-panel")]
        public WorkExperiencePanel WorkExperiencePanel { get; set; }

        public string WorkExperienceValidationErrorsCount
        {
            get { return WorkExperiencePanel.ValidationErrorsCount; }
        }

        [ElementLocator(Id = "work-experience-save-warning")]
        public IWebElement WorkExperienceSaveWarning { get; set; }

        #endregion

        #region Training Courses

        [ElementLocator(Id = "training-history-yes-label")]
        public IWebElement TrainingCoursesYes { get; set; }

        [ElementLocator(Id = "training-history-no-label")]
        public IWebElement TrainingCoursesNo { get; set; }

        [ElementLocator(Id = "addTrainingCourseBtn")]
        public IWebElement SaveTrainingCourseButton { get; set; }

        [ElementLocator(Id = "training-history-provider")]
        public IWebElement TrainingCourseProvider { get; set; }

        [ElementLocator(Id = "training-history-course-title")]
        public IWebElement TrainingCourseTitle { get; set; }

        [ElementLocator(Id = "training-history-from-year")]
        public IWebElement TrainingCourseFromYear { get; set; }

        [ElementLocator(Id = "training-history-to-year")]
        public IWebElement TrainingCourseToYear { get; set; }

        [ElementLocator(Id = "training-history-summary")]
        public IWebElement TrainingCourseSummary { get; set; }

        [ElementLocator(Id = "training-history-summary")]
        public IElementList<IWebElement, TrainingCourseItem> TrainingCourseSummaryItems { get; set; }

        public string TrainingCourseCount
        {
            get
            {
                return TrainingCourseSummaryItems.Count().ToString(CultureInfo.InvariantCulture);
            }
        }

        [ElementLocator(Id = "training-history-panel")]
        public WorkExperiencePanel TrainingCoursePanel { get; set; }

        public string TrainingCourseValidationErrorsCount
        {
            get
            {
                return TrainingCoursePanel.ValidationErrorsCount;
            }
        }

        [ElementLocator(Id = "training-history-save-warning")]
        public IWebElement TrainingCourseSaveWarning { get; set; }

        #endregion
    }

    [ElementLocator(TagName = "option")]
    public class TraineeshipQualificationTypeDropdownItem : WebElement
    {
        public TraineeshipQualificationTypeDropdownItem(ISearchContext parent)
            : base(parent)
        {
        }
    }


    public class TraineeshipWorkExperiencePanel : WebElement
    {
        public TraineeshipWorkExperiencePanel(ISearchContext parent) : base(parent)
        {
        }

        public string ValidationErrorsCount
        {
            get
            {
                var count = FindElements(By.ClassName("error-message"))
                    .Count(fve => fve.GetCssValue("display") != "none")
                    .ToString(CultureInfo.InvariantCulture);
                return count;
            }
        }
    }

    public class TraineeshipQualificationsPanel : WebElement
    {
        public TraineeshipQualificationsPanel(ISearchContext parent)
            : base(parent)
        {
        }

        public string ValidationErrorsCount
        {
            get
            {
                var count = FindElements(By.ClassName("error-message"))
                    .Count(fve => fve.GetCssValue("display") != "none")
                    .ToString(CultureInfo.InvariantCulture);
                return count;
            }
        }
    }
}