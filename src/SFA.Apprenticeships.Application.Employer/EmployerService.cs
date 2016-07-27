namespace SFA.Apprenticeships.Application.Employer
{
    using System.Collections.Generic;
    using CuttingEdge.Conditions;
    using Domain.Entities.Raa.Parties;
    using Interfaces.Employers;
    using Interfaces.Generic;
    using Strategies;

    public class EmployerService : IEmployerService
    {
        private readonly IGetByIdStrategy _getByIdStrategy;
        //TODO: temporary method. Remove after moving status checks to a higher tier
        private readonly IGetByIdWithoutStatusCheckStrategy _getByIdWithoutStatusCheckStrategy;
        private readonly IGetByIdsStrategy _getByIdsStrategy;
        private readonly IGetByEdsUrnStrategy _getByEdsUrnStrategy;
        private readonly IGetPagedEmployerSearchResultsStrategy _getPagedEmployerSearchResultsStrategy;
        private readonly ISaveEmployerStrategy _saveEmployerStrategy;
        private readonly ISendEmployerLinksStrategy _sendEmployerLinksStrategy;

        public EmployerService(IGetByIdStrategy getByIdStrategy, IGetByIdsStrategy getByIdsStrategy, IGetByEdsUrnStrategy getByEdsUrnStrategy, IGetPagedEmployerSearchResultsStrategy getPagedEmployerSearchResultsStrategy, ISaveEmployerStrategy saveEmployerStrategy, IGetByIdWithoutStatusCheckStrategy getByIdWithoutStatusCheckStrategy, ISendEmployerLinksStrategy sendEmployerLinksStrategy)
        {
            _getByIdStrategy = getByIdStrategy;
            _getByIdsStrategy = getByIdsStrategy;
            _getByEdsUrnStrategy = getByEdsUrnStrategy;
            _getPagedEmployerSearchResultsStrategy = getPagedEmployerSearchResultsStrategy;
            _saveEmployerStrategy = saveEmployerStrategy;
            //TODO: temporary method. Remove after moving status checks to a higher tier
            _getByIdWithoutStatusCheckStrategy = getByIdWithoutStatusCheckStrategy;
            _sendEmployerLinksStrategy = sendEmployerLinksStrategy;
        }

        public Employer GetEmployer(int employerId)
        {
            Condition.Requires(employerId);

            return _getByIdStrategy.Get(employerId);
        }

        //TODO: temporary method. Remove after moving status checks to a higher tier
        public Employer GetEmployerWithoutStatusCheck(int employerId)
        {
            Condition.Requires(employerId);

            return _getByIdWithoutStatusCheckStrategy.Get(employerId);
        }

        public Employer GetEmployer(string edsUrn)
        {
            Condition.Requires(edsUrn).IsNotNullOrEmpty();

            return _getByEdsUrnStrategy.Get(edsUrn);
        }

        public IEnumerable<Employer> GetEmployers(IEnumerable<int> employerIds)
        {
            return _getByIdsStrategy.Get(employerIds);
        }

        public Pageable<Employer> GetEmployers(string edsUrn, string name, string location, int currentPage, int pageSize)
        {
            return _getPagedEmployerSearchResultsStrategy.Get(edsUrn, name, location, currentPage, pageSize);
        }

        public Employer SaveEmployer(Employer employer)
        {
            return _saveEmployerStrategy.Save(employer);
        }

        public void SendApplicationLinks(IDictionary<string, string> applicationLinks, string recipientEmailAddress)
        {
            Condition.Requires(applicationLinks.Count).IsGreaterThan(0);
            Condition.Requires(recipientEmailAddress).IsNotNullOrEmpty();

            _sendEmployerLinksStrategy.Send(applicationLinks, recipientEmailAddress);
        }
    }
}
