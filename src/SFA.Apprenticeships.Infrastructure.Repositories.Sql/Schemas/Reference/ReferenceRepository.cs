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
    using Domain.Entities.Users;
    using Standard = Domain.Entities.Raa.Vacancies.Standard;

    public class ReferenceRepository : IReferenceRepository
    {
        private readonly IGetOpenConnection _getOpenConnection;
        private readonly IMapper _mapper;
        private readonly ILogService _logger;
        private readonly TimeSpan _cacheDuration = TimeSpan.FromHours(1);

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
                    StandardId = x.StandardId,
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

        public Standard CreateStandard(Standard standard)
        {
            //throw new System.NotImplementedException();
            _logger.Info("Creating new Standard");

            var dbStandard = MapStandard(standard);
            PopulateEducationLevelId(standard, dbStandard);

            var standardId = _getOpenConnection.Insert(dbStandard);
            standard.StandardId = (int)standardId;

            return standard;
        }

        private void PopulateEducationLevelId(Standard entity, Entities.Standard dbVacancy)
        {
            dbVacancy.EducationLevelId = _getOpenConnection.QueryCached<int>(_cacheDuration, @"
SELECT EducationLevelId
FROM   [Reference].[EducationLevel]
WHERE  CodeName = @EntityApprenticeshipLevel",
                new
                {
                    EntityApprenticeshipLevel = (int)entity.ApprenticeshipLevel
                }).Single();
        }

        public Sector CreateSector(Sector sector)
        {
            _logger.Info("Creating new Sector");

            var dbSector = MapSector(sector);
            var sectorId = _getOpenConnection.Insert(dbSector);
            sector.SectorId = (int) sectorId;

            return sector;
        }

        public Standard GetStandardById(int standardId)
        {
            _logger.Debug("Getting standard with StandardId={0}", standardId);

            const string sql = "SELECT * FROM reference.Standard WHERE StandardId = @standardId";

            var sqlParams = new
            {
                standardId
            };

            var dbStandard = _getOpenConnection.Query<Entities.Standard>(sql, sqlParams).SingleOrDefault();

            _logger.Debug(dbStandard == null
                ? "Did not find standard with StandardId={0}"
                : "Got standard with StandardId={0}",
                standardId);

            return MapStandard(dbStandard);
        }

        public Sector GetSectorById(int sectorId)
        {
            _logger.Debug("Getting sector with SectorId={0}", sectorId);

            const string sql = "SELECT * FROM Reference.StandardSector WHERE StandardSectorId = @sectorId";

            var sqlParams = new
            {
                sectorId
            };

            var dbSector = _getOpenConnection.Query<Entities.StandardSector>(sql, sqlParams).SingleOrDefault();

            _logger.Debug(dbSector == null
                ? "Did not find sector with SectorId={0}"
                : "Got sector with SectorId={0}",
                sectorId);

            return MapSector(dbSector);
        }

        public Standard UpdateStandard(Standard standard)
        {
            _logger.Debug("Saving standard with Id={0}", standard.StandardId);

            var dbStandard = MapStandard(standard);
            PopulateEducationLevelId(standard, dbStandard);

            // TODO: SQL: AG: note that we are not attempting to insert a new Provider, we always update (temporary).

            if (!_getOpenConnection.UpdateSingle(dbStandard))
            {
                throw new Exception($"Failed to save standard with Id={standard.StandardId}");
            }

            return MapStandard(dbStandard);
        }

        public Sector UpdateSector(Sector sector)
        {
            _logger.Debug("Saving sector with Id={0}", sector.SectorId);

            var dbSector = MapSector(sector);

            if (!_getOpenConnection.UpdateSingle(dbSector))
            {
                throw new Exception($"Failed to save sector with Id={sector.SectorId}");
            }

            return MapSector(dbSector);
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

        private Entities.Standard MapStandard(Standard standard)
        {
            return _mapper.Map<Standard, Entities.Standard>(standard);
        }

        private Standard MapStandard(Entities.Standard standard)
        {
            return standard == null
                ? null
                : _mapper.Map<Entities.Standard, Standard>(standard);
        }

        private Entities.StandardSector MapSector(Sector sector)
        {
            return _mapper.Map<Sector, Entities.StandardSector>(sector);
        }

        private Sector MapSector(Entities.StandardSector sector)
        {
            return sector == null
                ? null
                : _mapper.Map<Entities.StandardSector, Sector>(sector);
        }
    }
}