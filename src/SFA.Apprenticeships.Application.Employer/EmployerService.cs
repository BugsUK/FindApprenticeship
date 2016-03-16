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
        private readonly IGetByIdsStrategy _getByIdsStrategy;
        private readonly IGetByEdsUrnStrategy _getByEdsUrnStrategy;
        private readonly IGetPagedEmployerSearchResultsStrategy _getPagedEmployerSearchResultsStrategy;
        private readonly ISaveEmployerStrategy _saveEmployerStrategy;

        public EmployerService(IGetByIdStrategy getByIdStrategy, IGetByIdsStrategy getByIdsStrategy, IGetByEdsUrnStrategy getByEdsUrnStrategy, IGetPagedEmployerSearchResultsStrategy getPagedEmployerSearchResultsStrategy, ISaveEmployerStrategy saveEmployerStrategy)
        {
            _getByIdStrategy = getByIdStrategy;
            _getByIdsStrategy = getByIdsStrategy;
            _getByEdsUrnStrategy = getByEdsUrnStrategy;
            _getPagedEmployerSearchResultsStrategy = getPagedEmployerSearchResultsStrategy;
            _saveEmployerStrategy = saveEmployerStrategy;
        }

        public Employer GetEmployer(int employerId)
        {
            Condition.Requires(employerId);

            return _getByIdStrategy.Get(employerId);
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
    }
}
