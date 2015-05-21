namespace SFA.Apprenticeships.Web.Candidate.Mediators.Application
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Web.Security;
    using Common.Constants;
    using Common.Models.Application;
    using Common.Providers;
    using Constants.Pages;
    using Domain.Entities.Applications;
    using Domain.Entities.Vacancies;
    using Domain.Interfaces.Configuration;
    using Helpers;
    using Providers;
    using Validators;
    using ViewModels.Applications;
    using ViewModels.VacancySearch;

    public class ApprenticeshipApplicationMediator : ApplicationMediatorBase, IApprenticeshipApplicationMediator
    {
        private readonly IApprenticeshipApplicationProvider _apprenticeshipApplicationProvider;
        private readonly ApprenticeshipApplicationViewModelServerValidator _apprenticeshipApplicationViewModelFullValidator;
        private readonly ApprenticeshipApplicationViewModelSaveValidator _apprenticeshipApplicationViewModelSaveValidator;

        public ApprenticeshipApplicationMediator(IApprenticeshipApplicationProvider apprenticeshipApplicationProvider, ApprenticeshipApplicationViewModelServerValidator apprenticeshipApplicationViewModelFullValidator, ApprenticeshipApplicationViewModelSaveValidator apprenticeshipApplicationViewModelSaveValidator, IConfigurationService configService, IUserDataProvider userDataProvider)
            : base(configService, userDataProvider)
        {
            _apprenticeshipApplicationProvider = apprenticeshipApplicationProvider;
            _apprenticeshipApplicationViewModelFullValidator = apprenticeshipApplicationViewModelFullValidator;
            _apprenticeshipApplicationViewModelSaveValidator = apprenticeshipApplicationViewModelSaveValidator;
        }

        public MediatorResponse<ApprenticeshipApplicationViewModel> Resume(Guid candidateId, int vacancyId)
        {
            var model = _apprenticeshipApplicationProvider.GetApplicationViewModel(candidateId, vacancyId);

            if (model.HasError())
            {
                return GetMediatorResponse<ApprenticeshipApplicationViewModel>(ApprenticeshipApplicationMediatorCodes.Resume.HasError, null, model.ViewModelMessage, UserMessageLevel.Warning);
            }

            if (model.IsExpiredOrWithdrawn())
            {
                return GetMediatorResponse<ApprenticeshipApplicationViewModel>(ApprenticeshipApplicationMediatorCodes.Resume.HasError, null, MyApplicationsPageMessages.ApprenticeshipNoLongerAvailable, UserMessageLevel.Warning);
            }

            if (model.Status != ApplicationStatuses.Draft)
            {
                return GetMediatorResponse<ApprenticeshipApplicationViewModel>(ApprenticeshipApplicationMediatorCodes.Resume.IncorrectState, null, MyApplicationsPageMessages.ApplicationInIncorrectState, UserMessageLevel.Info);
            }

            return GetMediatorResponse<ApprenticeshipApplicationViewModel>(ApprenticeshipApplicationMediatorCodes.Resume.Ok, parameters: new { id = vacancyId });
        }

        public MediatorResponse<ApprenticeshipApplicationViewModel> Apply(Guid candidateId, string vacancyIdString)
        {
            int vacancyId;

            if (!TryParseVacancyId(vacancyIdString, out vacancyId))
            {
                return GetMediatorResponse<ApprenticeshipApplicationViewModel>(ApprenticeshipApplicationMediatorCodes.Apply.VacancyNotFound);
            }

            var model = _apprenticeshipApplicationProvider.GetApplicationViewModel(candidateId, vacancyId);
            if (model == null || model.IsNotFound())
            {
                model = _apprenticeshipApplicationProvider.CreateApplicationViewModel(candidateId, vacancyId);
            }

            if (model.HasError())
            {
                return GetMediatorResponse<ApprenticeshipApplicationViewModel>(ApprenticeshipApplicationMediatorCodes.Apply.HasError);
            }

            if (model.IsExpiredOrWithdrawn())
            {
                return GetMediatorResponse<ApprenticeshipApplicationViewModel>(ApprenticeshipApplicationMediatorCodes.Apply.VacancyNotFound, null, MyApplicationsPageMessages.ApprenticeshipNoLongerAvailable, UserMessageLevel.Warning);
            }

            if (model.Status != ApplicationStatuses.Draft)
            {
                return GetMediatorResponse<ApprenticeshipApplicationViewModel>(ApprenticeshipApplicationMediatorCodes.Apply.IncorrectState, null, MyApplicationsPageMessages.ApplicationInIncorrectState, UserMessageLevel.Info);
            }

            if (model.VacancyDetail.ApplyViaEmployerWebsite)
            {
                return GetMediatorResponse<ApprenticeshipApplicationViewModel>(ApprenticeshipApplicationMediatorCodes.Apply.OfflineVacancy);
            }

            model.SessionTimeout = FormsAuthentication.Timeout.TotalSeconds - 30;

            return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.Apply.Ok, model);
        }

        public MediatorResponse<ApprenticeshipApplicationViewModel> PreviewAndSubmit(Guid candidateId, int vacancyId, ApprenticeshipApplicationViewModel viewModel)
        {
            viewModel = StripApplicationViewModelBeforeValidation(viewModel);

            var savedModel = _apprenticeshipApplicationProvider.GetApplicationViewModel(candidateId, vacancyId);

            if (savedModel.HasError())
            {
                return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.PreviewAndSubmit.Error, viewModel, ApplicationPageMessages.PreviewFailed, UserMessageLevel.Warning);
            }

            if (savedModel.VacancyDetail.ApplyViaEmployerWebsite)
            {
                return GetMediatorResponse<ApprenticeshipApplicationViewModel>(ApprenticeshipApplicationMediatorCodes.PreviewAndSubmit.OfflineVacancy);
            }

            if (savedModel.IsExpiredOrWithdrawn())
            {
                return GetMediatorResponse<ApprenticeshipApplicationViewModel>(ApprenticeshipApplicationMediatorCodes.PreviewAndSubmit.VacancyNotFound);
            }

            if (savedModel.Status != ApplicationStatuses.Draft)
            {
                return GetMediatorResponse<ApprenticeshipApplicationViewModel>(ApprenticeshipApplicationMediatorCodes.PreviewAndSubmit.IncorrectState, null, MyApplicationsPageMessages.ApplicationInIncorrectState, UserMessageLevel.Info);
            }

            viewModel.SessionTimeout = FormsAuthentication.Timeout.TotalSeconds - 30;


            var result = _apprenticeshipApplicationViewModelFullValidator.Validate(viewModel);

            viewModel = _apprenticeshipApplicationProvider.PatchApplicationViewModel(candidateId, savedModel, viewModel);

            if (!result.IsValid)
            {
                return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.PreviewAndSubmit.ValidationError, viewModel, result);
            }

            _apprenticeshipApplicationProvider.SaveApplication(candidateId, vacancyId, viewModel);

            return GetMediatorResponse<ApprenticeshipApplicationViewModel>(ApprenticeshipApplicationMediatorCodes.PreviewAndSubmit.Ok, parameters: new { id = vacancyId });
        }

        public MediatorResponse<ApprenticeshipApplicationViewModel> Save(Guid candidateId, int vacancyId, ApprenticeshipApplicationViewModel viewModel)
        {
            viewModel = StripApplicationViewModelBeforeValidation(viewModel);

            var savedModel = _apprenticeshipApplicationProvider.GetApplicationViewModel(candidateId, vacancyId);

            if (savedModel.Status == ApplicationStatuses.ExpiredOrWithdrawn)
            {
                return GetMediatorResponse<ApprenticeshipApplicationViewModel>(ApprenticeshipApplicationMediatorCodes.Save.VacancyNotFound);
            }

            viewModel.SessionTimeout = FormsAuthentication.Timeout.TotalSeconds - 30;

            if (savedModel.HasError())
            {
                return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.Save.Error, viewModel, ApplicationPageMessages.SaveFailed, UserMessageLevel.Warning);
            }

            if (savedModel.VacancyDetail.ApplyViaEmployerWebsite)
            {
                return GetMediatorResponse<ApprenticeshipApplicationViewModel>(ApprenticeshipApplicationMediatorCodes.Save.OfflineVacancy);
            }

            var result = _apprenticeshipApplicationViewModelSaveValidator.Validate(viewModel);

            viewModel = _apprenticeshipApplicationProvider.PatchApplicationViewModel(candidateId, savedModel, viewModel);

            if (!result.IsValid)
            {
                return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.Save.ValidationError, viewModel, result);
            }

            _apprenticeshipApplicationProvider.SaveApplication(candidateId, vacancyId, viewModel);

            viewModel = _apprenticeshipApplicationProvider.GetApplicationViewModel(candidateId, vacancyId);
            viewModel.SessionTimeout = FormsAuthentication.Timeout.TotalSeconds - 30;

            return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.Save.Ok, viewModel);
        }

        public MediatorResponse<AutoSaveResultViewModel> AutoSave(Guid candidateId, int vacancyId, ApprenticeshipApplicationViewModel viewModel)
        {
            var autoSaveResult = new AutoSaveResultViewModel();

            var savedModel = _apprenticeshipApplicationProvider.GetApplicationViewModel(candidateId, vacancyId);

            if (savedModel.Status == ApplicationStatuses.ExpiredOrWithdrawn)
            {
                return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.AutoSave.VacancyNotFound, autoSaveResult);
            }

            viewModel.SessionTimeout = FormsAuthentication.Timeout.TotalSeconds - 30;

            if (savedModel.HasError())
            {
                return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.AutoSave.HasError, autoSaveResult);
            }

            var result = _apprenticeshipApplicationViewModelSaveValidator.Validate(viewModel);

            viewModel = _apprenticeshipApplicationProvider.PatchApplicationViewModel(candidateId, savedModel, viewModel);

            if (!result.IsValid)
            {
                return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.AutoSave.ValidationError, autoSaveResult, result);
            }

            _apprenticeshipApplicationProvider.SaveApplication(candidateId, vacancyId, viewModel);

            viewModel = _apprenticeshipApplicationProvider.GetApplicationViewModel(candidateId, vacancyId);
            viewModel.SessionTimeout = FormsAuthentication.Timeout.TotalSeconds - 30;

            autoSaveResult.Status = "succeeded";

            if (viewModel.DateUpdated != null)
            {
                autoSaveResult.DateTimeMessage = AutoSaveDateTimeHelper.GetDisplayDateTime((DateTime)viewModel.DateUpdated);
            }

            return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.AutoSave.Ok, autoSaveResult);
        }

        public MediatorResponse<ApprenticeshipApplicationViewModel> Submit(Guid candidateId, int vacancyId)
        {
            var savedModel = _apprenticeshipApplicationProvider.GetApplicationViewModel(candidateId, vacancyId);

            if (savedModel.HasError())
            {
                return GetMediatorResponse<ApprenticeshipApplicationViewModel>(ApprenticeshipApplicationMediatorCodes.Submit.Error, null, ApplicationPageMessages.SubmitApplicationFailed, UserMessageLevel.Warning, new { id = vacancyId });
            }

            if (savedModel.IsExpiredOrWithdrawn())
            {
                return GetMediatorResponse<ApprenticeshipApplicationViewModel>(ApprenticeshipApplicationMediatorCodes.Submit.VacancyNotFound);
            }

            var result = _apprenticeshipApplicationViewModelSaveValidator.Validate(savedModel);

            if (!result.IsValid)
            {
                return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.Submit.ValidationError, savedModel, result);
            }

            var model = _apprenticeshipApplicationProvider.SubmitApplication(candidateId, vacancyId);

            if (model.ViewModelStatus == ApplicationViewModelStatus.ApplicationInIncorrectState)
            {
                return GetMediatorResponse<ApprenticeshipApplicationViewModel>(ApprenticeshipApplicationMediatorCodes.Submit.IncorrectState);
            }
            if (model.ViewModelStatus == ApplicationViewModelStatus.Error || model.HasError())
            {
                return GetMediatorResponse<ApprenticeshipApplicationViewModel>(ApprenticeshipApplicationMediatorCodes.Submit.Error, null, ApplicationPageMessages.SubmitApplicationFailed, UserMessageLevel.Warning, new { id = vacancyId });
            }

            var parameters = new
            {
                id = vacancyId,
                vacancyReference = model.VacancyDetail.VacancyReference,
                vacancyTitle = model.VacancyDetail.Title
            };
            return GetMediatorResponse<ApprenticeshipApplicationViewModel>(ApprenticeshipApplicationMediatorCodes.Submit.Ok, parameters: parameters);
        }

        public MediatorResponse<ApprenticeshipApplicationViewModel> AddEmptyQualificationRows(ApprenticeshipApplicationViewModel viewModel)
        {
            viewModel.Candidate.Qualifications = RemoveEmptyRowsFromQualifications(viewModel.Candidate.Qualifications);
            viewModel.Candidate.HasQualifications = viewModel.Candidate.Qualifications.Any();

            viewModel.DefaultQualificationRows = 5;
            viewModel.DefaultWorkExperienceRows = 0;
            viewModel.DefaultTrainingHistoryRows = 0;

            return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.AddEmptyQualificationRows.Ok, viewModel);
        }

        public MediatorResponse<ApprenticeshipApplicationViewModel> AddEmptyWorkExperienceRows(ApprenticeshipApplicationViewModel viewModel)
        {
            viewModel.Candidate.WorkExperience = RemoveEmptyRowsFromWorkExperience(viewModel.Candidate.WorkExperience);
            viewModel.Candidate.HasWorkExperience = viewModel.Candidate.WorkExperience.Any();

            viewModel.DefaultQualificationRows = 0;
            viewModel.DefaultWorkExperienceRows = 3;
            viewModel.DefaultTrainingHistoryRows = 0;

            return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.AddEmptyWorkExperienceRows.Ok, viewModel);
        }

        public MediatorResponse<ApprenticeshipApplicationViewModel> AddEmptyTrainingHistoryRows(ApprenticeshipApplicationViewModel viewModel)
        {
            viewModel.Candidate.TrainingHistory = RemoveEmptyRowsFromTrainingHistory(viewModel.Candidate.TrainingHistory);
            viewModel.Candidate.HasTrainingHistory = viewModel.Candidate.TrainingHistory.Any();

            viewModel.DefaultQualificationRows = 0;
            viewModel.DefaultWorkExperienceRows = 0;
            viewModel.DefaultTrainingHistoryRows = 3;

            return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.AddEmptyTrainingHistoryRows.Ok, viewModel);
        }

        public MediatorResponse<ApprenticeshipApplicationViewModel> Preview(Guid candidateId, int vacancyId)
        {
            var model = _apprenticeshipApplicationProvider.GetApplicationViewModel(candidateId, vacancyId);

            if (model.HasError())
            {
                return GetMediatorResponse<ApprenticeshipApplicationViewModel>(ApprenticeshipApplicationMediatorCodes.Preview.HasError);
            }

            if (model.VacancyDetail.ApplyViaEmployerWebsite)
            {
                return GetMediatorResponse<ApprenticeshipApplicationViewModel>(ApprenticeshipApplicationMediatorCodes.Preview.OfflineVacancy);
            }

            if (model.IsExpiredOrWithdrawn())
            {
                return GetMediatorResponse<ApprenticeshipApplicationViewModel>(ApprenticeshipApplicationMediatorCodes.Preview.VacancyNotFound);
            }

            if (model.Status != ApplicationStatuses.Draft)
            {
                return GetMediatorResponse<ApprenticeshipApplicationViewModel>(ApprenticeshipApplicationMediatorCodes.Preview.IncorrectState, null, MyApplicationsPageMessages.ApplicationInIncorrectState, UserMessageLevel.Info);
            }

            return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.Preview.Ok, model);
        }

        public MediatorResponse<WhatHappensNextApprenticeshipViewModel> WhatHappensNext(Guid candidateId, string vacancyIdString, string vacancyReference, string vacancyTitle, string searchReturnUrl)
        {
            int vacancyId;

            if (!TryParseVacancyId(vacancyIdString, out vacancyId))
            {
                return GetMediatorResponse<WhatHappensNextApprenticeshipViewModel>(ApprenticeshipApplicationMediatorCodes.WhatHappensNext.VacancyNotFound);
            }

            var model = _apprenticeshipApplicationProvider.GetWhatHappensNextViewModel(candidateId, vacancyId, searchReturnUrl);

            if (model.HasError())
            {
                model.VacancyReference = vacancyReference;
                model.VacancyTitle = vacancyTitle;
            }
            else if (model.Status == ApplicationStatuses.ExpiredOrWithdrawn || model.VacancyStatus != VacancyStatuses.Live)
            {
                return GetMediatorResponse<WhatHappensNextApprenticeshipViewModel>(ApprenticeshipApplicationMediatorCodes.WhatHappensNext.VacancyNotFound);
            }
            
            return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.WhatHappensNext.Ok, model);
        }

        public MediatorResponse<ApprenticeshipApplicationViewModel> View(Guid candidateId, int vacancyId)
        {
            var model = _apprenticeshipApplicationProvider.GetApplicationViewModel(candidateId, vacancyId);

            if (model.ViewModelStatus == ApplicationViewModelStatus.ApplicationNotFound)
            {
                return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.View.ApplicationNotFound, model);
            }

            if (model.HasError())
            {
                return GetMediatorResponse<ApprenticeshipApplicationViewModel>(ApprenticeshipApplicationMediatorCodes.View.Error, null, ApplicationPageMessages.ViewApplicationFailed, UserMessageLevel.Warning);
            }

            return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.View.Ok, model);
        }

        public MediatorResponse<SavedVacancyViewModel> SaveVacancy(Guid candidateId, int vacancyId)
        {
            var viewModel = _apprenticeshipApplicationProvider.SaveVacancy(candidateId, vacancyId);
            int savedVacancyCount;

            int.TryParse(UserDataProvider.Get(UserDataItemNames.SavedAndDraftCount), out savedVacancyCount);
            UserDataProvider.Push(UserDataItemNames.SavedAndDraftCount, (savedVacancyCount + 1).ToString(CultureInfo.InvariantCulture));

            return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.SaveVacancy.Ok, viewModel);
        }

        public MediatorResponse<SavedVacancyViewModel> DeleteSavedVacancy(Guid candidateId, int vacancyId)
        {
            var viewModel = _apprenticeshipApplicationProvider.DeleteSavedVacancy(candidateId, vacancyId);
            int savedVacancyCount;

            int.TryParse(UserDataProvider.Get(UserDataItemNames.SavedAndDraftCount), out savedVacancyCount);
            UserDataProvider.Push(UserDataItemNames.SavedAndDraftCount, Math.Max(0, savedVacancyCount - 1).ToString(CultureInfo.InvariantCulture));

            return GetMediatorResponse(ApprenticeshipApplicationMediatorCodes.DeleteSavedVacancy.Ok, viewModel);
        }

        #region Helpers

        private static ApprenticeshipApplicationViewModel StripApplicationViewModelBeforeValidation(ApprenticeshipApplicationViewModel model)
        {
            model.Candidate.Qualifications = RemoveEmptyRowsFromQualifications(model.Candidate.Qualifications);
            model.Candidate.WorkExperience = RemoveEmptyRowsFromWorkExperience(model.Candidate.WorkExperience);
            model.Candidate.TrainingHistory = RemoveEmptyRowsFromTrainingHistory(model.Candidate.TrainingHistory);

            model.DefaultQualificationRows = 0;
            model.DefaultWorkExperienceRows = 0;
            model.DefaultTrainingHistoryRows = 0;

            if (model.IsJavascript)
            {
                return model;
            }

            model.Candidate.HasQualifications = model.Candidate.Qualifications.Any();
            model.Candidate.HasWorkExperience = model.Candidate.WorkExperience.Any();
            model.Candidate.HasTrainingHistory = model.Candidate.TrainingHistory.Any();

            return model;
        }

        #endregion
    }
}