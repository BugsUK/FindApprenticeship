namespace SFA.Apprenticeships.Application.Employer.Strategies
{
    using System.Collections.Generic;
    using Domain.Entities.Raa.Parties;

    public interface IGetByIdsStrategy
    {
        IEnumerable<Employer> GetEmployers(IEnumerable<int> employerIds, bool currentOnly = true);
    }
}