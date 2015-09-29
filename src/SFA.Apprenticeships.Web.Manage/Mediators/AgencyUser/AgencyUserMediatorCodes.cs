namespace SFA.Apprenticeships.Web.Manage.Mediators.AgencyUser
{
    public class AgencyUserMediatorCodes
    {
        public class Authorize
        {
            public const string EmptyUsername = "AgencyUserMediatorCodes.Authorize.EmptyUsername";
            public const string MissingServicePermission = "AgencyUserMediatorCodes.Authorize.MissingServicePermission";
            public const string MissingRoleListClaim = "AgencyUserMediatorCodes.Authorize.MissingRoleListClaim";
            public const string ReturnUrl = "AgencyUserMediatorCodes.Authorize.ReturnUrl";
            public const string Ok = "AgencyUserMediatorCodes.Authorize.Ok";
        }

        public class GetAgencyUser
        {
            public const string Ok = "AgencyUserMediatorCodes.GetAgencyUser.Ok";
        }

        public class SaveAgencyUser
        {
            public const string Ok = "AgencyUserMediatorCodes.SaveAgencyUser.Ok";
        }
    }
}