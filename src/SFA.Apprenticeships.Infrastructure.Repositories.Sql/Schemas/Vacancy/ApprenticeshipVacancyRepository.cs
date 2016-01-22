namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Vacancy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Domain.Entities.Locations;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using Domain.Interfaces.Queries;
    using Domain.Interfaces.Repositories;
    using Common;
    using Reference.Entities;
    using Entities;
    using SFA.Infrastructure.Interfaces;

    // TODO GenericSqlClient??
    public class ApprenticeshipVacancyRepository : IApprenticeshipVacancyReadRepository, IApprenticeshipVacancyWriteRepository
    {
        private IDictionary<string, Domain.Entities.Vacancies.ProviderVacancies.WageType> _wageTypeMap = new Dictionary<string, Domain.Entities.Vacancies.ProviderVacancies.WageType>();

        private readonly IMapper _mapper;
        private readonly ILogService _logger;

        private readonly IGetOpenConnection _getOpenConnection;

        public ApprenticeshipVacancyRepository(IGetOpenConnection getOpenConnection, IMapper mapper, ILogService logger)
        {
            _getOpenConnection = getOpenConnection;
            _mapper = mapper;
            _logger = logger;
        }

        public ApprenticeshipVacancy Get(Guid id)
        {
            _logger.Debug("Calling database to get apprenticeship vacancy with Id={0}", id);

            var dbVacancy = _getOpenConnection.Query<Repositories.Sql.Schemas.Vacancy.Entities.Vacancy>("SELECT * FROM Vacancy.Vacancy WHERE VacancyId = @VacancyGuid", new { VacancyGuid = id }).SingleOrDefault();

            return MapVacancy(dbVacancy);
        }

        public ApprenticeshipVacancy Get(long vacancyReferenceNumber)
        {
            _logger.Debug("Calling database to get apprenticeship vacancy with Vacancy Reference Number={0}", vacancyReferenceNumber);

            var dbVacancy = _getOpenConnection.Query<Repositories.Sql.Schemas.Vacancy.Entities.Vacancy>(
                "SELECT * FROM Vacancy.Vacancy WHERE VacancyReferenceNumber = @VacancyReferenceNumber",
                new { VacancyReferenceNumber = vacancyReferenceNumber }).SingleOrDefault();

            // return mongoEntity == null ? null : _mapper.Map<MongoApprenticeshipVacancy, ApprenticeshipVacancy>(mongoEntity);

            return MapVacancy(dbVacancy);
        }

        private ApprenticeshipVacancy MapVacancy(Repositories.Sql.Schemas.Vacancy.Entities.Vacancy dbVacancy)
        {
            if (dbVacancy == null)
                return null;

            // Vacancy

            var vacancyLocations = _getOpenConnection.Query<VacancyLocation>(@"
SELECT *
FROM   Vacancy.VacancyLocation
WHERE  VacancyId = @VacancyId",
new { VacancyId = dbVacancy.VacancyId });

            // TODO: Would like to make addresses immutable to allow caching - they probably don't
            // change that often. Also should have access methods that don't return the address as
            // most screens don't need it
            var addresses = _getOpenConnection.Query<Schemas.Address.Entities.PostalAddress>(@"
SELECT *
FROM   Address.PostalAddress
WHERE  PostalAddressId IN @PostalAddressIds",
new { PostalAddressIds = vacancyLocations.Select(l => l.PostalAddressId) /*.Union(dbVacancy.ManagerVacancyParty.PostalAddressId } */});

            var result = _mapper.Map<Repositories.Sql.Schemas.Vacancy.Entities.Vacancy, ApprenticeshipVacancy>(dbVacancy);

            result.LocationAddresses = new List<VacancyLocationAddress>();
            foreach (var dbLocation in vacancyLocations)
            {
                result.LocationAddresses.Add(new VacancyLocationAddress
                {
                    NumberOfPositions = dbLocation.NumberOfPositions,
                    Address = _mapper.Map<Schemas.Address.Entities.PostalAddress, SFA.Apprenticeships.Domain.Entities.Locations.Address>(addresses.Single(a => a.PostalAddressId == dbLocation.PostalAddressId))
                });
            }


            // TODO: Method which looks up in cache and if not found refreshes cache / loads new record
            result.Ukprn = _getOpenConnection
                .QueryCached<VacancyParty>(TimeSpan.FromHours(1), "SELECT * FROM Vacancy.VacancyParty")
                .Single(p => p.VacancyPartyId == dbVacancy.ManagerVacancyPartyId) // TODO: Verify
                .UKPrn.ToString(); // TODO: Casing. TODO: Type?

            // TODO: Method which looks up in cache and if not found refreshes cache / loads new record
            var employer = _getOpenConnection
                .QueryCached<VacancyParty>(TimeSpan.FromHours(1), "SELECT * FROM Vacancy.VacancyParty")
                .Single(p => p.VacancyPartyId == dbVacancy.EmployerVacancyPartyId); // TODO: Verify


            result.ProviderSiteEmployerLink.ProviderSiteErn = employer.EdsErn.ToString(); // TODO: Verify. TODO: Type?
            result.ProviderSiteEmployerLink.Employer = new Domain.Entities.Organisations.Employer()
            {
                Address = new Domain.Entities.Locations.Address()
                {
                    // TODO
                },
                //DateCreated = employer.DateCreated, TODO
                //DateUpdated = employer.DateUpdated, TODO
                //EntityId = employer.VacancyPartyId, // TODO: Verify
                Ern = employer.EdsErn.ToString(), // TODO: Verify. TODO: Case. TODO: Type?
                Name = employer.FullName
            };

            // ApprenticeshipVacancy

            if (dbVacancy.FrameworkId != null)
            {
                // TODO: QueryCachedDictionary
                result.FrameworkCodeName = _getOpenConnection
                    .QueryCached<Framework>(TimeSpan.FromHours(1), "SELECT * FROM Reference.Framework")
                    .Single(f => f.FrameworkId == dbVacancy.FrameworkId)
                    .CodeName;
            }

            // TODO: Inconsistency of location of comment fields Vacancy vs ApprenticeshipVacancy

            return result;
        }

        public List<ApprenticeshipVacancy> GetForProvider(string ukPrn, string providerSiteErn)
        {
            const string parentVacancyStatusCode = "PAR";
            _logger.Debug("Calling database to get apprenticeship vacancies with Vacancy UkPrn = {0}, providerSiteErn = {1}", ukPrn, providerSiteErn);

            var dbVacancies = _getOpenConnection.Query<Repositories.Sql.Schemas.Vacancy.Entities.Vacancy>(@"
SELECT *
FROM   Vacancy.Vacancy
WHERE  Vacancy.ManagerVacancyPartyId IN (
    SELECT VacancyPartyId
    FROM   Vacancy.VacancyParty p
    WHERE  p.UKPrn = @UkPrn
    AND    p.EdsErn = @ProviderSiteErn
)
AND Vacancy.VacancyStatusCode NOT IN @VacancyStatusCodes",
                new
                {
                    UkPrn = ukPrn,
                    ProviderSiteErn = providerSiteErn,
                    VacancyStatusCodes = new [] {parentVacancyStatusCode}
                });

            _logger.Debug(
                $"Found {dbVacancies.Count} apprenticeship vacancies with ukprn = {ukPrn}, providerSiteErn = {providerSiteErn}");

            return dbVacancies.Select(MapVacancy).ToList();
        }

        public List<ApprenticeshipVacancy> GetWithStatus(params ProviderVacancyStatuses[] desiredStatuses)
        {
            Dapper.SqlMapper.AddTypeMap(typeof(string), System.Data.DbType.AnsiString);
            _logger.Debug("Called database to get apprenticeship vacancies in status {1}", string.Join(",", desiredStatuses));

            var statuses = desiredStatuses.Select(_mapper.Map<ProviderVacancyStatuses, string>).ToList();
            
            var dbVacancies = _getOpenConnection.Query<Repositories.Sql.Schemas.Vacancy.Entities.Vacancy>(@"
SELECT *
FROM   Vacancy.Vacancy
WHERE  Vacancy.VacancyStatusCode IN @VacancyStatusCodes", new
            {
                VacancyStatusCodes = statuses
            });

            // throw new NotSupportedException("This is likely to use excessive memory. Return type should be IEnumerable.");
            _logger.Debug(
                $"Found {dbVacancies.Count} apprenticeship vacancies with statuses in {string.Join(",", desiredStatuses)}");

            return dbVacancies.Select(MapVacancy).ToList();
        }

        public List<ApprenticeshipVacancy> Find(ApprenticeshipVacancyQuery query, out int totalResultsCount)
        {
            _logger.Debug("Calling database to find apprenticeship vacancies");
            var liveStatus = _mapper.Map<ProviderVacancyStatuses, string>(ProviderVacancyStatuses.Live);

            var coreQuery = @"
FROM   Vacancy.Vacancy
WHERE  Vacancy.VacancyStatusCode = 'LIV' -- TODO: Probably would want to parameterise from constant
" + (string.IsNullOrWhiteSpace(query.FrameworkCodeName) ? "" : "AND    Vacancy.FrameworkCodeName = @FrameworkCodeName") + @"
" + (query.LiveDate.HasValue ? "AND Vacancy.PublishedDateTime >= @LiveDate" : "" ) + @" 
" + (query.LatestClosingDate.HasValue ? "AND Vacancy.ClosingDate <= @LiveDate" : "");   // Vacancy.PublishedDateTime >= @LiveDate was Vacancy.DateSubmitted >= @LiveDate

            // TODO: Vacancy.DateSubmitted should be DateLive (or DatePublished)???
            var data = _getOpenConnection.QueryMultiple<int,Repositories.Sql.Schemas.Vacancy.Entities.Vacancy>(@"
SELECT COUNT(*)
" + coreQuery + @"

SELECT *
" + coreQuery + @"
ORDER BY Vacancy.VacancyReferenceNumber
OFFSET ((@CurrentPage - 1) * @PageSize) ROWS
FETCH NEXT @PageSize ROWS ONLY
", query);

            totalResultsCount = data.Item1.Single();

            var dbVacancies = data.Item2;

            _logger.Debug("Found {0} apprenticeship vacanc(ies)", dbVacancies.Count);

            return new List<ApprenticeshipVacancy>();
        }

        public void Delete(Guid id)
        {
            _logger.Debug("Calling database to delete apprenticeship vacancy with Id={0}", id);

            throw new NotSupportedException("Don't really think vacancies are / should be ever deleted");

            //_logger.Debug("Deleted apprenticeship vacancy with Id={0}", id);
        }

        public ApprenticeshipVacancy Save(ApprenticeshipVacancy entity)
        {
            _logger.Debug("Calling database to save apprenticeship vacancy with id={0}", entity.EntityId);

            UpdateEntityTimestamps(entity);

            // TODO: Map from ApprenticeshipVacancy to Apprenticeship ??

            var dbVacancy = _mapper.Map<ApprenticeshipVacancy, Repositories.Sql.Schemas.Vacancy.Entities.Vacancy>(entity);

            dbVacancy.VacancyLocationTypeCode = "S"; // TODO: Can't get this right unless / until added to ApprenticeshipVacancy or exclude from updates

            // TODO: This should be the other way around (to avoid a race condition)
            // and be in a single call to the database (to avoid a double latency hit)
            // This should be done as a single method in _getOpenConnection

            try
            {
                _getOpenConnection.Insert(dbVacancy);
            }
            catch (Exception ex)
            {
                // TODO: Detect key violation

                if (!_getOpenConnection.UpdateSingle(dbVacancy))
                    throw new Exception("Failed to update record after failed insert", ex);

                if (entity.LocationAddresses != null)
                {
                    _getOpenConnection.MutatingQuery<int>(@"
-- TODO: Could be optimised. Locking may possibly be an issue
-- TODO: Should possibly split address into separate repo method
    DELETE Vacancy.VacancyLocation
    FROM   Vacancy.VacancyLocation
    WHERE  VacancyId = @VacancyId

    DELETE Address.PostalAddress
    WHERE  PostalAddressId IN (
        SELECT PostalAddressId
        FROM   Vacancy.VacancyLocation
        WHERE  VacancyId = @VacancyId
    )
", new { VacancyId = dbVacancy.VacancyId });
                }
            }

            if (entity.LocationAddresses != null) // TODO: Split into separate repository method
            {
                // TODO: Optimisation - insert several in one SQL round-trip
                foreach (var location in entity.LocationAddresses)
                {
                    var dbLocation = new VacancyLocation()
                    {
                        VacancyId = dbVacancy.VacancyId,
                        DirectApplicationUrl = "TODO",
                        NumberOfPositions = location.NumberOfPositions
                    };
                    
                    var dbAddress = _mapper.Map<Domain.Entities.Locations.Address, Schemas.Address.Entities.PostalAddress>(location.Address);

                    dbLocation.PostalAddressId = (int)_getOpenConnection.Insert(dbAddress);

                    _getOpenConnection.Insert(dbLocation);
                }
            }


            _logger.Debug("Saved apprenticeship vacancy with to database with id={0}", entity.EntityId);

            // TODO: Mongo used to map dbVacancy back to entity, not sure what the point in that is.
            
            return entity;
        }

        public ApprenticeshipVacancy ShallowSave(ApprenticeshipVacancy entity)
        {
            _logger.Debug("Calling database to shallow save apprenticeship vacancy with id={0}", entity.EntityId);

            UpdateEntityTimestamps(entity);

            // TODO: Map from ApprenticeshipVacancy to Apprenticeship ??

            var dbVacancy = _mapper.Map<ApprenticeshipVacancy, Repositories.Sql.Schemas.Vacancy.Entities.Vacancy>(entity);

            dbVacancy.VacancyLocationTypeCode = "S"; // TODO: Can't get this right unless / until added to ApprenticeshipVacancy or exclude from updates

            // TODO: This should be the other way around (to avoid a race condition)
            // and be in a single call to the database (to avoid a double latency hit)
            // This should be done as a single method in _getOpenConnection

            try
            {
                _getOpenConnection.Insert(dbVacancy);
            }
            catch (Exception ex)
            {
                // TODO: Detect key violation

                if (!_getOpenConnection.UpdateSingle(dbVacancy))
                    throw new Exception("Failed to update record after failed insert", ex);
            }

            _logger.Debug("Shallow saved apprenticeship vacancy with to database with id={0}", entity.EntityId);

            // TODO: Mongo used to map dbVacancy back to entity, not sure what the point in that is.

            return entity;
        }

        public ApprenticeshipVacancy ReserveVacancyForQA(long vacancyReferenceNumber)
        {
            _logger.Debug($"Calling database to get and reserve vacancy with reference number: {vacancyReferenceNumber} for QA");

            var userName = Thread.CurrentPrincipal.Identity.Name;

            // TODO: Add QAUserName / TimeStartedToQA. Perhaps a name without QA in would be better?
            // TODO: Possibly need MutatingQueryMulti to get address etc??? Or use join as only one record
            var dbVacancy = _getOpenConnection.MutatingQuery<Repositories.Sql.Schemas.Vacancy.Entities.Vacancy>(@"
UPDATE Vacancy.Vacancy
SET    QAUserName             = @UserName,
       TimeStartedToQA        = @TimeStartedToQA,
WHERE  VacancyReferenceNumber = @VacancyReferenceNumber
AND    (QAUserName IS NULL OR (QAUserName = @userName))
AND    (TimeStartedToQA IS NULL OR (TimeStartedToQA > @lockExpiryTime))

DECLARE @RowCount INT = @@RowCount

IF RowCount > 1
    RAISERROR etc etc.

SELECT *
FROM   Vacancy.Vacancy
JOIN   Address.Address etc. etc.
WHERE  etc etc
AND    @RowCount = 1
", new { userName = userName, TimeStartedToQA = DateTime.UtcNow }).SingleOrDefault();

            if (dbVacancy == null)
            {
                // TODO: Mongodb references
                //_logger.Warn($"Call to Mongodb to get and reserve vacancy with reference number: {vacancyReferenceNumber} for QA failed: {result.Code}, {result.ErrorMessage}");
                return null;
            }

            //_logger.Info($"Called Mongodb to get and reserve vacancy with reference number: {vacancyReferenceNumber} for QA successfully");

            // TODO: Mapping
            return null;
        }

        public ApprenticeshipVacancy ReplaceLocationInformation(long vacancyReferenceNumber,
            bool? isEmployerLocationMainApprenticeshipLocation, int? numberOfPositions, IEnumerable<VacancyLocationAddress> vacancyLocationAddresses,
            string locationAddressesComment, string additionalLocationInformation, string additionalLocationInformationComment)
        {
            throw new NotImplementedException();
        }

        private void UpdateEntityTimestamps(ApprenticeshipVacancy entity)
        {
            /* TODO
            // determine whether this is a "new" entity being saved for the first time
            if (entity.DateTimeCreated == DateTime.MinValue)
            {
                entity.DateTimeCreated = DateTime.UtcNow;
                entity.DateTimeUpdated = null;
            }
            else
            {
                entity.DateTimeUpdated = DateTime.UtcNow;
            }
            */
        }
    }
}
