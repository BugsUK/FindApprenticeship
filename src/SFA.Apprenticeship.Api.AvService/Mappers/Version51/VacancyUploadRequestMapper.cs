namespace SFA.Apprenticeship.Api.AvService.Mappers.Version51
{
    using System;
    using System.Linq;
    using Apprenticeships.Domain.Entities.Locations;
    using Apprenticeships.Domain.Entities.Organisations;
    using Apprenticeships.Domain.Entities.Providers;
    using Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies;
    using Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using MessageContracts.Version51;

    public class VacancyUploadRequestMapper : IVacancyUploadRequestMapper
    {
        public ApprenticeshipVacancy ToApprenticeshipVacancy(VacancyUploadRequest request)
        {
            var messageId = request.MessageId;
            var externalSystemId = request.ExternalSystemId;
            var publicKey = request.PublicKey;

            Console.WriteLine(messageId);
            Console.WriteLine(externalSystemId);
            Console.WriteLine(publicKey);

            var vacancy = request.Vacancies.First();

            return new ApprenticeshipVacancy
            {
                VacancyReferenceNumber = 0,
                Ukprn = "TODO: Ukprn",
                Title = vacancy.Title,
                ShortDescription = vacancy.ShortDescription,
                LongDescription = vacancy.LongDescription,

                ProviderSiteEmployerLink = new ProviderSiteEmployerLink
                {
                    ProviderSiteErn = string.Empty, // TODO
                    Description = vacancy.Employer.Description,
                    // TODO: ? = vacancy.Employer.AnonymousName
                    // TODO: ? = vacancy.Employer.ContactName
                    WebsiteUrl = vacancy.Employer.Website,
                    // TODO: ? = vacancy.Employer.Image
                    Employer = new Employer
                    {
                        DateCreated = DateTime.UtcNow, // TODO: EntityId too?
                        Ern = vacancy.Employer.EdsUrn.ToString(), // TODO: simple conversion to string
                        Name = string.Empty, // TODO
                        Address = new Address // TODO
                        {
                        }                        
                    }
                },

                WorkingWeek = vacancy.Vacancy.WorkingWeek,
                HoursPerWeek = 0m,

                WageType = Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies.WageType.Custom, // TODO
                Wage = vacancy.Vacancy.Wage,
                WageUnit = WageUnit.Weekly, // TODO

                DurationType = DurationType.Years, // TODO: vacancy.Apprenticeship.ExpectedDuration
                Duration = default(int?), // TODO: vacancy.Apprenticeship.ExpectedDuration

                ClosingDate = vacancy.Application.ClosingDate,
                PossibleStartDate = vacancy.Application.PossibleStartDate,

                DesiredSkills = vacancy.Vacancy.SkillsRequired,
                FutureProspects = vacancy.Vacancy.FutureProspects,
                PersonalQualities = vacancy.Vacancy.PersonalQualities,
                ThingsToConsider = string.Empty, // TODO: string
                DesiredQualifications = vacancy.Vacancy.QualificationRequired,
                // TODO: Xxx = vacancy.Vacancy.OtherImportantInformation
                // TODO: Xxx  = vacancy.Vacancy.RealityCheck

                FirstQuestion = vacancy.Vacancy.SupplementaryQuestion1,
                SecondQuestion = vacancy.Vacancy.SupplementaryQuestion2,

                OfflineVacancy = false, // TODO
                OfflineApplicationUrl = string.Empty, // TODO
                OfflineApplicationInstructions = string.Empty, // TODO

                DateSubmitted = DateTime.UtcNow,

                // TODO
                TrainingType = TrainingType.Unknown, // TODO
                ApprenticeshipLevel = ApprenticeshipLevel.Unknown, // TODO
                FrameworkCodeName = vacancy.Apprenticeship.Framework, // TODO
                StandardId = default(int?), // TODO
                Status = ProviderVacancyStatuses.PendingQA, // TODO
                DateCreated = DateTime.UtcNow // TODO
            };
        }
    }
}
