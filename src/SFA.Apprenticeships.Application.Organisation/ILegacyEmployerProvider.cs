namespace SFA.Apprenticeships.Application.Organisation
{
    using Domain.Entities.Organisations;

    public interface ILegacyEmployerProvider
    {
        Employer GetEmployer(string ern);
    }
}