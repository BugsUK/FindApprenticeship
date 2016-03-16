namespace SFA.Apprenticeships.Application.Employer.Strategies
{
    using Domain.Entities.Raa.Parties;
    using Domain.Raa.Interfaces.Repositories;

    public class SaveEmployerStrategy : ISaveEmployerStrategy
    {
        private readonly IEmployerWriteRepository _employerWriteRepository;

        public SaveEmployerStrategy(IEmployerWriteRepository employerWriteRepository)
        {
            _employerWriteRepository = employerWriteRepository;
        }

        public Employer Save(Employer employer)
        {
            return _employerWriteRepository.Save(employer);
        }
    }
}