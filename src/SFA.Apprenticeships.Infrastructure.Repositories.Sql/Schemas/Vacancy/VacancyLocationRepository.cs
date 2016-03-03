namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Vacancy
{
    using System.Collections.Generic;
    using System.Linq;
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
            _logger.Debug("Calling database to get vacancy with locations for vacancy with Id={0}", vacancyId);

            var vacancyLocations =
                _getOpenConnection.Query<Entities.VacancyLocation>("SELECT * FROM dbo.VacancyLocation WHERE VacancyId = @VacancyId",
                    new { VacancyId = vacancyId });

            return
                _mapper.Map<IList<Entities.VacancyLocation>, IList<VacancyLocation>>(vacancyLocations)
                    .ToList();
        }

        public List<VacancyLocation> Save(List<VacancyLocation> locationAddresses)
        {
            var vacanyLocations = _mapper.Map<IList<VacancyLocation>, IList<Entities.VacancyLocation>>(locationAddresses);

            foreach (var vacancyLocation in vacanyLocations)
            {
                _getOpenConnection.Insert(vacancyLocation);
            }

            return locationAddresses;
        }

        public void DeleteFor(int vacancyId)
        {
            var vacancyLocations =
                _getOpenConnection.Query<Entities.VacancyLocation>("SELECT * FROM dbo.VacancyLocation WHERE VacancyId = @VacancyId",
                    new { VacancyId = vacancyId });

            foreach (var vacancyLocation in vacancyLocations)
            {
                _getOpenConnection.DeleteSingle(vacancyLocation);
            }
        }
    }
}