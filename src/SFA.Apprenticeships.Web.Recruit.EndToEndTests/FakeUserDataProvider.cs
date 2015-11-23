namespace SFA.Apprenticeships.Web.Recruit.EndToEndTests
{
    using Common.Providers;

    public class FakeUserDataProvider : IUserDataProvider
    {
        public FakeUserDataProvider()
        {
            
        }
        public UserContext GetUserContext()
        {
            return new UserContext
            {
                UserName = "userName",
                FullName = "FullName",
                AcceptedTermsAndConditionsVersion = "1.0"
            };
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
            return "value";
        }

        public string Pop(string key)
        {
            return "value";
        }
    }
}