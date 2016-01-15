namespace SFA.Apprenticeships.Infrastructure.Repositories.Vacancies.Sql.Tests
{
    using System;
    using Domain.Entities.Organisations;
    using Domain.Entities.Providers;
    using Mappers;
    using NUnit.Framework;
    using SFA.Infrastructure.Interfaces;
    using Vacancy = NewDB.Domain.Entities.Vacancy.Vacancy;
    using Ploeh.AutoFixture;
    using FluentAssertions.Equivalency;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using Domain.Entities.Vacancies.ProviderVacancies;

    [TestFixture]
    public class BaseTests
    {
        readonly IMapper _mapper = new ApprenticeshipVacancyMappers();
        //private Container _container;

        protected static readonly Guid VacancyId_VacancyA = Guid.NewGuid();
        protected const int VacancyReferenceNumber_VacancyA = 1;

        protected const int VacancyPartyId_EmployerA = 3;
        protected const int VacancyPartyId_ProviderA = 4;
        protected const int FrameworkId_Framework1 = 1;
        protected const int StandardId_Standard1 = 1;
        protected const string VacancyTypeCode_Apprenticeship = "A";
        protected const string VacancyStatusCode_Live = "LIV";
        protected const string VacancyLocationTypeCode_Specific = "S";
        protected const string TrainingTypeCode_Framework = "F";
        protected const string TrainingTypeCode_Standard = "S";
        protected const string LevelCode_Intermediate = "2";
        protected const string WageTypeCode_Custom = "CUS";
        protected const string WageIntervalCode_Weekly = "W";
        protected const string DurationTypeCode_Years = "Y";
        protected const string Ukprn = "Ukrpn provider 1";
        protected const string EmployerErn = "Employer 1 ern";
        protected const string ProviderSiteErn = "Provider 1 site ern";

        protected Vacancy CreateValidDatabaseVacancy()
        {
            var result = new Fixture().Build<Vacancy>()
                .With(v => v.WageTypeCode, WageTypeCode_Custom)
                .With(v => v.WageIntervalCode, WageIntervalCode_Weekly)
                .With(v => v.DurationTypeCode, DurationTypeCode_Years)
                .With(v => v.TrainingTypeCode, TrainingTypeCode_Framework)
                .With(v => v.VacancyStatusCode, VacancyStatusCode_Live)
                .With(v => v.LevelCode, LevelCode_Intermediate)
                .With(v => v.VacancyTypeCode, VacancyTypeCode_Apprenticeship) // TODO: This is cheating the test as not mapped
                .Create();

            if (result.FrameworkId.GetHashCode() % 2 == 1)
            {
                result.TrainingTypeCode = TrainingTypeCode_Framework;
                result.FrameworkId = FrameworkId_Framework1;
                result.StandardId = null;
            }
            else
            {
                result.TrainingTypeCode = TrainingTypeCode_Standard;
                result.FrameworkId = null;
                result.StandardId = StandardId_Standard1;
            }

            return result;
        }

        protected ApprenticeshipVacancy CreateValidDomainVacancy()
        {
            var result = new Fixture().Build<ApprenticeshipVacancy>()
                .With(av => av.Status, ProviderVacancyStatuses.PendingQA)
                .With(av => av.DateSubmitted, null)
                .With(av => av.QAUserName, null)
                .With(av => av.DateStartedToQA, null)
                .Create();

            if (result.FrameworkCodeName != null && result.FrameworkCodeName.GetHashCode() % 2 == 1)
            {
                result.TrainingType = TrainingType.Frameworks;
                result.StandardId = null;
            }
            else
            {
                result.TrainingType = TrainingType.Standards;
                result.FrameworkCodeName = null;
                result.StandardId = StandardId_Standard1;
            }

            return result;
        }

        protected EquivalencyAssertionOptions<Vacancy> ExcludeHardOnes(EquivalencyAssertionOptions<Vacancy> options)
        {
            return options
                // TODO: Not in Domain object yet
                .Excluding(v => v.VacancyLocationTypeCode)
                .Excluding(v => v.AV_ContactName)
                .Excluding(v => v.AV_WageText)

                // TODO: Rather hard / out of scope?
                .Excluding(v => v.ParentVacancyId)
                .Excluding(v => v.EmployerVacancyPartyId)
                .Excluding(v => v.OwnerVacancyPartyId)
                .Excluding(v => v.ManagerVacancyPartyId)
                .Excluding(v => v.DeliveryProviderVacancyPartyId)
                .Excluding(v => v.ContractOwnerVacancyPartyId)
                .Excluding(v => v.OriginalContractOwnerVacancyPartyId)

                // TODO: Might be easier?
                .Excluding(v => v.FrameworkId)
                .Excluding(v => v.StartDate)

                ;
        }

        protected EquivalencyAssertionOptions<ApprenticeshipVacancy> ExcludeHardOnes(EquivalencyAssertionOptions<ApprenticeshipVacancy> options)
        {
            return options
                // TODO: Not in Domain object yet
                .Excluding(v => v.IsEmployerLocationMainApprenticeshipLocation)

                // TODO: Rather hard / out of scope?
                .Excluding(v => v.LocationAddresses)

                // TODO: Might be easier?
                .Excluding(v => v.FrameworkCodeName)
                .Excluding(v => v.Ukprn)
                .Excluding(v => v.ProviderSiteEmployerLink)
                .Excluding(v => v.DateStartedToQA)
                .Excluding(v => v.VacancyManagerId)
                .Excluding(v => v.LastEditedById)
                .Excluding(v => v.ParentVacancyReferenceNumber)
                .Excluding(v => v.DateCreated)
                .Excluding(v => v.DateUpdated)

                ;
        }

    }
}