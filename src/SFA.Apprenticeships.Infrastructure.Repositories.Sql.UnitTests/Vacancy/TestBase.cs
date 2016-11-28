namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.UnitTests.Vacancy
{
    using System;
    using Domain.Entities.Raa.Locations;
    using Domain.Entities.Raa.Vacancies;
    using FluentAssertions.Equivalency;
    using Ploeh.AutoFixture;
    using Vacancy = Schemas.Vacancy.Entities.Vacancy;
    using DomainVacancy = Domain.Entities.Raa.Vacancies.Vacancy;
    using VacancyLocation = Schemas.Vacancy.Entities.VacancyLocation;

    public abstract class TestBase
    {
        private const string FramworkCodeNameFramework1 = "260";
        private const int StandardIdStandard1 = 1;
        private int _vacancyReferenceNumber = 10;

        protected Vacancy CreateValidDatabaseVacancy()
        {
            var fixture = new Fixture();

            var result = fixture.Build<Vacancy>()
                .With(v => v.WageUnitId, 1)
                .With(v => v.WageType, 1)
                .Create();

            return result;
        }

        protected VacancyLocation CreateValidDatabaseVacancyLocation()
        {
            var fixture = new Fixture();

            var result = fixture.Build<VacancyLocation>()
                .Create();

            return result;
        }

        private DomainVacancy CreateValidDomainVacancy()
        {
            var fixture = new Fixture();

            fixture.Customizations.Add(
                new StringGenerator(() =>
                    Guid.NewGuid().ToString().Substring(0, 10)));

            var result = fixture.Build<DomainVacancy>()
                .With(av => av.Status, VacancyStatus.Submitted)
                .With(av => av.DateSubmitted, null)
                .With(av => av.QAUserName, null)
                .With(av => av.DateStartedToQA, null)
                .With(av => av.DateQAApproved, null)
                .With(av => av.VacancyReferenceNumber, _vacancyReferenceNumber++)
                .With(av => av.EmployerApprenticeshipLocation, VacancyLocationOption.Main)
                .With(av => av.ParentVacancyId, null)
                .With(av => av.UpdatedDateTime, null)
                .Create();

            result.Address = new PostalAddress
            {
                Postcode = "CV1 2WT",
                County = "CAM"
            };

            if (result.FrameworkCodeName != null && result.FrameworkCodeName.GetHashCode() % 2 == 1)
            {
                result.TrainingType = TrainingType.Frameworks;
                result.FrameworkCodeName = FramworkCodeNameFramework1;
                result.StandardId = null;
                result.SectorCodeName = null;
            }
            else
            {
                result.TrainingType = TrainingType.Standards;
                result.FrameworkCodeName = null;
                result.StandardId = StandardIdStandard1;
                result.SectorCodeName = "ALB";
            }

            return result;
        }

        protected Tuple<DomainVacancy, DomainVacancy> CreateValidParentChildDomainVacancies()
        {
            var parentVacancy = CreateValidDomainVacancy();

            var childVacancy = CreateValidDomainVacancy();
            childVacancy.ParentVacancyId = parentVacancy.VacancyReferenceNumber;

            return new Tuple<DomainVacancy, DomainVacancy>(parentVacancy, childVacancy);
        }

        protected EquivalencyAssertionOptions<DomainVacancy> ExcludeHardOnes(EquivalencyAssertionOptions<DomainVacancy> options)
        {
            return options.Excluding(v => v.FrameworkCodeName);
        }
    }
}
