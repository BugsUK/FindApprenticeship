namespace SFA.Apprenticeships.Domain.Raa.Interfaces.Repositories
{
    using Entities.Raa.Parties;

    public interface IEmployerReadRepository
    {
        Employer Get(string ern);
    }

    public interface IEmployerWriteRepository
    {
        void Delete(int employerId);

        Employer Save(Employer entity);
    }
}