namespace SFA.Apprenticeships.Application.Interfaces.Employers
{
    using Domain.Entities.Organisations;

    public interface IEmployerService
    {
        Employer GetEmployer(string ern);
        Employer SaveEmployer(Employer employer);
    }

    public class EmployerSearchRequest
    {
        public EmployerSearchRequest(string providerSiteErn, string ern = null, string employerName = null, string tradingName = null, string postcode = null)
        {
            ProviderSiteErn = providerSiteErn;
            Ern = ern;
            EmployerName = employerName;
            TradingName = tradingName;
            Postcode = postcode;
        }

        private EmployerSearchRequest() {}

        public string ProviderSiteErn { get; private set; }
        
        public string Ern { get; private set; }

        public string EmployerName { get; private set; }

        public string TradingName { get; private set; }

        public string Postcode { get; private set; }
    }
}