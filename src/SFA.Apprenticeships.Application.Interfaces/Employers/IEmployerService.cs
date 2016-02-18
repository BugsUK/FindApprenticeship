using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;

namespace SFA.Apprenticeships.Application.Interfaces.Employers
{
    using System.Collections.Generic;
    using Domain.Entities.Raa.Parties;
    using Generic;

    public interface IEmployerService
    {
        Employer GetEmployer(string edsErn);
        Employer SaveEmployer(Employer employer);
        //TODO: Use the object below once it has been agreed upon
        IEnumerable<Employer> GetEmployers(string edsErn, string name, string location);
        Pageable<Employer> GetEmployers(string edsErn, string name, string location, int currentPage, int pageSize);
    }
    
    public class EmployerSearchRequest
    {
        public EmployerSearchRequest(string providerSiteEdsErn)
            : this(providerSiteEdsErn, null)
        { }

        public EmployerSearchRequest(string providerSiteEdsErn, string ern)
            : this(providerSiteEdsErn, ern, null, null)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(ern));
        }

        public EmployerSearchRequest(string providerSiteEdsErn, string employerName, string location)
            : this(providerSiteEdsErn, null, employerName, location)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(employerName) | !string.IsNullOrWhiteSpace(location));
        }

        private EmployerSearchRequest(string providerSiteEdsErn, string ern, string employerName, string location)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(providerSiteEdsErn));
            EmployerEdsUrn = ern;
            ProviderSiteEdsErn = providerSiteEdsErn;
            Name = employerName;
            Location = !string.IsNullOrEmpty(location) ? Regex.Replace(location, @"\s+", "") : location;
        }

        private EmployerSearchRequest() {}

        public string ProviderSiteEdsErn { get; private set; }
        
        public string EmployerEdsUrn { get; private set; }

        public string Name { get; private set; }

        public string Location { get; private set; }

        public bool IsEmployerEdsUrnQuery => !string.IsNullOrWhiteSpace(EmployerEdsUrn);

        public bool IsNameQuery => !string.IsNullOrWhiteSpace(Name) && string.IsNullOrWhiteSpace(Location);

        public bool IsNameAndLocationQuery => !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Location);

        public bool IsLocationQuery => !string.IsNullOrWhiteSpace(Location) && string.IsNullOrWhiteSpace(Name);
    }
}