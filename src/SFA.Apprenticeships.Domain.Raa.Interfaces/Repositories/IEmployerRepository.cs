namespace SFA.Apprenticeships.Domain.Raa.Interfaces.Repositories
{
    using System.Collections.Generic;
    using Entities.Raa.Parties;

    public interface IEmployerReadRepository
    {
        Employer GetById(int employerId, bool currentOnly = true);

        Employer GetByEdsUrn(string edsUrn, bool currentOnly = true);

        List<Employer> GetByIds(IEnumerable<int> employerIds, bool currentOnly = true);

        IEnumerable<MinimalEmployerDetails> GetMinimalDetailsByIds(IEnumerable<int> employerIds, bool currentOnly = true);
    }

    public interface IEmployerWriteRepository
    {
        Employer Save(Employer employer);
    }
}
