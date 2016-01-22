namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Reference
{
    using System.Collections.Generic;
    using Domain.Entities.Reference;
    using Domain.Interfaces.Repositories;
    using SFA.Infrastructure.Interfaces;
    using Common;

    public class ReferenceRepository : IReferenceRepository
    {
        private readonly IGetOpenConnection _getOpenConnection;
        private readonly IMapper _mapper;
        private readonly ILogService _logger;

        public ReferenceRepository(IGetOpenConnection getOpenConnection, IMapper mapper, ILogService logger)
        {
            _getOpenConnection = getOpenConnection;
            _mapper = mapper;
            _logger = logger;
        }

        public IList<County> GetCounties()
        {
            _logger.Debug("Calling database to get all counties");

            var dbCounties = _getOpenConnection.Query<Entities.County>(@"SELECT * FROM Reference.County WHERE CountyId <> 0 ORDER BY FullName");

            _logger.Debug($"Found {dbCounties.Count} counties");

            var counties = _mapper.Map<IList<Entities.County>, IList<County>>(dbCounties);

            return counties;
        }

        public IList<Region> GetRegions()
        {
            _logger.Debug("Calling database to get all regions");

            var dbRegions = _getOpenConnection.Query<Entities.Region>(@"SELECT * FROM Reference.Region WHERE RegionId <> 0 ORDER BY FullName");

            _logger.Debug($"Found {dbRegions.Count} regions");

            var regions = _mapper.Map<IList<Entities.Region>, IList<Region>>(dbRegions);

            return regions;
        }

        public IList<LocalAuthority> GetLocalAuthorities()
        {
            _logger.Debug("Calling database to get all local authorities");

            var dbLocalAuthorities = _getOpenConnection.Query<Entities.LocalAuthority>(@"SELECT * FROM Reference.LocalAuthority WHERE LocalAuthorityId <> 0 ORDER BY FullName");

            _logger.Debug($"Found {dbLocalAuthorities.Count} local authorities");

            var localAuthorities = _mapper.Map<IList<Entities.LocalAuthority>, IList<LocalAuthority>>(dbLocalAuthorities);

            return localAuthorities;
        }
    }
}