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
                ReferencedEntityId = 2,
                ReferencedEntityGuid = Guid.NewGuid(),
                ReferencedEntitySurrogateId = 10033670
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
                ReferencedEntityGuid = Guid.NewGuid(),
                ReferencedEntitySurrogateId = 228616654
            };
        }
    }
}