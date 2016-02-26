namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Raa.Users;
    using Sql.Schemas.Vacancy.Entities;
    using Schemas.Vacancy;
    using Sql.Schemas.Reference.Entities;
    using ProviderUser = Sql.Schemas.Provider.Entities.ProviderUser;

    //using Sql.Schemas.Provider.Entities;
    //using AgencyUser = Sql.Schemas.UserProfile.Entities.AgencyUser;
    //using ProviderUser = Sql.Schemas.Provider.Entities.ProviderUser;

    internal class SeedData
    {
        #region Providers

        /*
        public static Provider Provider1 = new Provider()
        {
            ProviderId = 1,
            FullName = "Acme Corp",
            Ukprn = 678,
            IsContracted = false,
            IsNasProvider = false,
            ProviderStatusTypeId = (int)ProviderStatuses.Activated
        };
        */

        #endregion

        #region ProviderUsers
        public static ProviderUser ProviderUser1 = new ProviderUser()
        {
            ProviderUserId = 1,
            ProviderUserGuid = Guid.NewGuid(),
            ProviderUserStatusId = (int)ProviderUserStatus.Registered,
            ProviderId = 1,
            Username = "jane.doe",
            Fullname = "Jane Doe",
            PreferredProviderSiteId = 90392821,
            Email = "jane.doe@example.com",
            EmailVerificationCode = "ABC123",
            EmailVerifiedDateTime = DateTime.UtcNow,
            PhoneNumber = "07999555123",
            CreatedDateTime = DateTime.UtcNow.AddDays(-1),
            UpdatedDateTime = DateTime.UtcNow.AddHours(-10)
        };
        #endregion

        #region AgencyUsers
        public static AgencyUser AgencyUser1 = new AgencyUser() { Username = "userRoleTeam" };
        #endregion

        #region Vacancies

        /*
        public static List<Vacancy> GetVacancies()
        {
            var vacancies = new List<Vacancy>();

            for (int i = 0; i < 11; i++)
            {
                var frameworkId = i%2 == 0 ? TestBase.FrameworkId_Framework1 : TestBase.FrameworkId_Framework2;
                    //5 framework 1, 6 framework 2
                var date = DateTime.Today.AddDays(-i);

                vacancies.Add(new Vacancy
                {
                    VacancyId = 42,
                    VacancyReferenceNumber = null,
                    AV_ContactName = "av contact name",
                    VacancyTypeCode = TestBase.VacancyTypeCode_Apprenticeship,
                    VacancyStatusCode = TestBase.VacancyStatusCode_Live,
                    VacancyLocationTypeCode = TestBase.VacancyLocationTypeCode_Specific,
                    Title = "Test vacancy",
                    TrainingTypeCode = TestBase.TrainingTypeCode_Framework,
                    LevelCode = TestBase.LevelCode_Intermediate,
                    FrameworkId = frameworkId,
                    WageValue = 100.0M,
                    WageTypeCode = TestBase.WageTypeCode_Custom,
                    WageIntervalCode = TestBase.WageIntervalCode_Weekly,
                    ClosingDate = date,
                    PublishedDateTime = date,
                    ContractOwnerVacancyPartyId = 1,
                    DeliveryProviderVacancyPartyId = 1,
                    EmployerVacancyPartyId = 1,
                    ManagerVacancyPartyId = 3,
                    OriginalContractOwnerVacancyPartyId = 1,
                    ParentVacancyId = null,
                    OwnerVacancyPartyId = 1,
                    DurationValue = 3,
                    DurationTypeCode = TestBase.DurationTypeCode_Years
                });
            }

            vacancies.Add(new Vacancy
            {
                VacancyId = TestBase.VacancyId_VacancyAParent,
                VacancyReferenceNumber = null,
                AV_ContactName = "av contact name",
                VacancyTypeCode = TestBase.VacancyTypeCode_Apprenticeship,
                VacancyStatusCode = TestBase.VacancyStatusCode_Parent,
                VacancyLocationTypeCode = TestBase.VacancyLocationTypeCode_Specific,
                Title = "Test vacancy",
                TrainingTypeCode = TestBase.TrainingTypeCode_Framework,
                LevelCode = TestBase.LevelCode_Intermediate,
                FrameworkId = TestBase.FrameworkId_Framework1,
                WageValue = 100.0M,
                WageTypeCode = TestBase.WageTypeCode_Custom,
                WageIntervalCode = TestBase.WageIntervalCode_Weekly,
                ClosingDate = DateTime.Now,
                ContractOwnerVacancyPartyId = 1,
                DeliveryProviderVacancyPartyId = 1,
                EmployerVacancyPartyId = 1,
                ManagerVacancyPartyId = 3,
                OriginalContractOwnerVacancyPartyId = 1,
                ParentVacancyId = null,
                OwnerVacancyPartyId = 1,
                DurationValue = 3,
                DurationTypeCode = TestBase.DurationTypeCode_Years
            });

            vacancies.Add(new Vacancy
            {
                VacancyId = TestBase.VacancyId_VacancyA,
                VacancyReferenceNumber = TestBase.VacancyReferenceNumber_VacancyA,
                AV_ContactName = "av contact name",
                VacancyTypeCode = TestBase.VacancyTypeCode_Apprenticeship,
                VacancyStatusCode = TestBase.VacancyStatusCode_Live,
                VacancyLocationTypeCode = TestBase.VacancyLocationTypeCode_Specific,
                Title = "Test vacancy",
                TrainingTypeCode = TestBase.TrainingTypeCode_Framework,
                LevelCode = TestBase.LevelCode_Intermediate,
                FrameworkId = TestBase.FrameworkId_Framework1,
                WageValue = 100.0M,
                WageTypeCode = TestBase.WageTypeCode_Custom,
                WageIntervalCode = TestBase.WageIntervalCode_Weekly,
                ClosingDate = DateTime.Now,
                ContractOwnerVacancyPartyId = 1,
                DeliveryProviderVacancyPartyId = 1,
                EmployerVacancyPartyId = 1,
                ManagerVacancyPartyId = 3,
                OriginalContractOwnerVacancyPartyId = 1,
                ParentVacancyId = null,
                OwnerVacancyPartyId = 1,
                DurationValue = 3,
                DurationTypeCode = TestBase.DurationTypeCode_Years,
                PublishedDateTime = DateTime.UtcNow.AddDays(-1)
            });

            return vacancies;
        }

        public static Occupation occupation = new Occupation
        {
            OccupationId = 1,
            OccupationStatusId = 1,
            CodeName = "O01",
            FullName = "Occupation 1",
            ShortName = "Occupation 1"
        };

        public static Occupation occupation2 = new Occupation
        {
            OccupationId = 2,
            OccupationStatusId = 1,
            CodeName = "O02",
            FullName = "Occupation 2",
            ShortName = "Occupation 2"
        };

        public static Framework framework1 = new Framework
        {
            FrameworkId = TestBase.FrameworkId_Framework1,
            CodeName = "F01",
            FullName = "Framework 1",
            ShortName = "Framework 1",
            FrameworkStatusId = 1,
            OccupationId = 1
        };

        public static Framework framework2 = new Framework
        {
            FrameworkId = TestBase.FrameworkId_Framework2,
            CodeName = "F02",
            FullName = "Framework 2",
            ShortName = "Framework 2",
            FrameworkStatusId = 1,
            OccupationId = 2
        };

        public static VacancyParty vacancyParty1 = new VacancyParty
        {
            VacancyPartyTypeCode = "ES",
            FullName = "Employer A",
            Description = "A",
            WebsiteUrl = "URL",
            EdsUrn = 1,
            UKPrn = null
        };

        public static VacancyParty vacancyParty2 = new VacancyParty
        {
            VacancyPartyTypeCode = "PS",
            FullName = "Provider A",
            Description = "A",
            WebsiteUrl = "URL",
            EdsUrn = null,
            UKPrn = 1
        };

        public static VacancyParty vacancyParty3 = new VacancyParty
        {
            VacancyPartyTypeCode = "PS",
            FullName = "Provider B",
            Description = "B",
            WebsiteUrl = "URL",
            EdsUrn = 3,
            UKPrn = 1
        };*/

        #endregion

        //public static object[] Providers => new object[]
        //{
        //    Provider1
        //};
        /*
        public static object[] Vacancies()
        {
            var result = (new object[]
                {occupation, occupation2, framework1, framework2, vacancyParty1, vacancyParty2, vacancyParty3})
            .Union(GetVacancies()).ToArray();

            return result;
        }

        public static object[] ProviderUsers => new object[]
        {
            ProviderUser1
        };*/

        //public static object[] AgencyUsers => new object[]
        //{
        //    AgencyUser1
        //};
    }
}