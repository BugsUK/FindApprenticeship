namespace SFA.Apprenticeships.Application.Organisation
{
    using Domain.Entities.Organisations;

    /// <summary>
    /// For searching for organisations by ERN, name, location, type, etc. 
    /// </summary>
    public interface IVerifiedOrganisationProvider
    {
        VerifiedOrganisation GetByReferenceNumber(string referenceNumber);
    }
}
