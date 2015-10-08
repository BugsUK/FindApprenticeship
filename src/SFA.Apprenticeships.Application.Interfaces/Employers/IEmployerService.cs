namespace SFA.Apprenticeships.Application.Interfaces.Employers
{
    using System.Collections.Generic;
    using Domain.Entities.Organisations;

    public interface IEmployerService
    {
        Employer GetEmployer(string providerSiteErn, string ern);
        IEnumerable<Employer> GetEmployers(string providerSiteErn);
    }
}