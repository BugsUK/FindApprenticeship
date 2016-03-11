namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Tests
{
    using System;
    using Domain.Entities.Raa.Users;
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
            ProviderUserId = -1,
            Username = "jane.doe@example.com"
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