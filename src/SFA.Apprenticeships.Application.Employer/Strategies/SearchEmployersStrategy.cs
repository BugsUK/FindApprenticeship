namespace SFA.Apprenticeships.Application.Employer.Strategies
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Raa.Parties;
    using Domain.Raa.Interfaces.Repositories;
    using Domain.Raa.Interfaces.Repositories.Models;
    using Interfaces.Generic;
    using Interfaces.Organisations;
    using Interfaces;

    public class SearchEmployersStrategy : ISearchEmployersStrategy
    {
        private readonly IEmployerReadRepository _employerReadRepository;
        private readonly IOrganisationService _organisationService;
        private readonly IMapper _mapper;

        public SearchEmployersStrategy(IEmployerReadRepository employerReadRepository, IOrganisationService organisationService, IMapper mapper)
        {
            _employerReadRepository = employerReadRepository;
            _organisationService = organisationService;
            _mapper = mapper;
        }

        public IEnumerable<Employer> SearchEmployers(EmployerSearchParameters searchParameters)
        {
            return _employerReadRepository.Search(searchParameters);
        }

        public Pageable<Employer> GetEmployers(string edsUrn, string name, string location, int currentPage, int pageSize)
        {
            var pageable = new Pageable<Employer>
            {
                CurrentPage = currentPage
            };

            int resultCount;
            var verifiedOrganisationSummaries = _organisationService.GetVerifiedOrganisationSummaries(edsUrn, name, location, out resultCount);
            pageable.Page = _mapper.Map<IEnumerable<VerifiedOrganisationSummary>, IEnumerable<Employer>>(verifiedOrganisationSummaries.Skip((currentPage - 1) * pageSize).Take(pageSize)).ToList();
            pageable.ResultsCount = resultCount;
            pageable.TotalNumberOfPages = resultCount / pageSize + 1;
            return pageable;
        }
    }
}