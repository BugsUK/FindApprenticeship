namespace SFA.Apprenticeships.Application.Organisation
{
    using Domain.Entities.Raa.Parties;

    public interface ILegacyEmployerProvider
    {
        Employer GetEmployer(int employerId);
        Employer GetEmployer(string ern);
    }
}