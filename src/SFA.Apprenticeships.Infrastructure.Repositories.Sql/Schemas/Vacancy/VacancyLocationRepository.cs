﻿namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Vacancy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using dbo;
    using Domain.Entities.Raa.Locations;
    using Domain.Raa.Interfaces.Repositories;
    using Application.Interfaces;

    public class VacancyLocationRepository : IVacancyLocationReadRepository, IVacancyLocationWriteRepository
    {
        private readonly IGetOpenConnection _getOpenConnection;
        private readonly IMapper _mapper;
        private readonly ILogService _logger;
        private readonly TimeSpan _cacheDuration = TimeSpan.FromHours(1);

        public VacancyLocationRepository(IGetOpenConnection getOpenConnection, IMapper mapper, ILogService logger)
        {
            _getOpenConnection = getOpenConnection;
            _mapper = mapper;
            _logger = logger;
        }

        public List<VacancyLocation> GetForVacancyId(int vacancyId)
        {
            _logger.Debug("Calling database to get vacancy with locations for vacancy with Id={0}", vacancyId);

            var vacancyLocations =
                _getOpenConnection.Query<Entities.VacancyLocation>("SELECT * FROM dbo.VacancyLocation WHERE VacancyId = @VacancyId ORDER BY VacancyLocationId DESC",
                    new { VacancyId = vacancyId });

            return vacancyLocations.Select(vl =>
            {
                var vacancyLocation = _mapper.Map<Entities.VacancyLocation, VacancyLocation>(vl);
                MapLocalAuthorityCode(vl, vacancyLocation);
                MapCountyId(vl, vacancyLocation);

                return vacancyLocation;
            }).ToList();
        }

        public List<VacancyLocation> GetForVacancyReferenceNumber(int vacancyReferenceNumber)
        {
            _logger.Debug("Calling database to get vacancy with locations for vacancy with reference number={0}", vacancyReferenceNumber);

            var vacancyLocations =
                _getOpenConnection.Query<Entities.VacancyLocation>("SELECT vl.* FROM dbo.Vacancy v JOIN dbo.VacancyLocation vl on v.VacancyId = vl.VacancyId WHERE VacancyReferenceNumber = @vacancyReferenceNumber ORDER BY VacancyLocationId DESC",
                    new { vacancyReferenceNumber });

            return vacancyLocations.Select(vl =>
            {
                var vacancyLocation = _mapper.Map<Entities.VacancyLocation, VacancyLocation>(vl);
                MapLocalAuthorityCode(vl, vacancyLocation);
                MapCountyId(vl, vacancyLocation);

                return vacancyLocation;
            }).ToList();
        }

        public IReadOnlyDictionary<int, IEnumerable<VacancyLocation>> GetVacancyLocationsByVacancyIds(IEnumerable<int> vacancyIds)
        {
            var vacancyLocations = new Dictionary<int, IEnumerable<VacancyLocation>>();

            var splitVacancyIds = DbHelpers.SplitIds(vacancyIds);
            foreach (var vacanciesIds in splitVacancyIds)
            {
                var splitVacancyLocations =
                    _getOpenConnection.Query<VacancyLocation>(
                        @"SELECT * FROM dbo.VacancyLocation WHERE  VacancyId IN @Ids", new {Ids = vacanciesIds})
                        .GroupBy(x => x.VacancyId);
                foreach (var splitVacancyLocation in splitVacancyLocations)
                {
                    vacancyLocations[splitVacancyLocation.Key] = splitVacancyLocation;
                }
            }

            return vacancyLocations;
        }

        public List<VacancyLocation> Create(List<VacancyLocation> locationAddresses)
        {
            foreach (var vacancyLocationAddress in locationAddresses)
            {
                var vacancyLocation = _mapper.Map<VacancyLocation, Entities.VacancyLocation>(vacancyLocationAddress);
                PopulateLocalAuthorityId(vacancyLocationAddress, vacancyLocation);
                PopulateCountyId(vacancyLocationAddress, vacancyLocation);

                _getOpenConnection.Insert(vacancyLocation);
            }

            return locationAddresses;
        }

        public List<VacancyLocation> Update(List<VacancyLocation> locationAddresses)
        {
            foreach (var vacancyLocationAddress in locationAddresses)
            {
                var vacancyLocation = _mapper.Map<VacancyLocation, Entities.VacancyLocation>(vacancyLocationAddress);
                PopulateLocalAuthorityId(vacancyLocationAddress, vacancyLocation);
                PopulateCountyId(vacancyLocationAddress, vacancyLocation);

                _getOpenConnection.UpdateSingle(vacancyLocation);
            }

            return locationAddresses;
        }

        public void DeleteFor(int vacancyId)
        {
            var vacancyLocations =
                _getOpenConnection.Query<Entities.VacancyLocation>("SELECT * FROM dbo.VacancyLocation WHERE VacancyId = @VacancyId ORDER BY VacancyLocationId DESC",
                    new { VacancyId = vacancyId });

            foreach (var vacancyLocation in vacancyLocations)
            {
                _getOpenConnection.DeleteSingle(vacancyLocation);
            }
        }

        private void PopulateLocalAuthorityId(VacancyLocation entity, Entities.VacancyLocation dbVacancyLocation)
        {
            if (!string.IsNullOrWhiteSpace(entity.LocalAuthorityCode))
            {
                dbVacancyLocation.LocalAuthorityId = _getOpenConnection.QueryCached<int>(_cacheDuration, @"
SELECT LocalAuthorityId
FROM   dbo.LocalAuthority
WHERE  CodeName = @LocalAuthorityCode",
                    new
                    {
                        entity.LocalAuthorityCode
                    }).Single();
            }
            else
            {
                dbVacancyLocation.LocalAuthorityId = null;
            }
        }

        private void MapLocalAuthorityCode(Entities.VacancyLocation dbVacancyLocation, VacancyLocation result)
        {
            if (dbVacancyLocation.LocalAuthorityId.HasValue)
            {
                result.LocalAuthorityCode = _getOpenConnection.QueryCached<string>(_cacheDuration, @"
SELECT CodeName
FROM   dbo.LocalAuthority
WHERE  LocalAuthorityId = @LocalAuthorityId",
                    new
                    {
                        dbVacancyLocation.LocalAuthorityId
                    }).Single();
            }
            else
            {
                result.LocalAuthorityCode = null;
            }
        }

        private void PopulateCountyId(VacancyLocation entity, Entities.VacancyLocation dbVacancyLocation)
        {
            if (!string.IsNullOrWhiteSpace(entity.Address?.County))
            {
                dbVacancyLocation.CountyId = _getOpenConnection.QueryCached<int>(_cacheDuration, @"
SELECT CountyId
FROM   dbo.County
WHERE  FullName = @CountyFullName",
                    new
                    {
                        CountyFullName = entity.Address.County
                    }).SingleOrDefault();
            }
        }

        private void MapCountyId(Entities.VacancyLocation dbVacancyLocation, VacancyLocation result)
        {
            // Not all the vacancies have CountyId (before being accepted by QA).
            // A multilocation vacancy (more than one location) doesn't have anything in the address fields.
            if (dbVacancyLocation.CountyId > 0)
            {
                result.Address.County = _getOpenConnection.QueryCached<string>(_cacheDuration, @"
SELECT FullName
FROM   dbo.County
WHERE  CountyId = @CountyId",
                    new
                    {
                        CountyId = dbVacancyLocation.CountyId
                    }).SingleOrDefault();
            }
        }
    }
}