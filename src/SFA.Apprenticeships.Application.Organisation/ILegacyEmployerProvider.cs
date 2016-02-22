namespace SFA.Apprenticeships.Application.Organisation
{
    using System.Collections.Generic;
    using Domain.Entities.Raa.Parties;

    public interface ILegacyEmployerProvider
    {
        Employer GetEmployer(int employerId);
        Employer GetEmployer(string ern);
        IEnumerable<Employer> GetEmployersByIds(IEnumerable<int> employerIds);
    }
}