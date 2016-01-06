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
        public ApprenticeshipVacancy ToNewApprenticeshipVacancy(VacancyUploadData vacancyUploadData)
        {
            return new ApprenticeshipVacancy
            {
                EntityId = Guid.NewGuid(),
                VacancyReferenceNumber = 0, // allocated on save
                Ukprn = vacancyUploadData.ContractedProviderUkprn.ToString(),

                Title = vacancyUploadData.Title,
                TitleComment = null,

                ShortDescription = vacancyUploadData.ShortDescription,
                ShortDescriptionComment = null,

                LongDescription = vacancyUploadData.LongDescription,
                LongDescriptionComment = null,

                ProviderSiteEmployerLink = null,

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
                WorkingWeekComment = null,

                WageType = WageType.Custom,
                WageUnit = WageUnit.Weekly,
                Wage = vacancyUploadData.Vacancy.Wage,
                WageComment = null,

                DurationType = DurationType.Unknown,
                Duration = default(int?),
                DurationComment = vacancyUploadData.Apprenticeship.ExpectedDuration,

                ClosingDate = vacancyUploadData.Application.ClosingDate,
                ClosingDateComment = null,

                InterviewStartDate = vacancyUploadData.Application.InterviewStartDate,

                PossibleStartDate = vacancyUploadData.Application.PossibleStartDate,
                PossibleStartDateComment = null,

                DesiredSkills = vacancyUploadData.Vacancy.SkillsRequired,
                DesiredSkillsComment = null,

                FutureProspects = vacancyUploadData.Vacancy.FutureProspects,
                FutureProspectsComment = null,

                PersonalQualities = vacancyUploadData.Vacancy.PersonalQualities,
                PersonalQualitiesComment = null,

                ThingsToConsider = vacancyUploadData.Vacancy.RealityCheck,
                ThingsToConsiderComment = null,

                DesiredQualifications = vacancyUploadData.Vacancy.QualificationRequired,
                DesiredQualificationsComment = null,

                FirstQuestion = vacancyUploadData.Vacancy.SupplementaryQuestion1,
                FirstQuestionComment = null,

                SecondQuestion = vacancyUploadData.Vacancy.SupplementaryQuestion2,
                SecondQuestionComment = null,

                OfflineVacancy = vacancyUploadData.Application.Type == ApplicationType.Offline,

                OfflineApplicationUrl = vacancyUploadData.Vacancy.LocationDetails.First().EmployerWebsite,
                OfflineApplicationUrlComment = null,

                OfflineApplicationInstructions = vacancyUploadData.Application.Instructions,
                OfflineApplicationInstructionsComment = null,

                ApprenticeshipLevel = MapVacancyApprenticeshipType(vacancyUploadData.Apprenticeship.Type),
                ApprenticeshipLevelComment = null,

                FrameworkCodeName = vacancyUploadData.Apprenticeship.Framework,
                FrameworkCodeNameComment = null,

                TrainingType = TrainingType.Frameworks,
                StandardId = default(int?),
                Status = ProviderVacancyStatuses.Draft,
                DateCreated = DateTime.UtcNow,

                // TODO: US872: NumberOfPositions
                // TODO: US872: IsEmployerLocationMainApprenticeshipLocation
                // TODO: US872: LocationAddresses
                // TODO: US872: AdditionalLocationInformation
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
