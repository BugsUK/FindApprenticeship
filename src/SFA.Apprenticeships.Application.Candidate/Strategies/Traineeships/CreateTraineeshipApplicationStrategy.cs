﻿namespace SFA.Apprenticeships.Application.Candidate.Strategies.Traineeships
{
    using System;
    using AutoMapper;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using Domain.Entities.Vacancies.Traineeships;
    using Domain.Interfaces.Repositories;
    using Vacancy;

    public class CreateTraineeshipApplicationStrategy : ICreateTraineeshipApplicationStrategy
    {
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly IVacancyDataProvider<TraineeshipVacancyDetail> _vacancyDataProvider;

        public CreateTraineeshipApplicationStrategy(
            IVacancyDataProvider<TraineeshipVacancyDetail> vacancyDataProvider,
            ICandidateReadRepository candidateReadRepository)
        {
            _vacancyDataProvider = vacancyDataProvider;
            _candidateReadRepository = candidateReadRepository;
        }

        public TraineeshipApplicationDetail CreateApplication(Guid candidateId, int vacancyId)
        {
            var vacancyDetails = _vacancyDataProvider.GetVacancyDetails(vacancyId);

            if (vacancyDetails == null) return null;

            var candidate = _candidateReadRepository.Get(candidateId);
            var applicationDetail = CreateApplicationDetail(candidate, vacancyDetails);

            return applicationDetail;
        }

        private static TraineeshipApplicationDetail CreateApplicationDetail(Candidate candidate, TraineeshipVacancyDetail vacancyDetails)
        {
            return new TraineeshipApplicationDetail
            {
                EntityId = Guid.NewGuid(),
                DateCreated = DateTime.Now,
                CandidateId = candidate.EntityId,
                CandidateDetails = Mapper.Map<RegistrationDetails, RegistrationDetails>(candidate.RegistrationDetails),
                VacancyStatus = vacancyDetails.VacancyStatus,
                Vacancy = new TraineeshipSummary
                {
                    Id = vacancyDetails.Id,
                    VacancyReference = vacancyDetails.VacancyReference,
                    Title = vacancyDetails.Title,
                    EmployerName = vacancyDetails.IsEmployerAnonymous ?
                        vacancyDetails.AnonymousEmployerName : vacancyDetails.EmployerName,
                    StartDate = vacancyDetails.StartDate,
                    ClosingDate = vacancyDetails.ClosingDate,
                    Description = vacancyDetails.Description,
                    NumberOfPositions = vacancyDetails.NumberOfPositions,
                    Location = null // NOTE: no equivalent in legacy vacancy details.
                },
                // Populate traineeshipApplication template with candidate's most recent information.
                CandidateInformation = new ApplicationTemplate
                {
                    Qualifications = candidate.ApplicationTemplate.Qualifications,
                    WorkExperience = candidate.ApplicationTemplate.WorkExperience,
                    TrainingCourses = candidate.ApplicationTemplate.TrainingCourses,
                    AboutYou = candidate.ApplicationTemplate.AboutYou,
                    DisabilityStatus = candidate.MonitoringInformation.DisabilityStatus
                },
            };
        }
    }
}
