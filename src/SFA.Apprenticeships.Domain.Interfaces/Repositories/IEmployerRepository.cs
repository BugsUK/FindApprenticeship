namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using System.Collections.Generic;
    using Entities.Organisations;

    public interface IEmployerReadRepository : IReadRepository<Employer>
    {
        Employer Get(string providerSiteErn, string ern);
        IEnumerable<Employer> GetForProviderSite(string providerSiteErn);
    }

    public interface IEmployerWriteRepository : IWriteRepository<Employer> { }
}