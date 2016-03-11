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
        #region ProviderUsers

        public static ProviderUser ProviderUser1 = new ProviderUser()
        {
            ProviderUserId = -1,
            Username = "jane.doe@example.com"
        };

        #endregion

        #region AgencyUsers

        public static AgencyUser AgencyUser1 = new AgencyUser()
        {
            Username = "jane.agency@sfa.bis.gov.uk"
        };

        #endregion
    }
}
