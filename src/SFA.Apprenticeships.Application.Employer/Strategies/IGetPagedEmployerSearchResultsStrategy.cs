namespace SFA.Apprenticeships.Application.Employer.Strategies
{
    using Domain.Entities.Raa.Parties;
    using Interfaces.Generic;

    public interface IGetPagedEmployerSearchResultsStrategy
    {
        Pageable<Employer> Get(string edsUrn, string name, string location, int currentPage, int pageSize);
    }
}