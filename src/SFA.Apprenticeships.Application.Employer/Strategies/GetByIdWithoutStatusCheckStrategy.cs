namespace SFA.Apprenticeships.Application.Employer.Strategies
{
    using Domain.Entities.Raa.Parties;
    using Domain.Raa.Interfaces.Repositories;
    using Application.Interfaces;


    //TODO: temporary clas. Remove after moving status checks to a higher tier
    public class GetByIdWithoutStatusCheckStrategy : IGetByIdWithoutStatusCheckStrategy
    {
        private readonly IEmployerReadRepository _employerReadRepository;
        private readonly ILogService _logService;

        public GetByIdWithoutStatusCheckStrategy(IEmployerReadRepository employerReadRepository, ILogService logService)
        {
            _employerReadRepository = employerReadRepository;
            _logService = logService;
        }


        public Employer Get(int employerId)
        {
            _logService.Debug("Calling Employer Repository to get employer with Id='{0}'.", employerId);

            return _employerReadRepository.GetById(employerId, false);
        }
    }
}