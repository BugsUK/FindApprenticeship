namespace SFA.DAS.RAA.Api.UnitTests.Factories
{
    using System;
    using Apprenticeships.Domain.Entities.Raa.RaaApi;

    public class RaaApiUserFactory
    {
        public const int SkillsFundingAgencyProviderId = 1170;
        public const int SkillsFundingAgencyUkprn = 10033670;

        public static RaaApiUser GetValidProviderApiUser(Guid primaryApiKey)
        {
            return new RaaApiUser
            {
                PrimaryApiKey = primaryApiKey,
                SecondaryApiKey = Guid.NewGuid(),
                UserType = RaaApiUserType.Provider,
                ReferencedEntityId = SkillsFundingAgencyProviderId,
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