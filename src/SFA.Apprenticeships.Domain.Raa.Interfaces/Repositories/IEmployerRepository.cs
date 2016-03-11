namespace SFA.Apprenticeships.Domain.Raa.Interfaces.Repositories
{
    using System.Collections.Generic;
    using Entities.Raa.Parties;

    public interface IEmployerReadRepository
    {
        Employer Get(int employerId);

        Employer GetByEdsUrn(string edsUrn);

        List<Employer> GetByIds(IEnumerable<int> employerIds);
    }

    public interface IEmployerWriteRepository
    {
        Employer Save(Employer entity);
    }
}