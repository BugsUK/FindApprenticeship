namespace SFA.Apprenticeships.Web.Recruit.Mediators.Provider
{
    public static class ProviderMediatorCodes
    {
        public static class AddSite
        {
            public const string Ok = "Provider.AddSite.Ok";
            public const string ValidationError = "Provider.AddSite.ValidationError";
            public const string SiteNotFoundByEmployerReferenceNumber = "Provider.AddSite.SiteNotFoundByEmployerReferenceNumber";
            public const string SiteFoundByEmployerReferenceNumber = "Provider.AddSite.SiteFoundByEmployerReferenceNumber";
        }

        public static class Sites
        {
            public const string Ok = "Provider.Sites.Ok";
        }

        public static class UpdateSites
        {
            public const string FailedValidation = "Provider.UpdateSites.FailedValidation";
            public const string NoUserProfile = "Provider.UpdateSites.NoUserProfile";
            public const string Ok = "Provider.UpdateSites.Ok";
        }

        public static class GetSite
        {
            public const string Ok = "Provider.GetSite.Ok";
        }

        public static class UpdateSite
        {
            public const string FailedValidation = "Provider.UpdateSite.FailedValidation";
            public const string Ok = "Provider.UpdateSite.Ok";
        }
    }
}