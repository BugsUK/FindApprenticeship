namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Reference
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Raa.Reference;
    using Common;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Raa.Interfaces.Repositories;
    using Entities;
    using Application.Interfaces;
    using Domain.Entities.ReferenceData;
    using County = Domain.Entities.Raa.Reference.County;
    using LocalAuthority = Domain.Entities.Raa.Reference.LocalAuthority;
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
        public IList<Framework> GetFrameworks()
        {
            var dbFrameworks = GetApprenticeshipFrameworks();
            var frameworks = dbFrameworks.Select(_mapper.Map<ApprenticeshipFramework, Framework>).ToList();
            return frameworks;
        }

        public IList<Occupation> GetOccupations()
        {
            var dbOccupations = GetApprenticeshipOccupations();
            var dbFrameworks = GetApprenticeshipFrameworks();

            var occupations = dbOccupations.Select(
                x =>
                {
                    return new Occupation
                    {
                        CodeName = x.CodeName,
                        Id = x.ApprenticeshipOccupationId,
                        FullName = x.FullName,
                        ShortName = x.ShortName,
                        Status = (OccupationStatusType)x.ApprenticeshipOccupationStatusTypeId,
                        Frameworks =
                            dbFrameworks.Where(af => af.ApprenticeshipOccupationId == x.ApprenticeshipOccupationId)
                                .Select(_mapper.Map<ApprenticeshipFramework, Framework>).ToList()
                    };
                }).ToList();

            foreach (var occupation in occupations)
            {
                foreach (var framework in occupation.Frameworks)
                {
                    framework.ParentCategoryCodeName = occupation.CodeName;
                }
            }

            _logger.Debug("Got all apprenticeship occupations");

            return occupations;
        }

        public IList<Standard> GetStandards()
        {
            _logger.Debug("Getting all standards");

            const string standardSql = "SELECT * FROM Reference.Standard ORDER BY FullName;";

            //TODO: Does this need to be here? If not, test and remove.
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
                    ApprenticeshipSectorId = x.StandardSectorId,
                    Status = (FrameworkStatusType)x.ApprenticeshipFrameworkStatusTypeId
                };
                return std;
            }).ToList();

            return standards;
        }

        public IList<Sector> GetSectors()
        {
            _logger.Debug("Getting all sectors");

            const string sectorSql = "SELECT * FROM Reference.StandardSector ORDER BY FullName;";

            var sqlParams = new
            {
            };

            var dbSectors = _getOpenConnection
                .Query<StandardSector>(sectorSql, sqlParams);

            //set the standards.
            var standards = GetStandards();

            var sectors = dbSectors.Select(x =>
            {
                var result = _mapper.Map<StandardSector, Sector>(x);
                result.Standards = standards.Where(std => std.ApprenticeshipSectorId == x.StandardSectorId);
                return result;
            }).ToList();

            _logger.Debug("Got all sectors");

            return sectors ;
        }

        public IList<ReleaseNote> GetReleaseNotes()
        {
            _logger.Debug("Getting all release notes");

            var releaseNotes = _getOpenConnection.Query<ReleaseNote>("SELECT * FROM Reference.ReleaseNote ORDER BY Version");

            _logger.Debug($"Got {releaseNotes.Count} release notes");

            return releaseNotes;
        }

        public IList<StandardSubjectAreaTierOne> GetStandardSubjectAreaTierOnes()
        {
            _logger.Debug("Getting all SSAT1s");

            const string sectorSql = "SELECT * FROM dbo.ApprenticeshipOccupation;";

            var sqlParams = new
            {
            };

            var dbStandardSubjectAreaTierOnes = _getOpenConnection
                .Query<ApprenticeshipOccupation>(sectorSql, sqlParams);

            var sector = GetSectors();

            var standardSubjectAreaTierOnes = dbStandardSubjectAreaTierOnes.Select(x =>
            {
                var appOcc = new StandardSubjectAreaTierOne
                {
                    Id = x.ApprenticeshipOccupationId,
                    Name = x.FullName,
                    Sectors = sector.Where(s => s.ApprenticeshipOccupationId == x.ApprenticeshipOccupationId),
                };
                return appOcc;
            }).ToList();

            _logger.Debug("Got all SSAT1s");

            return standardSubjectAreaTierOnes;
        }

        public IEnumerable<County> GetCounties()
        {
            _logger.Debug("Getting all counties");

            const string sectorSql = "SELECT * FROM dbo.County ORDER BY FullName";

            var counties = _getOpenConnection.Query<County>(sectorSql);

            _logger.Debug($"Got {counties.Count} counties");

            return counties;
        }

        public County GetCounty(int countyId)
        {
            _logger.Debug($"Getting county with id {countyId}");

            const string sectorSql = "SELECT * FROM dbo.County WHERE CountyId = @CountyId";

            var sqlParams = new
            {
                countyId
            };

            var county = _getOpenConnection.Query<County>(sectorSql, sqlParams).FirstOrDefault();

            _logger.Debug($"Found {county}");

            return county;
        }

        public County GetCounty(string countyName)
        {
            _logger.Debug($"Getting county with name {countyName}");

            const string sectorSql = "SELECT * FROM dbo.County WHERE FullName = @CountyName";

            var sqlParams = new
            {
                countyName
            };

            var county = _getOpenConnection.Query<County>(sectorSql, sqlParams).FirstOrDefault();

            _logger.Debug($"Found {county}");

            return county;
        }

        public IEnumerable<LocalAuthority> GetLocalAuthorities()
        {
            _logger.Debug("Getting all local authorities");

            const string sectorSql = "SELECT la.*, c.CodeName, c.ShortName, c.FullName FROM dbo.LocalAuthority la JOIN dbo.County c ON la.CountyId = c.CountyId";

            var localAuthorities = _getOpenConnection.Query<LocalAuthority, County, LocalAuthority>(sectorSql, (la, c) =>
            {
                la.County = c;
                return la;
            }, splitOn: "CountyId");

            _logger.Debug($"Got {localAuthorities.Count} local authorities");

            return localAuthorities;
        }

        public LocalAuthority GetLocalAuthority(int localAuthorityId)
        {
            _logger.Debug($"Getting local authority with id {localAuthorityId}");

            const string sectorSql = "SELECT la.*, c.CodeName, c.ShortName, c.FullName FROM dbo.LocalAuthority la JOIN dbo.County c ON la.CountyId = c.CountyId WHERE la.LocalAuthorityId = @LocalAuthorityId";

            var sqlParams = new
            {
                localAuthorityId
            };

            var localAuthority = _getOpenConnection.Query<LocalAuthority, County, LocalAuthority>(sectorSql, (la, c) =>
            {
                la.County = c;
                return la;
            }, sqlParams, "CountyId").FirstOrDefault();

            _logger.Debug($"Found {localAuthority}");

            return localAuthority;
        }

        public LocalAuthority GetLocalAuthority(string localAuthorityCodeName)
        {
            _logger.Debug($"Getting local authority with code name {localAuthorityCodeName}");

            const string sectorSql = "SELECT la.*, c.CodeName, c.ShortName, c.FullName FROM dbo.LocalAuthority la JOIN dbo.County c ON la.CountyId = c.CountyId WHERE la.CodeName = @LocalAuthorityCodeName";

            var sqlParams = new
            {
                localAuthorityCodeName
            };

            var localAuthority = _getOpenConnection.Query<LocalAuthority, County, LocalAuthority>(sectorSql, (la, c) =>
            {
                la.County = c;
                return la;
            }, sqlParams, "CountyId").FirstOrDefault();

            _logger.Debug($"Found {localAuthority}");

            return localAuthority;
        }

        private IList<ApprenticeshipOccupation> GetApprenticeshipOccupations()
        {
            _logger.Debug("Getting all apprenticeship occupations");

            const string sectorSql = "SELECT * FROM dbo.ApprenticeshipOccupation ORDER BY FullName;";

            var sqlParams = new
            {
            };

            var dbOccupations = _getOpenConnection
                .Query<ApprenticeshipOccupation>(sectorSql, sqlParams);

            _logger.Debug("Got all apprenticeship occupations");

            return dbOccupations;
        } 

        private IList<ApprenticeshipFramework> GetApprenticeshipFrameworks()
        {
            _logger.Debug("Getting all frameworks");

            const string frameworkSql = "SELECT * FROM dbo.ApprenticeshipFramework ORDER BY FullName;";

            var sqlParams = new
            {
            };

            var dbFrameworks = _getOpenConnection
                .Query<ApprenticeshipFramework>(frameworkSql, sqlParams);

            return dbFrameworks;
        } 

        private IList<EducationLevel> GetEducationLevels()
        {
            _logger.Debug("Getting all education levels");

            const string sectorSql = "SELECT * FROM Reference.EducationLevel;";

            var sqlParams = new
            {
            };

            var levels = _getOpenConnection
                .Query<EducationLevel>(sectorSql, sqlParams);
            
            _logger.Debug("Got all education levels");

            return levels;
        }

        public void UpdateStandard(Standard standard)
        {
            _logger.Debug($"Updating standard with id={standard.Id}");

            const string standardSql = "SELECT * FROM Reference.Standard WHERE StandardId = @Id";

            //TODO: Does this need to be here? If not, test and remove.
            var sqlParams = new
            {
                standard.Id
            };

            var dbStandards = _getOpenConnection.Query<Entities.Standard>(standardSql, sqlParams);

            var dbStandard = dbStandards.Single();

            dbStandard.ApprenticeshipFrameworkStatusTypeId = (int)standard.Status;

            var result = _getOpenConnection.UpdateSingle(dbStandard);

            if (!result)
                throw new Exception($"Failed to save standard with id={standard.Id}");
        }
    }
}