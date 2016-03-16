namespace SFA.Apprenticeships.Application.Employer.Strategies
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Raa.Parties;
    using Domain.Raa.Interfaces.Repositories;

    public class GetByIdsStrategy : IGetByIdsStrategy
    {
        private readonly IEmployerReadRepository _employerReadRepository;

        public GetByIdsStrategy(IEmployerReadRepository employerReadRepository)
        {
            _employerReadRepository = employerReadRepository;
        }

        public IEnumerable<Employer> Get(IEnumerable<int> employerIds)
        {
            var ids = employerIds.ToList();
            return _employerReadRepository.GetByIds(ids);
        }
    }
}