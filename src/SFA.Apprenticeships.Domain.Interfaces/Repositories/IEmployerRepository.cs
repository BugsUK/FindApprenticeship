namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using Entities.Organisations;

    public interface IEmployerReadRepository : IReadRepository<Employer>
    {
        Employer Get(string ern);
    }

    public interface IEmployerWriteRepository : IWriteRepository<Employer> { }
}