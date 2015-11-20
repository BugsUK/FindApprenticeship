namespace SFA.Apprenticeship.Api.AvService.Mappers.Version51
{
    using System;
    using Apprenticeships.Domain.Entities.Locations;
    using Apprenticeships.Domain.Entities.Organisations;
    using Apprenticeships.Domain.Entities.Providers;
    using Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies;
    using Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using DataContracts.Version51;

    public class VacancyUploadRequestMapper : IVacancyUploadRequestMapper
    {
        public ApprenticeshipVacancy ToApprenticeshipVacancy(VacancyUploadData vacancyUploadData)
        {
            return new ApprenticeshipVacancy
            {
                VacancyReferenceNumber = 0,
                Ukprn = "TODO: Ukprn",
                Title = vacancyUploadData.Title,
                ShortDescription = vacancyUploadData.ShortDescription,
                LongDescription = vacancyUploadData.LongDescription,

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

                WorkingWeek = vacancyUploadData.Vacancy.WorkingWeek,
                HoursPerWeek = 0m,

                WageType = WageType.Custom, // TODO
                Wage = vacancyUploadData.Vacancy.Wage,
                WageUnit = WageUnit.Weekly, // TODO

                DurationType = DurationType.Years, // TODO: vacancy.Apprenticeship.ExpectedDuration
                Duration = default(int?), // TODO: vacancy.Apprenticeship.ExpectedDuration

                ClosingDate = vacancyUploadData.Application.ClosingDate,
                PossibleStartDate = vacancyUploadData.Application.PossibleStartDate,

                DesiredSkills = vacancyUploadData.Vacancy.SkillsRequired,
                FutureProspects = vacancyUploadData.Vacancy.FutureProspects,
                PersonalQualities = vacancyUploadData.Vacancy.PersonalQualities,
                ThingsToConsider = string.Empty, // TODO: string
                DesiredQualifications = vacancyUploadData.Vacancy.QualificationRequired,
                // TODO: Xxx = vacancy.Vacancy.OtherImportantInformation
                // TODO: Xxx  = vacancy.Vacancy.RealityCheck

                FirstQuestion = vacancyUploadData.Vacancy.SupplementaryQuestion1,
                SecondQuestion = vacancyUploadData.Vacancy.SupplementaryQuestion2,

                OfflineVacancy = false, // TODO
                OfflineApplicationUrl = string.Empty, // TODO
                OfflineApplicationInstructions = string.Empty, // TODO

                DateSubmitted = DateTime.UtcNow,

                // TODO
                TrainingType = TrainingType.Unknown, // TODO
                ApprenticeshipLevel = ApprenticeshipLevel.Unknown, // TODO
                FrameworkCodeName = vacancyUploadData.Apprenticeship.Framework, // TODO
                StandardId = default(int?), // TODO
                Status = ProviderVacancyStatuses.PendingQA, // TODO
                DateCreated = DateTime.UtcNow // TODO
            };
        }
    }
}
