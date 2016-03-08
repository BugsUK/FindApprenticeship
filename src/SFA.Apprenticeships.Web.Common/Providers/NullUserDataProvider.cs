namespace SFA.Apprenticeships.Web.Common.Providers
{
    public class NullUserDataProvider : IUserDataProvider
    {
        public UserContext GetUserContext()
        {
            return new UserContext();
        }

        public void SetUserContext(string userName, string fullName, string acceptedTermsAndConditionsVersion)
        {
            
        }

        public void Clear()
        {
            
        }

        public void Push(string key, string value)
        {
            
        }

        public string Get(string key)
        {
            return "";
        }

        public string Pop(string key)
        {
            return "";
        }
    }
}