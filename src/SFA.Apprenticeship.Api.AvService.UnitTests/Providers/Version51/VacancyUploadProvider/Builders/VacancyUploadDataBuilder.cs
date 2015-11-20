namespace SFA.Apprenticeship.Api.AvService.UnitTests.Providers.Version51.VacancyUploadProvider.Builders
{
    using System;
    using System.Collections.Generic;
    using Common;
    using DataContracts.Version51;

    public class VacancyUploadDataBuilder
    {
        private Guid _vacancyId = Guid.Empty;

        public VacancyUploadData Build()
        {
            return new VacancyUploadData
            {
                VacancyId = _vacancyId,
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
                    Type = VacancyApprenticeshipType.IntermediateLevelApprenticeship,
                    TrainingToBeProvided = string.Empty,
                    ExpectedDuration = string.Empty
                },
                ContractedProviderUkprn = default(int?),
                VacancyOwnerEdsUrn = -1,
                VacancyManagerEdsUrn = default(int?),
                DeliveryProviderEdsUrn = default(int?),
                IsDisplayRecruitmentAgency = default(bool?),
                IsSmallEmployerWageIncentive = false
            };
        }

        public VacancyUploadDataBuilder WithVacancyId(Guid vacancyId)
        {
            _vacancyId = vacancyId;
            return this;
        }
    }
}
