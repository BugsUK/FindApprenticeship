using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;

namespace SFA.Apprenticeships.Application.Interfaces.Employers
{
    using Domain.Entities.Raa.Parties;
    using Domain.Raa.Interfaces.Repositories.Models;
    using Generic;
    using System;
    using System.Collections.Generic;

    public interface IEmployerService
    {
        Employer GetEmployer(int employerId, bool currentOnly);
        Employer GetEmployer(string edsUrn);
        IEnumerable<Employer> GetEmployers(IEnumerable<int> employerIds, bool currentOnly = true);
        IEnumerable<Employer> SearchEmployers(EmployerSearchParameters searchParameters);
        Pageable<Employer> GetEmployers(string edsUrn, string name, string location, int currentPage, int pageSize);
        Employer SaveEmployer(Employer employer);
        void SendApplicationLinks(string vacancyTitle, string providerName, IDictionary<string, string> applicationLinks,
            DateTime linkExpiryDateTime, string recipientEmailAddress, string optionalMessage = null);
    }

    public class EmployerSearchRequest
    {
        public EmployerSearchRequest(int providerSiteId)
            : this(providerSiteId, null)
        { }

        public EmployerSearchRequest(int providerSiteId, string employerEdsUrn)
            : this(providerSiteId, employerEdsUrn, null, null)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(employerEdsUrn));
        }

        public EmployerSearchRequest(int providerSiteId, string employerName, string location)
            : this(providerSiteId, null, employerName, location)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(employerName) | !string.IsNullOrWhiteSpace(location));
        }

        private EmployerSearchRequest(int providerSiteId, string employerEdsUrn, string employerName, string location)
        {
            Contract.Requires(providerSiteId != 0);
            EmployerEdsUrn = employerEdsUrn;
            ProviderSiteId = providerSiteId;
            Name = employerName;
            Location = !string.IsNullOrEmpty(location) ? Regex.Replace(location, @"\s+", "") : location;
        }

        private EmployerSearchRequest() { }

        public int ProviderSiteId { get; private set; }

        public string EmployerEdsUrn { get; private set; }

        public string Name { get; private set; }

        public string Location { get; private set; }

        public bool IsEmployerEdsUrnQuery => !string.IsNullOrWhiteSpace(EmployerEdsUrn);

        public bool IsNameQuery => !string.IsNullOrWhiteSpace(Name) && string.IsNullOrWhiteSpace(Location);

        public bool IsNameAndLocationQuery => !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Location);

        public bool IsLocationQuery => !string.IsNullOrWhiteSpace(Location) && string.IsNullOrWhiteSpace(Name);

        public bool IsQuery => IsEmployerEdsUrnQuery || IsNameQuery || IsNameAndLocationQuery || IsLocationQuery;
    }
}