namespace SFA.Apprenticeships.Application.Employer.Strategies
{
    using System.Collections.Generic;
    using Domain.Entities.Raa.Parties;

    public interface IGetByIdsStrategy
    {
        IEnumerable<Employer> Get(IEnumerable<int> employerIds, bool currentOnly = true);

        IEnumerable<MinimalEmployerDetails> GetMinimalDetails(IEnumerable<int> employerIds, bool currentOnly = true);
    }
}