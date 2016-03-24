namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Reference
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Raa.Reference;
    using SFA.Infrastructure.Interfaces;
    using Common;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Raa.Interfaces.Repositories;
    using Entities;
    using Standard = Domain.Entities.Raa.Vacancies.Standard;

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

        //public IList<County> GetCounties()
        //{
        //    _logger.Debug("Calling database to get all counties");

        //    var dbCounties = _getOpenConnection.QueryCached<Entities.County>(TimeSpan.FromHours(1), @"SELECT * FROM dbo.County WHERE CountyId <> 0 ORDER BY FullName");

        //    _logger.Debug($"Found {dbCounties.Count} counties");

        //    var counties = _mapper.Map<IList<Entities.County>, IList<County>>(dbCounties);

        //    return counties;
        //}

        //public IList<Region> GetRegions()
        //{
        //    _logger.Debug("Calling database to get all regions");

        //    var dbRegions = _getOpenConnection.QueryCached<Entities.Region>(TimeSpan.FromHours(1), @"SELECT * FROM Reference.Region ORDER BY RegionId");

        //    _logger.Debug($"Found {dbRegions.Count} regions");

        //    var regions = _mapper.Map<IList<Entities.Region>, IList<Region>>(dbRegions);

        //    return regions;
        //}

        //public IList<LocalAuthority> GetLocalAuthorities()
        //{
        //    _logger.Debug("Calling database to get all local authorities");

        //    const string sql = @"SELECT * FROM dbo.LocalAuthority la JOIN dbo.County c ON la.CountyId = c.CountyId WHERE LocalAuthorityId <> 0 ORDER BY c.CountyId";
        //    var dbLocalAuthorities =
        //        _getOpenConnection.QueryCached<Entities.LocalAuthority, Entities.County, Entities.LocalAuthority>(TimeSpan.FromHours(1),
        //            sql, (localAuthority, county) => { localAuthority.County = county; return localAuthority; }, splitOn: "CountyId");

        //    _logger.Debug($"Found {dbLocalAuthorities.Count} local authorities");

        //    var localAuthorities = _mapper.Map<IList<Entities.LocalAuthority>, IList<LocalAuthority>>(dbLocalAuthorities);

        //    return localAuthorities;
        //}

        public IList<Framework> GetFrameworks()
        {
            _logger.Debug("Getting all frameworks");

            const string frameworkSql = "SELECT * FROM dbo.ApprenticeshipFramework;";

            var sqlParams = new
            {
            };

            var frameworks = _getOpenConnection
                .Query<Entities.ApprenticeshipFramework>(frameworkSql, sqlParams)
                .Select(_mapper.Map<Entities.ApprenticeshipFramework, Framework>).ToList();

            //const string occupationSql = "SELECT * FROM dbo.ApprenticeshipOccupation;";

            //var occupations = _getOpenConnection
            //    .Query<Entities.ApprenticeshipOccupation>(occupationSql, sqlParams)
            //    .Select(_mapper.Map<Entities.ApprenticeshipOccupation, Occupation>).ToList();
            
            _logger.Debug("Got all frameworks");

            return frameworks;
        }

        public IList<Occupation> GetOccupations()
        {
            throw new NotImplementedException();
        }

        public IList<Standard> GetStandards()
        {
            throw new NotImplementedException();
        }

        public IList<Sector> GetSectors()
        {
            throw new NotImplementedException();
        }
    }
}