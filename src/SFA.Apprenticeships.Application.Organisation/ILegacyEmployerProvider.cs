namespace SFA.Apprenticeships.Application.Organisation
{
    using System.Collections.Generic;
    using Domain.Entities.Organisations;

    public interface ILegacyEmployerProvider
    {
        Employer GetEmployer(string providerSiteErn, string ern);
        IEnumerable<Employer> GetEmployers(string providerSiteErn);
    }
}