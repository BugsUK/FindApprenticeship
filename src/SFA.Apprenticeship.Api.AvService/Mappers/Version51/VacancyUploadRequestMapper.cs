namespace SFA.Apprenticeship.Api.AvService.Mappers.Version51
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Apprenticeships.Domain.Entities.Locations;
    using Apprenticeships.Domain.Entities.Organisations;
    using Apprenticeships.Domain.Entities.Providers;
    using Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies;
    using Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using Common;
    using DataContracts.Version51;
    using MessageContracts.Version51;
    using WageType = Common.WageType;

    public static class VacancyUploadRequestMapper
    {
        public static ApprenticeshipVacancy ToApprenticeshipVacancy(VacancyUploadRequest request)
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

        private static VacancyUploadRequest ToDummyApprenticeshipVacancy()
        {
            return new VacancyUploadRequest
            {
                MessageId = Guid.Empty,
                ExternalSystemId = Guid.Empty,
                PublicKey = string.Empty,
                Vacancies = new List<VacancyUploadData>
                {
                    new VacancyUploadData
                    {
                        VacancyId = Guid.Empty,
                        Title = string.Empty,
                        ShortDescription = string.Empty,
                        LongDescription = string.Empty,
                        Employer = new EmployerData
                        {                            
                            EdsUrn = 0,
                            Description = string.Empty,
                            AnonymousName = string.Empty,
                            ContactName = string.Empty,
                            Website = string.Empty,
                            Image = new byte[] {}
                        },
                        Vacancy = new VacancyData
                        {
                            Wage = 0.0m,
                            WageType = WageType.Text,
                            WorkingWeek = string.Empty,
                            SkillsRequired = string.Empty,
                            QualificationRequired = string.Empty,
                            PersonalQualities = string.Empty,
                            FutureProspects = string.Empty,
                            OtherImportantInformation = string.Empty,
                            LocationType = VacancyLocationType.Standard,
                            LocationDetails = new List<SiteVacancyData>
                            {
                                new SiteVacancyData
                                {
                                    AddressDetails = new AddressData
                                    {
                                        AddressLine1 = string.Empty,
                                        AddressLine2 = string.Empty,
                                        AddressLine3 = string.Empty,
                                        AddressLine4 = string.Empty,
                                        AddressLine5 = string.Empty,
                                        County = string.Empty,
                                        GridEastM = 0,
                                        GridNorthM = 0,
                                        Latitude = 0m,
                                        Longitude = 0m,
                                        PostCode = string.Empty,
                                        Town = string.Empty,
                                        LocalAuthority = string.Empty
                                    },
                                    NumberOfVacancies = -1,
                                    EmployerWebsite = string.Empty
                                }
                            },
                            RealityCheck = string.Empty,
                            SupplementaryQuestion1 = string.Empty,
                            SupplementaryQuestion2 = string.Empty
                        },
                        Application = new ApplicationData
                        {
                            ClosingDate = DateTime.Today,
                            InterviewStartDate = DateTime.Today,
                            PossibleStartDate = DateTime.Today,
                            Type = ApplicationType.Online,
                            Instructions = string.Empty
                        },
                        Apprenticeship = new ApprenticeshipData
                        {
                            Framework = string.Empty,
                            Type = VacancyApprenticeshipType.Online,
                            TrainingToBeProvided = string.Empty,
                            ExpectedDuration = string.Empty
                        },
                        ContractedProviderUkprn = default(int?),
                        VacancyOwnerEdsUrn = -1,
                        VacancyManagerEdsUrn = default(int?),
                        DeliveryProviderEdsUrn = default(int?),
                        IsDisplayRecruitmentAgency = default(bool?),
                        IsSmallEmployerWageIncentive = false
                    }
                }
            };
        }
    }
}
