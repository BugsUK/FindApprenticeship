namespace SFA.Apprenticeship.Api.AvService.UnitTests.Builders.Version51
{
    using System;
    using System.Collections.Generic;
    using Common;
    using DataContracts.Version51;

    // TODO: AG: US872: review all unmapped fields in this class and comment.

    public class VacancyUploadDataBuilder
    {
        private Guid _vacancyId = Guid.Empty;

        private int _contractedProviderUkprn;

        private string _title;
        private string _shortDescription;
        private string _longDescription;
        private string _workingWeek;
        private WageType _wageType;
        private decimal _wage;
        private string _expectedDuration;
        private DateTime _closingDate;
        private DateTime _interviewStartDate;
        private DateTime _possibleStartDate;
        private string _skillsRequired;
        private string _futureProspects;
        private string _personalQualities;
        private string _qualificationRequired;
        private string _realityCheck;
        private string _supplementaryQuestion1;
        private string _supplementaryQuestion2;
        private string _employerWebsite;
        private ApplicationType _applicationType;
        private string _employersApplicationInstructions;
        private string _frameworkCode;
        private VacancyApprenticeshipType _vacancyApprenticeshipType = VacancyApprenticeshipType.IntermediateLevelApprenticeship;

        public VacancyUploadData Build()
        {
            return new VacancyUploadData
            {
                VacancyId = _vacancyId,
                Title = _title,
                ShortDescription = _shortDescription,
                LongDescription = _longDescription,
                Employer = new EmployerData
                {
                    EdsUrn = 0,
                    Description = null,
                    // TODO: US872: AG: AnonymousName not mapped?
                    AnonymousName = null,
                    ContactName = null,
                    Website = null,
                    Image = new byte[] { }
                },
                Vacancy = new VacancyData
                {
                    Wage = _wage,
                    WageType = _wageType,
                    WorkingWeek = _workingWeek,
                    QualificationRequired = _qualificationRequired,
                    PersonalQualities = _personalQualities,
                    FutureProspects = _futureProspects,
                    SkillsRequired = _skillsRequired,
                    // TODO: US872: AG: OtherImportantInformation not mapped?
                    OtherImportantInformation = null,
                    // TODO: US872: AG: handle multi-location vacancies.
                    LocationType = VacancyLocationType.Standard,
                    LocationDetails = new List<SiteVacancyData>
                    {
                        new SiteVacancyData
                        {
                            AddressDetails = new AddressData
                            {
                                AddressLine1 = null,
                                AddressLine2 = null,
                                AddressLine3 = null,
                                AddressLine4 = null,
                                AddressLine5 = null,
                                County = null,
                                GridEastM = 0,
                                GridNorthM = 0,
                                Latitude = 0m,
                                Longitude = 0m,
                                PostCode = null,
                                Town = null,
                                LocalAuthority = null
                            },
                            NumberOfVacancies = -1,
                            // TODO: US872: AG: currently only one saved per vacancy, not per location.
                            EmployerWebsite = _employerWebsite
                        }
                    },
                    RealityCheck = _realityCheck,
                    SupplementaryQuestion1 = _supplementaryQuestion1,
                    SupplementaryQuestion2 = _supplementaryQuestion2
                },
                Application = new ApplicationData
                {
                    ClosingDate = _closingDate,
                    InterviewStartDate = _interviewStartDate,
                    PossibleStartDate = _possibleStartDate,
                    Type = _applicationType,
                    Instructions = _employersApplicationInstructions
                },
                Apprenticeship = new ApprenticeshipData
                {
                    Framework = _frameworkCode,
                    Type = _vacancyApprenticeshipType,
                    // TODO: US872: AG: TrainingToBeProvided not mapped?
                    TrainingToBeProvided = null,
                    ExpectedDuration = _expectedDuration
                },
                ContractedProviderUkprn = _contractedProviderUkprn,
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

        public VacancyUploadDataBuilder WithContractedProviderUkprn(int contractedProviderUkprn)
        {
            _contractedProviderUkprn = contractedProviderUkprn;
            return this;
        }

        public VacancyUploadDataBuilder WithTitle(string title)
        {
            _title = title;
            return this;
        }

        public VacancyUploadDataBuilder WithShortDescription(string shortDescription)
        {
            _shortDescription = shortDescription;
            return this;
        }

        public VacancyUploadDataBuilder WithLongDescription(string longDescription)
        {
            _longDescription = longDescription;
            return this;
        }

        public VacancyUploadDataBuilder WithWorkingWeek(string workingWeek)
        {
            _workingWeek = workingWeek;
            return this;
        }

        public VacancyUploadDataBuilder WithWage(WageType wageType, decimal wage)
        {
            _wageType = wageType;
            _wage = wage;
            return this;
        }

        public VacancyUploadDataBuilder WithExpectedDuration(string expectedDuration)
        {
            _expectedDuration = expectedDuration;
            return this;
        }

        public VacancyUploadDataBuilder WithClosingDate(DateTime closingDate)
        {
            _closingDate = closingDate;
            return this;
        }

        public VacancyUploadDataBuilder WithInterviewStartDate(DateTime interviewStartDate)
        {
            _interviewStartDate = interviewStartDate;
            return this;
        }

        public VacancyUploadDataBuilder WithPossibleStartDate(DateTime possibleStartDate)
        {
            _possibleStartDate = possibleStartDate;
            return this;
        }

        public VacancyUploadDataBuilder WithSkillsRequired(string skillsRequired)
        {
            _skillsRequired = skillsRequired;
            return this;
        }

        public VacancyUploadDataBuilder WithFutureProspects(string futureProspects)
        {
            _futureProspects = futureProspects;
            return this;
        }

        public VacancyUploadDataBuilder WithPersonalQualities(string personalQualities)
        {
            _personalQualities = personalQualities;
            return this;
        }

        public VacancyUploadDataBuilder WithQualificationRequired(string qualificationRequired)
        {
            _qualificationRequired = qualificationRequired;
            return this;
        }

        public VacancyUploadDataBuilder WithRealityCheck(string realityCheck)
        {
            _realityCheck = realityCheck;
            return this;
        }

        public VacancyUploadDataBuilder WithSupplementaryQuestions(string supplementaryQuestion1, string supplementaryQuestion2)
        {
            _supplementaryQuestion1 = supplementaryQuestion1;
            _supplementaryQuestion2 = supplementaryQuestion2;
            return this;
        }

        public VacancyUploadDataBuilder WithApplicationType(ApplicationType applicationType)
        {
            _applicationType = applicationType;
            return this;
        }

        public VacancyUploadDataBuilder WithEmployerWebsite(string employerWebsite)
        {
            _employerWebsite = employerWebsite;
            return this;
        }

        public VacancyUploadDataBuilder WithEmployersApplicationInstructions(string employersApplicationInstructions)
        {
            _employersApplicationInstructions = employersApplicationInstructions;
            return this;
        }

        public VacancyUploadDataBuilder WithApprenticeshipFrameworkCode(string frameworkCode)
        {
            _frameworkCode = frameworkCode;
            return this;
        }

        public VacancyUploadDataBuilder WithVacancyApprenticeshipType(VacancyApprenticeshipType vacancyApprenticeshipType)
        {
            _vacancyApprenticeshipType = vacancyApprenticeshipType;
            return this;
        }
    }
}
