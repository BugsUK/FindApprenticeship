namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using System;
    using Entities.Organisations;

    public interface IEmployerReadRepository : IReadRepository<Employer, Guid>
    {
        Employer Get(string ern);
    }

    public interface IEmployerWriteRepository : IWriteRepository<Employer, Guid> { }
}