namespace SFA.DAS.RAA.Api.UnitTests.Factories
{
    using System;
    using Apprenticeships.Domain.Entities.Raa.RaaApi;

    public class RaaApiUserFactory
    {
        public static RaaApiUser GetValidProviderApiUser(Guid primaryApiKey)
        {
            return new RaaApiUser
            {
                PrimaryApiKey = primaryApiKey,
                SecondaryApiKey = Guid.NewGuid(),
                UserType = RaaApiUserType.Provider,
                ReferencedEntityId = 1170,
                ReferencedEntityGuid = null,
                ReferencedEntitySurrogateId = 10033670 //Skills funding agency
            };
        }

        public static RaaApiUser GetValidEmployerApiUser(Guid primaryApiKey)
        {
            return new RaaApiUser
            {
                PrimaryApiKey = primaryApiKey,
                SecondaryApiKey = Guid.NewGuid(),
                UserType = RaaApiUserType.Employer,
                ReferencedEntityId = 3,
                ReferencedEntityGuid = null,
                ReferencedEntitySurrogateId = 228616654
            };
        }
    }
}