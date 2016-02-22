namespace SFA.Apprenticeships.Domain.Raa.Interfaces.Repositories
{
    using System.Collections.Generic;
    using Entities.Raa.Parties;

    public interface IEmployerReadRepository
    {
        Employer Get(int employerId);

        Employer GetByEdsErn(string edsErn);

        List<Employer> GetByIds(IEnumerable<int> employerIds);
    }

    public interface IEmployerWriteRepository
    {
        void Delete(int employerId);

        Employer Save(Employer entity);
    }
}