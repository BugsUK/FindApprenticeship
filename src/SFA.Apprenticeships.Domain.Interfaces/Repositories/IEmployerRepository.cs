namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using Entities.Organisations;

    public interface IEmployerReadRepository
    {
        Employer Get(string ern);
    }

    public interface IEmployerWriteRepository
    {
        Employer Save(Employer entity);
    }
}