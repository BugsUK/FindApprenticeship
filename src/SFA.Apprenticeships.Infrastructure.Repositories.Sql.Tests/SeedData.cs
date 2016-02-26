namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Raa.Users;
    using Sql.Schemas.Vacancy.Entities;
    using Schemas.Vacancy;
    using Sql.Schemas.Reference.Entities;
    using AgencyUser = Sql.Schemas.UserProfile.Entities.AgencyUser;
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

        //public static object[] Providers => new object[]
        //{
        //    Provider1
        //};
        
        //public static object[] Vacancies()
        //{
        //    var result = (new object[]
        //        {occupation, occupation2, framework1, framework2, vacancyParty1, vacancyParty2, vacancyParty3})
        //    .Union(GetVacancies()).ToArray();

        //    return result;
        //}

        public static object[] ProviderUsers => new object[]
        {
            ProviderUser1
        };
        
        

        public static object[] AgencyUsers => new object[]
        {
            AgencyUser1
        };
    }
}