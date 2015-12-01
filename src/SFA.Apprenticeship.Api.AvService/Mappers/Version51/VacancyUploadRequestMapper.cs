namespace SFA.Apprenticeship.Api.AvService.Mappers.Version51
{
    using System;
    using System.Linq;
    using Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies;
    using Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using Common;
    using DataContracts.Version51;
    using WageType = Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies.WageType;

    public class VacancyUploadRequestMapper : IVacancyUploadRequestMapper
    {
        public ApprenticeshipVacancy ToApprenticeshipVacancy(VacancyUploadData vacancyUploadData)
        {
            return new ApprenticeshipVacancy
            {
                VacancyReferenceNumber = 0, // allocated on save
                Ukprn = vacancyUploadData.ContractedProviderUkprn.ToString(),
                Title = vacancyUploadData.Title,
                ShortDescription = vacancyUploadData.ShortDescription,
                LongDescription = vacancyUploadData.LongDescription,
                /*
                ProviderSiteEmployerLink = new ProviderSiteEmployerLink
                {
                    ProviderSiteErn = string.Empty, // TODO
                    Description = vacancyUploadData.Employer.Description,
                    // TODO: ? = vacancy.Employer.AnonymousName
                    // TODO: ? = vacancy.Employer.ContactName
                    WebsiteUrl = vacancyUploadData.Employer.Website,
                    // TODO: ? = vacancy.Employer.Image
                    Employer = new Employer
                    {
                        DateCreated = DateTime.UtcNow, // TODO: EntityId too?
                        Ern = vacancyUploadData.Employer.EdsUrn.ToString(), // TODO: simple conversion to string
                        Name = string.Empty, // TODO
                        Address = new Address() // TODO
                    }
                },
                */

                WorkingWeek = vacancyUploadData.Vacancy.WorkingWeek,
                HoursPerWeek = null,

                WageType = WageType.Custom,
                WageUnit = WageUnit.Weekly,
                Wage = vacancyUploadData.Vacancy.Wage,

                DurationType = DurationType.Text,
                Duration = default(int?),
                DurationComment = vacancyUploadData.Apprenticeship.ExpectedDuration,

                ClosingDate = vacancyUploadData.Application.ClosingDate,
                InterviewStartDate = vacancyUploadData.Application.InterviewStartDate,
                PossibleStartDate = vacancyUploadData.Application.PossibleStartDate,

                DesiredSkills = vacancyUploadData.Vacancy.SkillsRequired,
                FutureProspects = vacancyUploadData.Vacancy.FutureProspects,
                PersonalQualities = vacancyUploadData.Vacancy.PersonalQualities,
                ThingsToConsider = vacancyUploadData.Vacancy.RealityCheck,
                DesiredQualifications = vacancyUploadData.Vacancy.QualificationRequired,

                FirstQuestion = vacancyUploadData.Vacancy.SupplementaryQuestion1,
                SecondQuestion = vacancyUploadData.Vacancy.SupplementaryQuestion2,

                OfflineVacancy = vacancyUploadData.Application.Type == ApplicationType.Offline,
                OfflineApplicationUrl = vacancyUploadData.Vacancy.LocationDetails.First().EmployerWebsite,
                OfflineApplicationInstructions = vacancyUploadData.Application.Instructions,

                ApprenticeshipLevel = MapVacancyApprenticeshipType(vacancyUploadData.Apprenticeship.Type),
                FrameworkCodeName = vacancyUploadData.Apprenticeship.Framework,
                TrainingType = TrainingType.Frameworks,
                StandardId = default(int?),
                Status = ProviderVacancyStatuses.Draft,
                DateCreated = DateTime.UtcNow
            };
        }

        #region Helpers

        private static ApprenticeshipLevel MapVacancyApprenticeshipType(VacancyApprenticeshipType vacancyApprenticeshipType)
        {
            switch (vacancyApprenticeshipType)
            {
                case VacancyApprenticeshipType.IntermediateLevelApprenticeship:
                    return ApprenticeshipLevel.Intermediate;
                case VacancyApprenticeshipType.AdvancedLevelApprenticeship:
                    return ApprenticeshipLevel.Advanced;
                case VacancyApprenticeshipType.HigherApprenticeship:
                    return ApprenticeshipLevel.Higher;
            }

            throw new ArgumentOutOfRangeException(
                nameof(vacancyApprenticeshipType), vacancyApprenticeshipType, "Invalid vacancy apprenticeship type.");
        }

        #endregion
    }
}
