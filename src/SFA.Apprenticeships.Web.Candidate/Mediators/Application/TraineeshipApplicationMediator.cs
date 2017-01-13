using SFA.Apprenticeships.Web.Common.Mediators;

namespace SFA.Apprenticeships.Web.Candidate.Mediators.Application
{
    using Apprenticeships.Application.Interfaces;
    using Common.Constants;
    using Common.Models.Application;
    using Common.Providers;
    using Constants.Pages;
    using Domain.Entities.Applications;
    using Providers;
    using System;
    using System.Linq;
    using System.Web.Security;
    using Validators;
    using ViewModels.Applications;

    public class TraineeshipApplicationMediator : ApplicationMediatorBase, ITraineeshipApplicationMediator
    {
        private readonly ITraineeshipApplicationProvider _traineeshipApplicationProvider;
        private readonly TraineeshipApplicationViewModelServerValidator _traineeshipApplicationViewModelServer;

        public TraineeshipApplicationMediator(
            ITraineeshipApplicationProvider traineeshipApplicationProvider,
            IConfigurationService configService,
            IUserDataProvider userDataProvider,
            TraineeshipApplicationViewModelServerValidator traineeshipApplicationViewModelServer)
            : base(configService, userDataProvider)
        {
            _traineeshipApplicationProvider = traineeshipApplicationProvider;
            _traineeshipApplicationViewModelServer = traineeshipApplicationViewModelServer;
        }

        public MediatorResponse<TraineeshipApplicationViewModel> Apply(Guid candidateId, string vacancyIdString)
        {
            int vacancyId;

            if (!TryParseVacancyId(vacancyIdString, out vacancyId))
            {
                return GetMediatorResponse<TraineeshipApplicationViewModel>(TraineeshipApplicationMediatorCodes.Apply.VacancyNotFound);
            }

            var model = _traineeshipApplicationProvider.GetApplicationViewModel(candidateId, vacancyId);

            if (model.HasError())
            {
                return GetMediatorResponse<TraineeshipApplicationViewModel>(TraineeshipApplicationMediatorCodes.Apply.HasError);
            }

            model.SessionTimeout = FormsAuthentication.Timeout.TotalSeconds - 30;

            return GetMediatorResponse(TraineeshipApplicationMediatorCodes.Apply.Ok, model);
        }

        public MediatorResponse<TraineeshipApplicationViewModel> Submit(Guid candidateId, int vacancyId, TraineeshipApplicationViewModel viewModel)
        {
            viewModel = StripApplicationViewModelBeforeValidation(viewModel);

            var savedModel = _traineeshipApplicationProvider.GetApplicationViewModel(candidateId, vacancyId);

            if (savedModel.HasError())
            {
                return GetMediatorResponse(TraineeshipApplicationMediatorCodes.Submit.Error, viewModel, ApplicationPageMessages.SubmitApplicationFailed, UserMessageLevel.Warning, new { id = vacancyId });
            }

            var result = _traineeshipApplicationViewModelServer.Validate(viewModel);

            viewModel = _traineeshipApplicationProvider.PatchApplicationViewModel(candidateId, savedModel, viewModel);

            if (!result.IsValid)
            {
                return GetMediatorResponse(TraineeshipApplicationMediatorCodes.Submit.ValidationError, viewModel, result);
            }

            var submittedApplicationModel = _traineeshipApplicationProvider.SubmitApplication(candidateId, vacancyId, viewModel);

            if (submittedApplicationModel.ViewModelStatus == ApplicationViewModelStatus.ApplicationInIncorrectState)
            {
                return GetMediatorResponse<TraineeshipApplicationViewModel>(TraineeshipApplicationMediatorCodes.Submit.IncorrectState);
            }
            if (submittedApplicationModel.ViewModelStatus == ApplicationViewModelStatus.Error)
            {
                return GetMediatorResponse(TraineeshipApplicationMediatorCodes.Submit.Error, viewModel, ApplicationPageMessages.SubmitApplicationFailed, UserMessageLevel.Warning, new { id = vacancyId });
            }

            var parameters = new
            {
                id = vacancyId,
                vacancyReference = submittedApplicationModel.VacancyDetail.VacancyReference,
                vacancyTitle = submittedApplicationModel.VacancyDetail.Title
            };
            return GetMediatorResponse<TraineeshipApplicationViewModel>(TraineeshipApplicationMediatorCodes.Submit.Ok, parameters: parameters);
        }

