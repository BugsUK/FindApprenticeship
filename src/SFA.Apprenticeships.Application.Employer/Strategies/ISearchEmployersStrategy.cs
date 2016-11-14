namespace SFA.Apprenticeships.Application.Employer.Strategies
{
    using System.Collections.Generic;
    using Domain.Entities.Raa.Parties;
    using Domain.Raa.Interfaces.Repositories.Models;
    using Interfaces.Generic;

    public interface ISearchEmployersStrategy
    {
        IEnumerable<Employer> SearchEmployers(EmployerSearchParameters searchParameters);
        Pageable<Employer> GetEmployers(string edsUrn, string name, string location, int currentPage, int pageSize);
    }
}