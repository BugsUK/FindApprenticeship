namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.dbo
{
    using System;
    using Domain.Entities.Organisations;
    using Domain.Interfaces.Repositories;
    public class EmployerRepository : IEmployerReadRepository, IEmployerWriteRepository
    {
        public Employer Get(string ern)
        {
            throw new NotImplementedException();
        }

        public Employer Save(Employer entity)
        {
            throw new NotImplementedException();
        }
    }
}
