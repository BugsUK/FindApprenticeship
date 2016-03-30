namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Reference
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Remoting.Messaging;

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

            var dbFrameworks = _getOpenConnection
                .Query<Entities.ApprenticeshipFramework>(frameworkSql, sqlParams);

            IList<Occupation> occupations = this.GetOccupations();                      

            var frameworks = dbFrameworks.Select(
                x =>
                    {
                        Framework result = this._mapper.Map<Entities.ApprenticeshipFramework, Framework>(x);
                        result.Occupation =
                            occupations.FirstOrDefault(occu => occu.Id == x.ApprenticeshipOccupationId);
                        return result;
                    }).ToList();

            return frameworks;
        }

        public IList<Occupation> GetOccupations()
        {
            _logger.Debug("Getting all apprenticeship occupations");

            const string sectorSql = "SELECT * FROM dbo.ApprenticeshipOccupation;";

            var sqlParams = new
            {
            };

            var dbOccupations = this._getOpenConnection
                .Query<Entities.ApprenticeshipOccupation>(sectorSql, sqlParams);           

            var occupations = dbOccupations.Select(
                x =>
                    {
                        var occupation = new Occupation
                                             {
                                                 CodeName = x.CodeName,
                                                 Id = x.ApprenticeshipOccupationId,
                                                 FullName = x.FullName,
                                                 ShortName = x.ShortName
                                             };
                        return occupation;
                    }).ToList();

            _logger.Debug("Got all apprenticeship occupations");

            return occupations;
        }

        public IList<Standard> GetStandards()
        {
            _logger.Debug("Getting all standards");

            const string standardSql = "SELECT * FROM Reference.Standard;";

            var sqlParams = new
            {
            };

            var educationLevels = GetEducationLevels();
            
            var dbStandards = _getOpenConnection.Query<Entities.Standard>(standardSql, sqlParams);
            var standards = dbStandards.Select(x =>
            {
                var level = educationLevels.FirstOrDefault(el => el.EducationLevelId == x.EducationLevelId);
                var levelAsInt = int.Parse(level.CodeName);
                var std = new Standard()
                {
                    ApprenticeshipLevel = (ApprenticeshipLevel)levelAsInt,
                    Id = x.StandardId,
                    Name = x.FullName,
                    ApprenticeshipSectorId = x.StandardSectorId
                };
                return std;
            }).ToList();

            return standards;
        }

        public IList<Sector> GetSectors()
        {
            _logger.Debug("Getting all sectors");

            const string sectorSql = "SELECT * FROM Reference.StandardSector;";

            var sqlParams = new
            {
            };

            var dbSectors = _getOpenConnection
                .Query<Entities.StandardSector>(sectorSql, sqlParams);

            //set the standards.
            var standards = GetStandards();

            var sectors = dbSectors.Select(x =>
            {
                var result = _mapper.Map<Entities.StandardSector, Sector>(x);
                result.Standards = standards.Where(std => std.ApprenticeshipSectorId == x.StandardSectorId);
                return result;
            }).ToList();

            _logger.Debug("Got all sectors");

            return sectors ;
        }

        private IList<EducationLevel> GetEducationLevels()
        {
            _logger.Debug("Getting all education levels");

            const string sectorSql = "SELECT * FROM Reference.EducationLevel;";

            var sqlParams = new
            {
            };

            var levels = _getOpenConnection
                .Query<Entities.EducationLevel>(sectorSql, sqlParams);
            
            _logger.Debug("Got all education levels");

            return levels;
        }
    }
}