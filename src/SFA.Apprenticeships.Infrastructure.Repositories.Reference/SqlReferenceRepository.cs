namespace SFA.Apprenticeships.Infrastructure.Repositories.Reference
{
    using System.Collections.Generic;
    using Domain.Entities.Reference;
    using Domain.Interfaces.Repositories;
    using SFA.Infrastructure.Interfaces;
    using SFA.Infrastructure.Sql;

    public class SqlReferenceRepository : IReferenceRepository
    {
        private readonly IGetOpenConnection _getOpenConnection;
        private readonly IMapper _mapper;
        private readonly ILogService _logger;

        public SqlReferenceRepository(IGetOpenConnection getOpenConnection, IMapper mapper, ILogService logger)
        {
            _getOpenConnection = getOpenConnection;
            _mapper = mapper;
            _logger = logger;
        }

        public IList<County> GetCounties()
        {
            _logger.Debug("Calling database to get all counties");

            var dbCounties = _getOpenConnection.Query<NewDB.Domain.Entities.Reference.County>(@"SELECT * FROM Reference.Counties ORDER BY FullName");

            _logger.Debug($"Found {dbCounties.Count} counties");

            var counties = _mapper.Map<IList<NewDB.Domain.Entities.Reference.County>, IList<County>>(dbCounties);

            return counties;
        }
    }
}