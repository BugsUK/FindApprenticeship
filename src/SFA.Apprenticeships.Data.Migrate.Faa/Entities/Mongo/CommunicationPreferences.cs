namespace SFA.Apprenticeships.Data.Migrate.Faa.Entities.Mongo
{
    public class CommunicationPreferences
    {
        public CommunicationPreferences()
        {
            //Need these defaults to match defaults in FAA
            VerifiedMobile = false;

            MarketingPreferences = new CommunicationPreference
            {
                EnableEmail = true,
                EnableText = true
            };
        }

        public bool VerifiedMobile { get; set; }

        public CommunicationPreference MarketingPreferences { get; set; }
    }
}