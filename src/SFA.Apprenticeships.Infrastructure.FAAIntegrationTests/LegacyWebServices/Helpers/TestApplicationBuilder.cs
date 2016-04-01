namespace SFA.Apprenticeships.Infrastructure.FAAIntegrationTests.LegacyWebServices.Helpers
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Entities.Vacancies;
    using Domain.Entities.Vacancies.Apprenticeships;

    public class TestApplicationBuilder
    {
        private readonly Guid _entityid = new Guid();
        private int _vacancyId = 12345;
        private ApplicationTemplate _candidateInformation;

        public ApprenticeshipApplicationDetail Build()
        {
            if (_candidateInformation == null)
            {
                _candidateInformation = new ApplicationTemplate
                {
                    AboutYou = new AboutYou(),
                    EducationHistory = null
                };
            }
            return new ApprenticeshipApplicationDetail
            {
                EntityId = _entityid,
                VacancyStatus = VacancyStatuses.Live,
                Vacancy = new ApprenticeshipSummary
                {
                    Id = _vacancyId
                },
                CandidateInformation = _candidateInformation,
                AdditionalQuestion1Answer = "Integration Test",
                AdditionalQuestion2Answer = "Integration Test",
            };
        }

        public TestApplicationBuilder WithCandidateInformation()
        {
            _candidateInformation = new ApplicationTemplate
            {
                AboutYou = CreateFakeAboutYou(),
                EducationHistory = CreateFakeEducationHistory(),
                Qualifications = CreateFakeQualifications(),
                WorkExperience = CreateFakeWorkExperience(),
                TrainingCourses = CreateFakeTrainingCourses(),
            };

            return this;
        }

        public TestApplicationBuilder WithVacancyId(int vacancyId)
        {
            _vacancyId = vacancyId;
            return this;
        }

        public static ApprenticeshipApplicationDetail CreateFakeApplicationDetail()
        {
            return new ApprenticeshipApplicationDetail
            {
                EntityId = Guid.NewGuid(),
                VacancyStatus = VacancyStatuses.Live,
                Vacancy = new ApprenticeshipSummary
                {
                    Id = 12345 // legacy vacancy id
                },
                CandidateInformation = new ApplicationTemplate
                {
                    AboutYou = CreateFakeAboutYou(),
                    EducationHistory = CreateFakeEducationHistory(),
                    Qualifications = CreateFakeQualifications(),
                    WorkExperience = CreateFakeWorkExperience()
                }
            };
        }

        public static ApprenticeshipApplicationDetail CreateFakeMinimalApplicationDetail()
        {
            return new ApprenticeshipApplicationDetail
            {
                EntityId = Guid.NewGuid(),
                VacancyStatus = VacancyStatuses.Live,
                Vacancy = new ApprenticeshipSummary
                {
                    Id = 12345 // legacy vacancy id
                },
                CandidateInformation = new ApplicationTemplate
                {
                    AboutYou = null,
                    EducationHistory = null
                }
            };
        }


        private static AboutYou CreateFakeAboutYou()
        {
            return new AboutYou
            {
                Strengths = "Strengths",
                Improvements = "Improvements",
                HobbiesAndInterests = "HobbiesAndInterests",
                Support = "Support"
            };
        }

        private static Education CreateFakeEducationHistory()
        {
            return new Education
            {
                Institution = "Bash Street School",
                FromYear = 2008,
                ToYear = 2010
            };
        }

        private static List<WorkExperience> CreateFakeWorkExperience()
        {
            return new List<WorkExperience>
            {
                new WorkExperience
                {
                    Employer = "Acme Corp",
                    FromDate = new DateTime(2011, 1, 1),
                    ToDate = new DateTime(2012, 1, 1),
                    Description = "Barista"
                },
                new WorkExperience
                {
                    Employer = "Nether Products",
                    FromDate = new DateTime(2011, 1, 1),
                    ToDate = new DateTime(2012, 1, 1),
                    Description = "Cashier"
                },
                new WorkExperience
                {
                    Employer = "Argos",
                    FromDate = new DateTime(2011, 1, 1),
                    ToDate = DateTime.MinValue,
                    Description = "Counter Staff"
                }
            };
        }


        private static List<TrainingCourse> CreateFakeTrainingCourses()
        {
            return new List<TrainingCourse>
            {
                new TrainingCourse
                {
                    Provider = "Serbian Language Institute",
                    Title = "Conversational Serbian",
                    FromDate = new DateTime(2011, 1, 1),
                    ToDate = new DateTime(2012, 1, 1),
                },
                new TrainingCourse
                {
                    Provider = "Woodworking Inc.",
                    Title = "See Saw",
                    FromDate = new DateTime(2011, 1, 1),
                    ToDate = new DateTime(2012, 1, 1),
                }
            };
        }

        private static List<Qualification> CreateFakeQualifications()
        {
            return new List<Qualification>
            {
                new Qualification
                {
                    Subject = "English",
                    Year = 2009,
                    Grade = "A",
                    IsPredicted = false,
                    QualificationType = "GCSE"
                },
                new Qualification
                {
                    Subject = "Maths",
                    Year = 2010,
                    Grade = "C",
                    IsPredicted = true,
                    QualificationType = "A Level"
                }
            };
        }
    }
}
