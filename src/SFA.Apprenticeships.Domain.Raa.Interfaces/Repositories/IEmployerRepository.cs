namespace SFA.Apprenticeships.Domain.Raa.Interfaces.Repositories
{
    using System.Collections.Generic;
    using Entities.Raa.Parties;
    using Models;

    public interface IEmployerReadRepository
    {
        Employer GetById(int employerId, bool currentOnly = true);

        Employer GetByEdsUrn(string edsUrn, bool currentOnly = true);

        List<Employer> GetByIds(IEnumerable<int> employerIds, bool currentOnly = true);

        IEnumerable<Employer> Search(EmployerSearchParameters searchParameters);
    }

    public interface IEmployerWriteRepository
    {
        Employer Save(Employer employer);
    }
}
