namespace SFA.Apprenticeships.Application.Organisation
{
    using System.Collections.Generic;
    using Domain.Entities.Organisations;

    public interface ILegacyEmployerProvider
    {
        IEnumerable<Employer> GetEmployers(string ern);
    }
}