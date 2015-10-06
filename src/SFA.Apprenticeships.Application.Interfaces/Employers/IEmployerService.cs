namespace SFA.Apprenticeships.Application.Interfaces.Employers
{
    using System.Collections.Generic;
    using Domain.Entities.Organisations;

    public interface IEmployerService
    {
        IEnumerable<Employer> GetEmployers(string ern);
    }
}