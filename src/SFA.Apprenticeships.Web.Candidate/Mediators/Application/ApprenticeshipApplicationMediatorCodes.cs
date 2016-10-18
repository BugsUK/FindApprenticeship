namespace SFA.Apprenticeships.Web.Candidate.Mediators.Application
{
    public class ApprenticeshipApplicationMediatorCodes
    {
        public class Apply
        {
            public const string Ok = "ApprenticeshipApplication.Apply.Ok";
            public const string HasError = "ApprenticeshipApplication.Apply.HasError";
            public const string VacancyNotFound = "ApprenticeshipApplication.Apply.VacancyNotFound";
            public const string OfflineVacancy = "ApprenticeshipApplication.Apply.OfflineVacancy";
            public const string IncorrectState = "ApprenticeshipApplication.Apply.IncorrectState";
        }

        public class Submit
        {
            public const string VacancyNotFound = "ApprenticeshipApplication.Submit.VacancyNotFound";
            public const string IncorrectState = "ApprenticeshipApplication.Submit.IncorrectState";
            public const string Error = "ApprenticeshipApplication.Submit.Error";
            public const string Ok = "ApprenticeshipApplication.Submit.Ok";
            public const string ValidationError = "ApprenticeshipApplication.Submit.ValidationError";
            public const string AcceptSubmitValidationError = "ApprenticeshipApplication.Submit.AcceptSubmitValidationError";
        }

        public class AddEmptyQualificationRows
        {
            public const string Ok = "ApprenticeshipApplication.AddEmptyQualificationRows.Ok";
        }

        public class AddEmptyWorkExperienceRows
        {
            public const string Ok = "ApprenticeshipApplication.AddEmptyWorkExperienceRows.Ok";
        }

        public class AddEmptyTrainingCourseRows
        {
            public const string Ok = "ApprenticeshipApplication.AddEmptyTrainingCourseRows.Ok";
        }

        public class WhatHappensNext
        {
            public const string VacancyNotFound = "ApprenticeshipApplication.WhatHappensNext.VacancyNotFound";
            public const string Ok = "ApprenticeshipApplication.WhatHappensNext.Ok";
            public const string OfflineVacancy = "ApprenticeshipApplication.WhatHappensNext.OfflineVacancy";
        }

        public class Resume
        {
            public const string Ok = "ApprenticeshipApplication.Resume.Ok";
            public const string HasError = "ApprenticeshipApplication.Resume.HasError";
            public const string IncorrectState = "ApprenticeshipApplication.Resume.IncorrectState";
        }

        public class PreviewAndSubmit
        {
            public const string OfflineVacancy = "ApprenticeshipApplication.PreviewAndSubmit.OfflineVacancy";
            public const string VacancyNotFound = "ApprenticeshipApplication.PreviewAndSubmit.VacancyNotFound";
            public const string IncorrectState = "ApprenticeshipApplication.PreviewAndSubmit.IncorrectState";
            public const string Error = "ApprenticeshipApplication.PreviewAndSubmit.Error";
            public const string ValidationError = "ApprenticeshipApplication.PreviewAndSubmit.ValidationError";
            public const string Ok = "ApprenticeshipApplication.PreviewAndSubmit.Ok";
        }

        public class Preview
        {
            public const string Ok = "ApprenticeshipApplication.Preview.Ok";
            public const string HasError = "ApprenticeshipApplication.Preview.HasError";
            public const string IncorrectState = "ApprenticeshipApplication.Preview.IncorrectState";
            public const string VacancyNotFound = "ApprenticeshipApplication.Preview.VacancyNotFound";
            public const string OfflineVacancy = "ApprenticeshipApplication.Preview.OfflineVacancy";
        }

        public class Save
        {
            public const string OfflineVacancy = "ApprenticeshipApplication.Save.OfflineVacancy";
            public const string VacancyNotFound = "ApprenticeshipApplication.Save.VacancyNotFound";
            public const string Error = "ApprenticeshipApplication.Save.Error";
            public const string ValidationError = "ApprenticeshipApplication.Save.ValidationError";
            public const string Ok = "ApprenticeshipApplication.Save.Ok";
            public const string IncorrectState = "ApprenticeshipApplication.Save.IncorrectState";
        }

        public class AutoSave
        {
            public const string VacancyNotFound = "ApprenticeshipApplication.AutoSave.VacancyNotFound";
            public const string ValidationError = "ApprenticeshipApplication.AutoSave.ValidationError";
            public const string HasError = "ApprenticeshipApplication.AutoSave.HasError";
            public const string Ok = "ApprenticeshipApplication.AutoSave.Ok";
            public const string IncorrectState = "ApprenticeshipApplication.AutoSave.IncorrectState";
        }

        public class SaveVacancy
        {
            public const string Ok = "ApprenticeshipApplication.SaveVacancy.Ok";
        }

        public class DeleteSavedVacancy
        {
            public const string Ok = "ApprenticeshipApplication.DeleteSavedVacancy.Ok";
        }

        public class View
        {
            public const string Ok = "ApprenticeshipApplication.View.Ok";
            public const string ApplicationNotFound = "ApprenticeshipApplication.View.ApplicationNotFound";
            public const string Error = "ApprenticeshipApplication.View.Error";
        }

        public class CandidateApplicationFeedback
        {
            public const string Ok = "ApprenticeshipApplication.View.Ok";
            public const string ApplicationNotFound = "ApprenticeshipApplication.View.ApplicationNotFound";
            public const string Error = "ApprenticeshipApplication.View.Error";
        }
    }
}
