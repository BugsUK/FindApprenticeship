namespace SFA.Apprenticeships.Application.Employer.Strategies
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Raa.Parties;
    using Interfaces.Generic;
    using Interfaces.Organisations;
    using SFA.Infrastructure.Interfaces;

    public class GetPagedEmployerSearchResultsStrategy : IGetPagedEmployerSearchResultsStrategy
    {
        private readonly IOrganisationService _organisationService;
        private readonly IMapper _mapper;

        public GetPagedEmployerSearchResultsStrategy(IOrganisationService organisationService, IMapper mapper)
        {
            _organisationService = organisationService;
            _mapper = mapper;
        }

        public Pageable<Employer> Get(string edsUrn, string name, string location, int currentPage, int pageSize)
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