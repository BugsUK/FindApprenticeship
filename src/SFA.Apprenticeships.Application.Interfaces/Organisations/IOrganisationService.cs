namespace SFA.Apprenticeships.Application.Interfaces.Organisations
{
    using System;
    using Domain.Entities.Organisations;

    /// <summary>
    /// For searching for organisations
    /// </summary>
    public interface IOrganisationService
    {
        // inject IVerifiedOrganisationProvider, IProviderDataProvider
        Organisation GetByReferenceNumber(string referenceNumber);
    }
}
