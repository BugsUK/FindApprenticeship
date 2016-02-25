namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Tests.Schemas.Vacancy
{
    using System;
    using FluentAssertions.Equivalency;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using Vacancy = Sql.Schemas.Vacancy.Entities.Vacancy;
    using DomainVacancy = Domain.Entities.Raa.Vacancies.Vacancy;
    using Domain.Entities.Raa.Vacancies;

    [TestFixture]
    public class TestBase
    {
        protected static readonly Guid VacancyIdVacancyA = Guid.NewGuid();
        protected const int VacancyReferenceNumberVacancyA = 1;
        protected const int FrameworkIdFramework1 = 1;
        protected const string FramworkCodeNameFramework1 = "F01";
        protected const int FrameworkIdFramework2 = 2;
        protected const int StandardIdStandard1 = 1;
        protected int VacancyReferenceNumber = 10;

        protected Vacancy CreateValidDatabaseVacancy()
        {
            var fixture = new Fixture();

            var result = fixture.Build<Vacancy>()
                //.With(v => v.WageTypeCode, WageTypeCode_Custom)
                //.With(v => v.WageIntervalCode, WageIntervalCode_Weekly)
                //.With(v => v.DurationTypeCode, DurationTypeCode_Years)
                //.With(v => v.TrainingTypeCode, TrainingTypeCode_Framework)
                //.With(v => v.VacancyStatusCode, VacancyStatusCode_Live)
                //.With(v => v.LevelCode, LevelCode_Intermediate)
                //.With(v => v.VacancyTypeCode, VacancyTypeCode_Apprenticeship) // TODO: This is cheating the test as not mapped
                .Create();

            //if (fixture.Create<bool>())
            //{
            //    result.TrainingTypeCode = TrainingTypeCode_Framework;
            //    result.FrameworkId = FrameworkIdFramework1;
            //    result.StandardId = null;
            //}
            //else
            //{
            //    result.TrainingTypeCode = TrainingTypeCode_Standard;
            //    result.FrameworkId = null;
            //    result.StandardId = StandardIdStandard1;
            //}

            //result.VacancyLocationTypeCode = fixture.Create<bool>() ? VacancyLocationType.Employer : VacancyLocationType.Specific;

            return result;
        }

        protected DomainVacancy CreateValidDomainVacancy()
        {
            var fixture = new Fixture();

            fixture.Customizations.Add(
                new StringGenerator(() =>
                    Guid.NewGuid().ToString().Substring(0, 10)));

            var result = fixture.Build<DomainVacancy>()
                .With(av => av.Status, VacancyStatus.PendingQA)
                .With(av => av.DateSubmitted, null)
                .With(av => av.QAUserName, null)
                .With(av => av.DateStartedToQA, null)
                .With(av => av.DateQAApproved, null)
                .With(av => av.VacancyReferenceNumber, VacancyReferenceNumber++)
                .With(av => av.IsEmployerLocationMainApprenticeshipLocation, true)
                .With(av => av.ParentVacancyReferenceNumber, 0)
                .Create();

            if (result.FrameworkCodeName != null && result.FrameworkCodeName.GetHashCode() % 2 == 1)
            {
                result.TrainingType = TrainingType.Frameworks;
                result.FrameworkCodeName = FramworkCodeNameFramework1;
                result.StandardId = null;
            }
            else
            {
                result.TrainingType = TrainingType.Standards;
                result.FrameworkCodeName = null;
                result.StandardId = StandardIdStandard1;
            }

            //result.ProviderSiteEmployerLink.Employer.Ern = "101";
            //result.ProviderSiteEmployerLink.Employer.Address.Postcode = "CV1 2WT";
            //result.Ukprn = "202"; //TODO: check with database values.

            return result;
        }

        protected Tuple<DomainVacancy, DomainVacancy> CreateValidParentChildDomainVacancies()
        {
            var parentVacancy = CreateValidDomainVacancy();

            var childVacancy = CreateValidDomainVacancy();
            childVacancy.ParentVacancyReferenceNumber = parentVacancy.VacancyReferenceNumber;

            return new Tuple<DomainVacancy, DomainVacancy>(parentVacancy, childVacancy);
        }

        protected EquivalencyAssertionOptions<Vacancy> ExcludeHardOnes(EquivalencyAssertionOptions<Vacancy> options)
        {
            return options
                // TODO: Not in Domain object yet
                //.Excluding(v => v.AV_ContactName)
                //.Excluding(v => v.AV_WageText)
                ;
        }

        protected EquivalencyAssertionOptions<DomainVacancy> ExcludeHardOnes(EquivalencyAssertionOptions<DomainVacancy> options)
        {
            return options
                // TODO: Might be easier?
                .Excluding(v => v.FrameworkCodeName)
                //.Excluding(v => v.Ukprn)
                //.Excluding(v => v.ProviderSiteEmployerLink)
                //.Excluding(v => v.VacancyManagerId)
                //.Excluding(v => v.LastEditedById)
                ;
        }

        protected EquivalencyAssertionOptions<DomainVacancy> ForShallowSave(EquivalencyAssertionOptions<DomainVacancy> options)
        {
            return options
                // TODO: Might be easier?
                .Excluding(v => v.FrameworkCodeName)
                //.Excluding(v => v.Ukprn)
                //.Excluding(v => v.ProviderSiteEmployerLink)
                ;
        }
    }
}
