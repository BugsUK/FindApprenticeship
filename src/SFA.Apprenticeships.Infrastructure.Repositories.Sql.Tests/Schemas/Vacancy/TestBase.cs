namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Tests.Schemas.Vacancy
{
    using System;
    using System.Linq;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using FluentAssertions.Equivalency;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using TrainingType = Domain.Entities.Vacancies.ProviderVacancies.TrainingType;
    using Vacancy = Sql.Schemas.Vacancy.Entities.Vacancy;
    using VacancyLocationType = Sql.Schemas.Vacancy.Entities.VacancyLocationType;
    using System.Collections.Generic;
    [TestFixture]
    public class TestBase
    {
        protected static readonly Guid VacancyId_VacancyA = Guid.NewGuid();
        protected static readonly Guid VacancyId_VacancyAParent = Guid.NewGuid();
        protected const int VacancyReferenceNumber_VacancyA = 1;
        protected const int VacancyPartyId_EmployerA = 3;
        protected const int VacancyPartyId_ProviderA = 4;
        protected const int FrameworkId_Framework1 = 1;
        protected const string FramworkCodeName_Framework1 = "F01";
        protected const int FrameworkId_Framework2 = 2;
        protected const int StandardId_Standard1 = 1;
        protected const string VacancyTypeCode_Apprenticeship = "A";
        protected const string VacancyStatusCode_Live = "LIV";
        protected const string VacancyStatusCode_Parent = "PAR";
        protected const string TrainingTypeCode_Framework = "F";
        protected const string TrainingTypeCode_Standard = "S";
        protected const string LevelCode_Intermediate = "2";
        protected const string WageTypeCode_Custom = "CUS";
        protected const string WageIntervalCode_Weekly = "W";
        protected const string DurationTypeCode_Years = "Y";
        protected const string Ukprn = "Ukrpn provider 1";
        protected const string EmployerErn = "Employer 1 ern";
        protected const string ProviderSiteErn = "Provider 1 site ern";
        protected int vacancyReferenceNumber = 10;

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
            //    result.FrameworkId = FrameworkId_Framework1;
            //    result.StandardId = null;
            //}
            //else
            //{
            //    result.TrainingTypeCode = TrainingTypeCode_Standard;
            //    result.FrameworkId = null;
            //    result.StandardId = StandardId_Standard1;
            //}

            //result.VacancyLocationTypeCode = fixture.Create<bool>() ? VacancyLocationType.Employer : VacancyLocationType.Specific;

            return result;
        }

        protected ApprenticeshipVacancy CreateValidDomainVacancy()
        {
            var fixture = new Fixture();

            fixture.Customizations.Add(
                new StringGenerator(() =>
                    Guid.NewGuid().ToString().Substring(0, 10)));

            var result = fixture.Build<ApprenticeshipVacancy>()
                .With(av => av.Status, ProviderVacancyStatuses.PendingQA)
                .With(av => av.DateSubmitted, null)
                .With(av => av.QAUserName, null)
                .With(av => av.DateStartedToQA, null)
                .With(av => av.DateQAApproved, null)
                .With(av => av.VacancyReferenceNumber, vacancyReferenceNumber++)
                .With(av => av.IsEmployerLocationMainApprenticeshipLocation, true)
                .With(av => av.ParentVacancyId, null)
                .Create();

            if (result.FrameworkCodeName != null && result.FrameworkCodeName.GetHashCode() % 2 == 1)
            {
                result.TrainingType = TrainingType.Frameworks;
                result.FrameworkCodeName = FramworkCodeName_Framework1;
                result.StandardId = null;
            }
            else
            {
                result.TrainingType = TrainingType.Standards;
                result.FrameworkCodeName = null;
                result.StandardId = StandardId_Standard1;
            }

            result.ProviderSiteEmployerLink.Employer.Ern = "101";
            result.ProviderSiteEmployerLink.Employer.Address.Postcode = "CV1 2WT";
            result.Ukprn = "202"; //TODO: check with database values.

            if (result.IsEmployerLocationMainApprenticeshipLocation.GetValueOrDefault())
            {
                result.LocationAddresses = new List<Domain.Entities.Locations.VacancyLocationAddress>();
            }

            return result;
        }

        protected Tuple<ApprenticeshipVacancy, ApprenticeshipVacancy> CreateValidParentChildDomainVacancies()
        {
            var parentVacancy = CreateValidDomainVacancy();

            var childVacancy = CreateValidDomainVacancy();
            childVacancy.ParentVacancyId = parentVacancy.VacancyGuid;

            return new Tuple<ApprenticeshipVacancy, ApprenticeshipVacancy>(parentVacancy, childVacancy);
        }

        protected EquivalencyAssertionOptions<Vacancy> ExcludeHardOnes(EquivalencyAssertionOptions<Vacancy> options)
        {
            return options
                // TODO: Not in Domain object yet
                //.Excluding(v => v.AV_ContactName)
                //.Excluding(v => v.AV_WageText)
                ;
        }

        protected EquivalencyAssertionOptions<ApprenticeshipVacancy> ExcludeHardOnes(EquivalencyAssertionOptions<ApprenticeshipVacancy> options)
        {
            return options
                // TODO: Might be easier?
                .Excluding(v => v.FrameworkCodeName)
                .Excluding(v => v.Ukprn)
                .Excluding(v => v.ProviderSiteEmployerLink)
                //.Excluding(v => v.VacancyManagerId)
                //.Excluding(v => v.LastEditedById)
                ;
        }

    }
}