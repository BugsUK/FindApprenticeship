namespace SFA.Apprenticeships.Application.Candidate.Strategies.Apprenticeships
{
    using System;
    using AutoMapper;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Interfaces.Repositories;
    using Vacancy;

    public class CreateApprenticeshipApplicationStrategy : ICreateApprenticeshipApplicationStrategy, ISaveApprenticeshipVacancyStrategy
    {
        private readonly IApprenticeshipApplicationReadRepository _apprenticeshipApplicationReadRepository;
        private readonly IApprenticeshipApplicationWriteRepository _apprenticeshipApplicationWriteRepository;
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly IVacancyDataProvider<ApprenticeshipVacancyDetail> _vacancyDataProvider;

        public CreateApprenticeshipApplicationStrategy(
            IVacancyDataProvider<ApprenticeshipVacancyDetail> vacancyDataProvider,
            IApprenticeshipApplicationReadRepository apprenticeshipApplicationReadRepository,
            IApprenticeshipApplicationWriteRepository apprenticeshipApplicationWriteRepository,
            ICandidateReadRepository candidateReadRepository)
        {
            _vacancyDataProvider = vacancyDataProvider;
            _apprenticeshipApplicationWriteRepository = apprenticeshipApplicationWriteRepository;
            _apprenticeshipApplicationReadRepository = apprenticeshipApplicationReadRepository;
            _candidateReadRepository = candidateReadRepository;
        }

        public ApprenticeshipApplicationDetail CreateApplication(Guid candidateId, int vacancyId)
        {
            var applicationDetail = _apprenticeshipApplicationReadRepository.GetForCandidate(candidateId, vacancyId);

            if (applicationDetail == null)
            {
                // Candidate has not previously applied for this vacancy.
                return CreateNewApplication(candidateId, vacancyId);
            }

            applicationDetail.AssertState("Create apprenticeshipApplication", ApplicationStatuses.Draft);

            if (applicationDetail.IsArchived)
            {
                return UnarchiveApplication(applicationDetail);
            }

            return applicationDetail;
        }

        public ApprenticeshipApplicationDetail SaveVacancy(Guid candidateId, int vacancyId)
        {
            var applicationDetail = _apprenticeshipApplicationReadRepository.GetForCandidate(candidateId, vacancyId);

            if (applicationDetail != null)
            {
                return applicationDetail;
            }

            return CreateNewApplication(candidateId, vacancyId, true);
        }

        #region

        private ApprenticeshipApplicationDetail UnarchiveApplication(ApprenticeshipApplicationDetail apprenticeshipApplicationDetail)
        {
            apprenticeshipApplicationDetail.IsArchived = false;
            _apprenticeshipApplicationWriteRepository.Save(apprenticeshipApplicationDetail);

            return _apprenticeshipApplicationReadRepository.Get(apprenticeshipApplicationDetail.EntityId);
        }

        private ApprenticeshipApplicationDetail CreateNewApplication(Guid candidateId, int vacancyId, bool saveVacancyOnly = false)
        {
            var vacancyDetails = _vacancyDataProvider.GetVacancyDetails(vacancyId);

            if (vacancyDetails == null) return null;
            
            var candidate = _candidateReadRepository.Get(candidateId);

            var applicationTemplate = saveVacancyOnly
                ? null
                : new ApplicationTemplate
                {
                    EducationHistory = candidate.ApplicationTemplate.EducationHistory,
                    Qualifications = candidate.ApplicationTemplate.Qualifications,
                    WorkExperience = candidate.ApplicationTemplate.WorkExperience,
                    AboutYou = candidate.ApplicationTemplate.AboutYou
                };

            var applicationDetail = CreateApplicationDetail(candidate, vacancyDetails, applicationTemplate);

            if (vacancyDetails.ApplyViaEmployerWebsite && !saveVacancyOnly)
            {
                return applicationDetail;
            }

            _apprenticeshipApplicationWriteRepository.Save(applicationDetail);

            return applicationDetail;
        }

        private static ApprenticeshipApplicationDetail CreateApplicationDetail(
            Candidate candidate, ApprenticeshipVacancyDetail vacancyDetails, ApplicationTemplate applicationTemplate)
        {
            return new ApprenticeshipApplicationDetail
            {
                EntityId = Guid.NewGuid(),
                Status = ApplicationStatuses.Draft,
                DateCreated = DateTime.Now,
                CandidateId = candidate.EntityId,
                CandidateDetails = Mapper.Map<RegistrationDetails, RegistrationDetails>(candidate.RegistrationDetails),
                VacancyStatus = vacancyDetails.VacancyStatus,
                Vacancy = new ApprenticeshipSummary
                {
                    Id = vacancyDetails.Id,
                    VacancyReference = vacancyDetails.VacancyReference,
                    Title = vacancyDetails.Title,
                    EmployerName = vacancyDetails.IsEmployerAnonymous ? vacancyDetails.AnonymousEmployerName : vacancyDetails.EmployerName,
                    StartDate = vacancyDetails.StartDate,
                    ClosingDate = vacancyDetails.ClosingDate,
                    Description = vacancyDetails.Description,
                    Location = null, // NOTE: no equivalent in legacy vacancy details.
                    VacancyLocationType = vacancyDetails.VacancyLocationType
                },
                // Populate apprenticeshipApplication template with candidate's most recent information.
                CandidateInformation = applicationTemplate
            };
        }

        #endregion
    }
}
