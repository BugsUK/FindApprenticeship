namespace SFA.Apprenticeships.Application.Employer
{
    using CuttingEdge.Conditions;
    using Domain.Entities.Raa.Parties;
    using Domain.Raa.Interfaces.Repositories.Models;
    using Interfaces.Employers;
    using Interfaces.Generic;
    using Strategies;
    using System;
    using System.Collections.Generic;

    public class EmployerService : IEmployerService
    {
        private readonly IGetByIdStrategy _getByIdStrategy;
        private readonly IGetByIdsStrategy _getByIdsStrategy;
        private readonly IGetByEdsUrnStrategy _getByEdsUrnStrategy;
        private readonly ISearchEmployersStrategy _searchEmployersStrategy;
        private readonly ISaveEmployerStrategy _saveEmployerStrategy;
        private readonly ISendEmployerLinksStrategy _sendEmployerLinksStrategy;

        public EmployerService(IGetByIdStrategy getByIdStrategy, IGetByIdsStrategy getByIdsStrategy, IGetByEdsUrnStrategy getByEdsUrnStrategy, ISearchEmployersStrategy searchEmployersStrategy, ISaveEmployerStrategy saveEmployerStrategy, ISendEmployerLinksStrategy sendEmployerLinksStrategy)
        {
            _getByIdStrategy = getByIdStrategy;
            _getByIdsStrategy = getByIdsStrategy;
            _getByEdsUrnStrategy = getByEdsUrnStrategy;
            _searchEmployersStrategy = searchEmployersStrategy;
            _saveEmployerStrategy = saveEmployerStrategy;
            //TODO: temporary method. Remove after moving status checks to a higher tier
            _sendEmployerLinksStrategy = sendEmployerLinksStrategy;
        }

        public Employer GetEmployer(int employerId, bool currentOnly)
        {
            Condition.Requires(employerId);

            return _getByIdStrategy.Get(employerId, currentOnly);
        }

        public Employer GetEmployer(string edsUrn)
        {
            Condition.Requires(edsUrn).IsNotNullOrEmpty();

            return _getByEdsUrnStrategy.Get(edsUrn);
        }

        public IEnumerable<Employer> GetEmployers(IEnumerable<int> employerIds, bool currentOnly = true)
        {
            return _getByIdsStrategy.GetEmployers(employerIds, currentOnly);
        }

        public IEnumerable<Employer> SearchEmployers(EmployerSearchParameters searchParameters)
        {
            return _searchEmployersStrategy.SearchEmployers(searchParameters);
        }

        public Pageable<Employer> GetEmployers(string edsUrn, string name, string location, int currentPage, int pageSize)
        {
            return _searchEmployersStrategy.GetEmployers(edsUrn, name, location, currentPage, pageSize);
        }

        public Employer SaveEmployer(Employer employer)
        {
            return _saveEmployerStrategy.Save(employer);
        }

        public void SendApplicationLinks(string vacancyTitle, string providerName,
            IDictionary<string, string> applicationLinks, DateTime linkExpiryDateTime,
            string recipientEmailAddress, string optionalMessage = null)
        {
            Condition.Requires(vacancyTitle).IsNotNullOrEmpty();
            Condition.Requires(providerName).IsNotNullOrEmpty();
            Condition.Requires(applicationLinks.Count).IsGreaterThan(0);
            Condition.Requires(recipientEmailAddress).IsNotNullOrEmpty();

            _sendEmployerLinksStrategy.Send(vacancyTitle, providerName, applicationLinks, linkExpiryDateTime,
                recipientEmailAddress, optionalMessage);
        }
    }
}
