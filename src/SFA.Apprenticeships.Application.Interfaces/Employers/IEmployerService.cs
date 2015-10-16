using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;

namespace SFA.Apprenticeships.Application.Interfaces.Employers
{
    using System.Collections.Generic;
    using Domain.Entities.Organisations;

    public interface IEmployerService
    {
        Employer GetEmployer(string ern);
        Employer SaveEmployer(Employer employer);
        //TODO: Use the object below once it has been agreed upon
        IEnumerable<Employer> GetEmployers(string ern, string name, string location);
    }
    
    public class EmployerSearchRequest
    {
        public EmployerSearchRequest(string providerSiteErn)
            : this(providerSiteErn, null)
        { }

        public EmployerSearchRequest(string providerSiteErn, string ern)
            : this(providerSiteErn, ern, null, null)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(ern));
        }

        public EmployerSearchRequest(string providerSiteErn, string employerName, string postcode)
            : this(providerSiteErn, null, employerName, postcode)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(employerName) | !string.IsNullOrWhiteSpace(postcode));
        }

        private EmployerSearchRequest(string providerSiteErn, string ern, string employerName, string postcode)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(providerSiteErn));
            EmployerEdsUrn = ern;
            ProviderSiteErn = providerSiteErn;
            Name = employerName;
            Postcode = !string.IsNullOrEmpty(postcode) ? Regex.Replace(postcode, @"\s+", "") : postcode;
        }

        private EmployerSearchRequest() {}

        public string ProviderSiteErn { get; private set; }
        
        public string EmployerEdsUrn { get; private set; }

        public string Name { get; private set; }

        public string Postcode { get; private set; }

        public bool IsEmployerEdsUrnQuery => !string.IsNullOrWhiteSpace(EmployerEdsUrn);

        public bool IsNameQuery => !string.IsNullOrWhiteSpace(Name) && string.IsNullOrWhiteSpace(Postcode);

        public bool IsNameAndPostCodeQuery => !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Postcode);

        public bool IsPostCodeQuery => !string.IsNullOrWhiteSpace(Postcode) && string.IsNullOrWhiteSpace(Name);
    }
}