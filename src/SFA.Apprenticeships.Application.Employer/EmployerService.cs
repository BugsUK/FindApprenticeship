namespace SFA.Apprenticeships.Application.Employer
{
    using System;
    using System.Collections.Generic;
    using CuttingEdge.Conditions;
    using Domain.Entities.Raa.Parties;
    using Interfaces.Employers;
    using Interfaces.Generic;
    using Strategies;

    public class EmployerService : IEmployerService
    {
        private readonly IGetByIdStrategy _getByIdStrategy;
        private readonly IGetByIdsStrategy _getByIdsStrategy;
        private readonly IGetByEdsUrnStrategy _getByEdsUrnStrategy;
        private readonly IGetPagedEmployerSearchResultsStrategy _getPagedEmployerSearchResultsStrategy;
        private readonly ISaveEmployerStrategy _saveEmployerStrategy;
        private readonly ISendEmployerLinksStrategy _sendEmployerLinksStrategy;

        public EmployerService(IGetByIdStrategy getByIdStrategy, IGetByIdsStrategy getByIdsStrategy, IGetByEdsUrnStrategy getByEdsUrnStrategy, IGetPagedEmployerSearchResultsStrategy getPagedEmployerSearchResultsStrategy, ISaveEmployerStrategy saveEmployerStrategy, ISendEmployerLinksStrategy sendEmployerLinksStrategy)
        {
            _getByIdStrategy = getByIdStrategy;
            _getByIdsStrategy = getByIdsStrategy;
            _getByEdsUrnStrategy = getByEdsUrnStrategy;
            _getPagedEmployerSearchResultsStrategy = getPagedEmployerSearchResultsStrategy;
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
            return _getByIdsStrategy.Get(employerIds, currentOnly);
        }

        public IEnumerable<MinimalEmployerDetails> GetMinimalEmployerDetails(IEnumerable<int> employerIds, bool currentOnly = true)
        {
            return _getByIdsStrategy.GetMinimalDetails(employerIds, currentOnly);
        }

        public Pageable<Employer> GetEmployers(string edsUrn, string name, string location, int currentPage, int pageSize)
        {
            return _getPagedEmployerSearchResultsStrategy.Get(edsUrn, name, location, currentPage, pageSize);
        }

        public Employer SaveEmployer(Employer employer)
        {
            return _saveEmployerStrategy.Save(employer);
        }

        public void SendApplicationLinks(string vacancyTitle, string providerName, IDictionary<string, string> applicationLinks, DateTime linkExpiryDateTime, string recipientEmailAddress)
        {
            Condition.Requires(vacancyTitle).IsNotNullOrEmpty();
            Condition.Requires(providerName).IsNotNullOrEmpty();
            Condition.Requires(applicationLinks.Count).IsGreaterThan(0);
            Condition.Requires(recipientEmailAddress).IsNotNullOrEmpty();

            _sendEmployerLinksStrategy.Send(vacancyTitle, providerName, applicationLinks, linkExpiryDateTime, recipientEmailAddress);
        }
    }
}
