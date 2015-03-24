﻿namespace SFA.Apprenticeships.Web.Candidate.Mappers.Resolvers
{
    using System.Collections.Generic;
    using AutoMapper;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Entities.Locations;
    using Domain.Entities.Users;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Helpers;
    using ViewModels.Applications;
    using ViewModels.Candidate;
    using ViewModels.VacancySearch;

    public class ApprenticeshipApplicationViewModelToApprenticeshipApplicationDetailResolver :
        ITypeConverter<ApprenticeshipApplicationViewModel, ApprenticeshipApplicationDetail>
    {
        public ApprenticeshipApplicationDetail Convert(ResolutionContext context)
        {
            var model = (ApprenticeshipApplicationViewModel) context.SourceValue;

            var application = new ApprenticeshipApplicationDetail
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

        private static ApplicationTemplate GetCandidateInformation(ApprenticeshipCandidateViewModel modelBase)
        {
            return new ApplicationTemplate
            {
                AboutYou = ApplicationConverter.GetAboutYou(modelBase.AboutYou),
                EducationHistory = ApplicationConverter.GetEducation(modelBase.Education),
                Qualifications =
                    modelBase.HasQualifications
                        ? ApplicationConverter.GetQualifications(modelBase.Qualifications)
                        : new List<Qualification>(),
                WorkExperience =
                    modelBase.HasWorkExperience
                        ? ApplicationConverter.GetWorkExperiences(modelBase.WorkExperience)
                        : new List<WorkExperience>(),
            };
        }

        private static RegistrationDetails GetCandidateDetails(CandidateViewModelBase modelBase)
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

        private static ApprenticeshipSummary GetVacancy(ApprenticeshipVacancyDetailViewModel model)
        {
            return new ApprenticeshipSummary
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