namespace SFA.Apprenticeships.Web.Candidate.Mappers.Resolvers
{
    using System.Collections.Generic;
    using AutoMapper;
    using Common.ViewModels.Applications;
    using Common.ViewModels.Candidate;
    using Common.ViewModels.VacancySearch;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Entities.Locations;
    using Domain.Entities.Users;
    using Domain.Entities.Vacancies.Traineeships;
    using Helpers;

    public class TraineeeshipApplicationViewModelToTraineeeshipApplicationDetailResolver :
        ITypeConverter<TraineeshipApplicationViewModel, TraineeshipApplicationDetail>
    {
        public TraineeshipApplicationDetail Convert(ResolutionContext context)
        {
            var model = (TraineeshipApplicationViewModel) context.SourceValue;

            var application = new TraineeshipApplicationDetail
            {
                CandidateId = model.Candidate.Id,
                VacancyStatus = model.VacancyDetail.VacancyStatus,
                Vacancy = GetVacancy(model.VacancyDetail),
                CandidateDetails = GetCandidateDetails(model.Candidate),
                CandidateInformation = GetCandidateInformation(model.Candidate),
                AdditionalQuestion1Answer =
                    model.Candidate.EmployerQuestionAnswers != null
                        ? model.Candidate.EmployerQuestionAnswers.CandidateAnswer1
                        : string.Empty,
                AdditionalQuestion2Answer =
                    model.Candidate.EmployerQuestionAnswers != null
                        ? model.Candidate.EmployerQuestionAnswers.CandidateAnswer2
                        : string.Empty
            };

            return application;
        }

        private static ApplicationTemplate GetCandidateInformation(TraineeshipCandidateViewModel modelBase)
        {
            return new ApplicationTemplate
            {
                AboutYou = ApplicationConverter.GetAboutYou(modelBase.AboutYou, modelBase.MonitoringInformation),
                Qualifications =
                    modelBase.HasQualifications
                        ? ApplicationConverter.GetQualifications(modelBase.Qualifications)
                        : new List<Qualification>(),
                WorkExperience =
                    modelBase.HasWorkExperience
                        ? ApplicationConverter.GetWorkExperiences(modelBase.WorkExperience)
                        : new List<WorkExperience>(),
                TrainingCourses = 
                    modelBase.HasTrainingCourses
                        ? ApplicationConverter.GetTrainingCourses(modelBase.TrainingCourses)
                        : new List<TrainingCourse>(),
                DisabilityStatus = ApplicationConverter.GetDisabilityStatus(modelBase.MonitoringInformation.DisabilityStatus)
            };
        }

        private static RegistrationDetails GetCandidateDetails(TraineeshipCandidateViewModel modelBase)
        {
            return new RegistrationDetails
            {
                EmailAddress = modelBase.EmailAddress,
                Address = ApplicationConverter.GetAddress(modelBase.Address),
                DateOfBirth = modelBase.DateOfBirth,
                PhoneNumber = modelBase.PhoneNumber,
                FirstName = modelBase.FirstName,
                LastName = modelBase.LastName,
            };
        }

        private static TraineeshipSummary GetVacancy(TraineeshipVacancyDetailViewModel model)
        {
            return new TraineeshipSummary
            {
                Id = model.Id,
                ClosingDate = model.ClosingDate,
                Description = model.Description,
                EmployerName = model.EmployerName,
                Location = new GeoPoint
                {
                    Longitude = model.VacancyAddress.GeoPoint.Longitude,
                    Latitude = model.VacancyAddress.GeoPoint.Latitude
                },
                Title = model.Title,
            };
        }
    }
}