namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using System.Collections.Generic;
    using Entities.Organisations;

    public interface IEmployerReadRepository : IReadRepository<Employer>
    {
        IEnumerable<Employer> GetForProviderSite(string ern);
    }

    public interface IEmployerWriteRepository : IWriteRepository<Employer> { }
}