        public MediatorResponse<TraineeshipApplicationViewModel> AddEmptyQualificationRows(TraineeshipApplicationViewModel viewModel)
        {
            viewModel.Candidate.Qualifications = RemoveEmptyRowsFromQualifications(viewModel.Candidate.Qualifications);
            viewModel.Candidate.HasQualifications = viewModel.Candidate.Qualifications.Any();

            viewModel.DefaultQualificationRows = 5;
            viewModel.DefaultWorkExperienceRows = 0;
            viewModel.DefaultTrainingCourseRows = 0;

            return GetMediatorResponse(TraineeshipApplicationMediatorCodes.AddEmptyQualificationRows.Ok, viewModel);
        }

        public MediatorResponse<TraineeshipApplicationViewModel> AddEmptyWorkExperienceRows(TraineeshipApplicationViewModel viewModel)
        {
            viewModel.Candidate.WorkExperience = RemoveEmptyRowsFromWorkExperience(viewModel.Candidate.WorkExperience);
            viewModel.Candidate.HasWorkExperience = viewModel.Candidate.WorkExperience.Any();

            viewModel.DefaultQualificationRows = 0;
            viewModel.DefaultWorkExperienceRows = 3;
            viewModel.DefaultTrainingCourseRows = 0;

            return GetMediatorResponse(TraineeshipApplicationMediatorCodes.AddEmptyWorkExperienceRows.Ok, viewModel);
        }

        public MediatorResponse<TraineeshipApplicationViewModel> AddEmptyTrainingCourseRows(TraineeshipApplicationViewModel viewModel)
        {
            viewModel.Candidate.TrainingCourses = RemoveEmptyRowsFromTrainingCourses(viewModel.Candidate.TrainingCourses);
            viewModel.Candidate.HasTrainingCourses = viewModel.Candidate.TrainingCourses.Any();

            viewModel.DefaultQualificationRows = 0;
            viewModel.DefaultWorkExperienceRows = 0;
            viewModel.DefaultTrainingCourseRows = 3;

            return GetMediatorResponse(TraineeshipApplicationMediatorCodes.AddEmptyTrainingCourseRows.Ok, viewModel);
        }

        public MediatorResponse<WhatHappensNextTraineeshipViewModel> WhatHappensNext(Guid candidateId, string vacancyIdString, string vacancyReference, string vacancyTitle)
        {
            int vacancyId;

            if (!TryParseVacancyId(vacancyIdString, out vacancyId))
            {
                return GetMediatorResponse<WhatHappensNextTraineeshipViewModel>(TraineeshipApplicationMediatorCodes.WhatHappensNext.VacancyNotFound);
            }

            var model = _traineeshipApplicationProvider.GetWhatHappensNextViewModel(candidateId, vacancyId);

            if (model.Status == ApplicationStatuses.ExpiredOrWithdrawn)
            {
                return GetMediatorResponse<WhatHappensNextTraineeshipViewModel>(TraineeshipApplicationMediatorCodes.WhatHappensNext.VacancyNotFound);
            }

            if (model.HasError())
            {
                model.VacancyReference = vacancyReference;
                model.VacancyTitle = vacancyTitle;
            }

            return GetMediatorResponse(TraineeshipApplicationMediatorCodes.WhatHappensNext.Ok, model);
        }

        public MediatorResponse<TraineeshipApplicationViewModel> View(Guid candidateId, int vacancyId)
        {
            var model = _traineeshipApplicationProvider.GetApplicationViewModelEx(candidateId, vacancyId);

            if (model.ViewModelStatus == ApplicationViewModelStatus.ApplicationNotFound)
            {
                return GetMediatorResponse(TraineeshipApplicationMediatorCodes.View.ApplicationNotFound, model);
            }

            if (model.HasError())
            {
                return GetMediatorResponse<TraineeshipApplicationViewModel>(
                    TraineeshipApplicationMediatorCodes.View.Error, null, ApplicationPageMessages.ViewApplicationFailed, UserMessageLevel.Warning);
            }

            return GetMediatorResponse(TraineeshipApplicationMediatorCodes.View.Ok, model);
        }

        private static TraineeshipApplicationViewModel StripApplicationViewModelBeforeValidation(TraineeshipApplicationViewModel model)
        {
            model.Candidate.Qualifications = RemoveEmptyRowsFromQualifications(model.Candidate.Qualifications);
            model.Candidate.WorkExperience = RemoveEmptyRowsFromWorkExperience(model.Candidate.WorkExperience);
            model.Candidate.TrainingCourses = RemoveEmptyRowsFromTrainingCourses(model.Candidate.TrainingCourses);

            model.DefaultQualificationRows = 0;
            model.DefaultWorkExperienceRows = 0;
            model.DefaultTrainingCourseRows = 0;

            if (model.IsJavascript)
            {
                return model;
            }

            model.Candidate.HasQualifications = model.Candidate.Qualifications.Any();
            model.Candidate.HasWorkExperience = model.Candidate.WorkExperience.Any();
            model.Candidate.HasTrainingCourses = model.Candidate.TrainingCourses.Any();

            return model;
        }
    }
}