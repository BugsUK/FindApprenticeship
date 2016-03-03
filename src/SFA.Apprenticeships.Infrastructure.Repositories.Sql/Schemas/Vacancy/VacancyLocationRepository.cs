namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Vacancy
{
    using System.Collections.Generic;
    using Common;
    using Domain.Entities.Raa.Locations;
    using Domain.Raa.Interfaces.Repositories;
    using SFA.Infrastructure.Interfaces;

    public class VacancyLocationRepository : IVacancyLocationReadRepository, IVacancyLocationWriteRepository
    {
        private readonly IGetOpenConnection _getOpenConnection;
        private readonly IMapper _mapper;
        private readonly IDateTimeService _dateTimeService;
        private readonly ILogService _logger;

        public VacancyLocationRepository(IGetOpenConnection getOpenConnection, IMapper mapper, IDateTimeService dateTimeService, ILogService logger)
        {
            _getOpenConnection = getOpenConnection;
            _mapper = mapper;
            _dateTimeService = dateTimeService;
            _logger = logger;
        }

        public List<VacancyLocation> GetForVacancyId(int vacancyId)
        {
            throw new System.NotImplementedException();
        }

        public List<VacancyLocation> Save(List<VacancyLocation> locationAddresses)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteFor(int vacancyId)
        {
            throw new System.NotImplementedException();
        }
    }
}