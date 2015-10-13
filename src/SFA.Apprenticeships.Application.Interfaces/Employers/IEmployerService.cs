namespace SFA.Apprenticeships.Application.Interfaces.Employers
{
    using Domain.Entities.Organisations;

    public interface IEmployerService
    {
        Employer GetEmployer(string ern);
        Employer SaveEmployer(Employer employer);
    }
}