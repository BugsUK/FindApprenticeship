namespace SFA.Apprenticeships.Application.Employer.Strategies
{
    using System.Collections.Generic;
    using Domain.Entities.Raa.Parties;
    using Domain.Raa.Interfaces.Repositories;

    public class GetByIdsStrategy : IGetByIdsStrategy
    {
        private readonly IEmployerReadRepository _employerReadRepository;

        public GetByIdsStrategy(IEmployerReadRepository employerReadRepository)
        {
            _employerReadRepository = employerReadRepository;
        }

        public IEnumerable<Employer> GetEmployers(IEnumerable<int> employerIds, bool currentOnly = true)
        {
            return _employerReadRepository.GetByIds(employerIds, currentOnly);
        }
    }
}