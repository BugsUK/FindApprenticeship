namespace SFA.Apprenticeships.Application.Interfaces.Organisations
{
    using Domain.Entities.Organisations;

    public interface IOrganisationService
    {
        Organisation GetByReferenceNumber(string referenceNumber);
    }
}
