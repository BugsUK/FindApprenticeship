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
    using CuttingEdge.Conditions;
    using Reference.Entities;
    using Entities;
    using SFA.Infrastructure.Interfaces;
    using System.Diagnostics;
    using Vacancy = Entities.Vacancy;

    // TODO GenericSqlClient??
    public class ApprenticeshipVacancyRepository : IApprenticeshipVacancyReadRepository, IApprenticeshipVacancyWriteRepository
    {
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

            var dbVacancy = _getOpenConnection.Query<Entities.Vacancy>("SELECT * FROM Vacancy.Vacancy WHERE VacancyId = @VacancyGuid", new { VacancyGuid = id }).SingleOrDefault();

            return MapVacancy(dbVacancy);
        }

        public ApprenticeshipVacancy Get(int vacancyReferenceNumber)
        {
            _logger.Debug("Calling database to get apprenticeship vacancy with Vacancy Reference Number={0}", vacancyReferenceNumber);

            var dbVacancy = _getOpenConnection.Query<Entities.Vacancy>(
                "SELECT * FROM Vacancy.Vacancy WHERE VacancyReferenceNumber = @VacancyReferenceNumber",
                new { VacancyReferenceNumber = vacancyReferenceNumber }).SingleOrDefault();

            // return mongoEntity == null ? null : _mapper.Map<MongoApprenticeshipVacancy, ApprenticeshipVacancy>(mongoEntity);

            return MapVacancy(dbVacancy);
        }

        private ApprenticeshipVacancy MapVacancy(Entities.Vacancy dbVacancy)
        {
            if (dbVacancy == null)
                return null;

            // Vacancy

            var vacancyLocations = _getOpenConnection.Query<VacancyLocation>(@"
SELECT *
FROM   Vacancy.VacancyLocation
WHERE  VacancyId = @VacancyId",
new { VacancyId = dbVacancy.VacancyId });

            // TODO: Method which looks up in cache and if not found refreshes cache / loads new record
            //var employer = _getOpenConnection
            //    .QueryCached<VacancyParty>(TimeSpan.FromHours(1), "SELECT * FROM Vacancy.VacancyParty")
            //    .Single(p => p.VacancyPartyId == dbVacancy.EmployerVacancyPartyId); // TODO: Verify

            //if (!employer.PostalAddressId.HasValue)
            //    throw new InvalidOperationException("All employers should have addresses");

            // TODO: Would like to make addresses immutable to allow caching - they probably don't
            // change that often. Also should have access methods that don't return the address as
            // most screens don't need it
//            var addresses = _getOpenConnection.Query<Schemas.Address.Entities.PostalAddress>(@"
//SELECT *
//FROM   Address.PostalAddress
//WHERE  PostalAddressId IN @PostalAddressIds",
//new { PostalAddressIds = vacancyLocations.Select(l => l.PostalAddressId).Union(new int[] { employer.PostalAddressId.Value }) });

//            var result = _mapper.Map<Repositories.Sql.Schemas.Vacancy.Entities.Vacancy, ApprenticeshipVacancy>(dbVacancy);

//            result.LocationAddresses = new List<VacancyLocationAddress>();
//            foreach (var dbLocation in vacancyLocations)
//            {
//                result.LocationAddresses.Add(new VacancyLocationAddress
//                {
//                    NumberOfPositions = dbLocation.NumberOfPositions,
//                    Address = _mapper.Map<Schemas.Address.Entities.PostalAddress, PostalAddress>(addresses.Single(a => a.PostalAddressId == dbLocation.PostalAddressId)) //VGA_Address
//                });
//            }

            //// TODO: Method which looks up in cache and if not found refreshes cache / loads new record
            //result.Ukprn = _getOpenConnection
            //    .QueryCached<VacancyParty>(TimeSpan.FromHours(1), "SELECT * FROM Vacancy.VacancyParty")
            //    .Single(p => p.VacancyPartyId == dbVacancy.ManagerVacancyPartyId) // TODO: Verify
            //    .UKPrn.ToString(); // TODO: Type?


            //result.ProviderSiteEmployerLink.ProviderSiteErn = employer.EdsErn.ToString(); // TODO: Verify. TODO: Type?

            ////var x = _mapper.Map<Schemas.Address.Entities.PostalAddress, PostalAddress>(addresses.Single(a => a.PostalAddressId == employer.PostalAddressId));

            //result.ProviderSiteEmployerLink.Employer = new Domain.Entities.Organisations.Employer
            //{
            //    Address = _mapper.Map<Schemas.Address.Entities.PostalAddress,PostalAddress>(addresses.Single(a => a.PostalAddressId == employer.PostalAddressId)),
            //    //DateCreated = employer.DateCreated, TODO
            //    //DateUpdated = employer.DateUpdated, TODO
            //    //EntityId = employer.VacancyPartyId, // TODO: Verify
            //    Ern = employer.EdsErn.ToString(), // TODO: Verify. TODO: Case. TODO: Type?
            //    Name = employer.FullName
            //};

            //// ApprenticeshipVacancy

            //if (dbVacancy.FrameworkId != null)
            //{
            //    // TODO: QueryCachedDictionary
            //    result.FrameworkCodeName = _getOpenConnection
            //        .QueryCached<Framework>(TimeSpan.FromHours(1), "SELECT * FROM Reference.Framework")
            //        .Single(f => f.FrameworkId == dbVacancy.FrameworkId)
            //        .CodeName;
            //}

            //// TODO: Inconsistency of location of comment fields Vacancy vs ApprenticeshipVacancy

            //return result;

            return new ApprenticeshipVacancy();
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
            _logger.Debug("Called database to get apprenticeship vacancies in status {0}", string.Join(",", desiredStatuses));

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

            var paramObject =
                new
                {
                    FrameworkCodeName = query.FrameworkCodeName,
                    LiveDate = query.LiveDate,
                    LatestClosingDate = query.LatestClosingDate,
                    VacancyStatusCode = liveStatus,
                    CurrentPage = query.CurrentPage,
                    PageSize = query.PageSize
                };

            var coreQuery = @"
FROM   Vacancy.Vacancy
WHERE  Vacancy.VacancyStatusCode = @VacancyStatusCode
" + (string.IsNullOrWhiteSpace(query.FrameworkCodeName) ? "" : "AND Vacancy.FrameworkId = (SELECT FrameworkId FROM Reference.Framework where CodeName = @FrameworkCodeName) ") + @"
" + (query.LiveDate.HasValue ? "AND Vacancy.PublishedDateTime >= @LiveDate" : "" ) + @" 
" + (query.LatestClosingDate.HasValue ? "AND Vacancy.ClosingDate <= @LatestClosingDate" : "");   // Vacancy.PublishedDateTime >= @LiveDate was Vacancy.DateSubmitted >= @LiveDate

            // TODO: Vacancy.DateSubmitted should be DateLive (or DatePublished)???
            var data = _getOpenConnection.QueryMultiple<int,Repositories.Sql.Schemas.Vacancy.Entities.Vacancy>(@"
SELECT COUNT(*)
" + coreQuery + @"

SELECT *
" + coreQuery + @"
ORDER BY Vacancy.VacancyReferenceNumber
OFFSET ((@CurrentPage - 1) * @PageSize) ROWS
FETCH NEXT @PageSize ROWS ONLY
", paramObject);

            totalResultsCount = data.Item1.Single();

            var dbVacancies = data.Item2;

            _logger.Debug("Found {0} apprenticeship vacanc(ies)", dbVacancies.Count);

            return dbVacancies.Select(MapVacancy).ToList();
        }

        public ApprenticeshipVacancy DeepSave(ApprenticeshipVacancy entity)
        {
            _logger.Debug("Calling database to save apprenticeship vacancy with id={0}", entity.EntityId);

            Condition.Requires(entity.LocationAddresses, "LocationAddresses").IsNotNull();

            UpdateEntityTimestamps(entity);

            var dbVacancy = _mapper.Map<ApprenticeshipVacancy, Repositories.Sql.Schemas.Vacancy.Entities.Vacancy>(entity);

            //if (dbVacancy.VacancyLocationTypeCode == VacancyLocationType.Employer)
            //{
            //    if (entity.LocationAddresses.Count != 0)
            //        throw new InvalidOperationException(entity.LocationAddresses.Count.ToString());
            //}
            //else if (entity.LocationAddresses.Count > 1)
            //{
            //    if (dbVacancy.VacancyLocationTypeCode != VacancyLocationType.Specific)
            //        throw new InvalidOperationException(dbVacancy.VacancyLocationTypeCode);
            //    dbVacancy.VacancyLocationTypeCode = "M";
            //}

            int employerPostalAddressId = PopulateVacancyPartyIds(entity, dbVacancy);
            PopulateFrameworkId(entity, dbVacancy);

            // TODO: This should be in a single call to the database (to avoid a double latency hit)
            // This should be done as a single method in _getOpenConnection

            var vacancyId = dbVacancy.VacancyId;

            try
            {
                _getOpenConnection.Insert(dbVacancy);
            }
            catch (Exception ex)
            {
                // TODO: Detect key violation

                if (!_getOpenConnection.UpdateSingle(dbVacancy))
                    throw new Exception("Failed to update record after failed insert", ex);

                //if (dbVacancy.VacancyLocationTypeCode != VacancyLocationType.Employer)
                //{
                //    RemoveVacancyLocationAddresses(vacancyId);
                //}
            }

            // TODO: Optimisation - insert several in one SQL round-trip
            InsertVacancyLocationAddresses(entity.LocationAddresses, entity.EntityId);

            //if (dbVacancy.VacancyLocationTypeCode == VacancyLocationType.Employer)
            //{
            //    InsertEmployerLocationAddressAsVacancyLocationAddress(entity.NumberOfPositions.Value, dbVacancy, employerPostalAddressId);
            //}

            _logger.Debug("Saved apprenticeship vacancy with to database with id={0}", entity.EntityId);

            return entity;
        }

        private void InsertEmployerLocationAddressAsVacancyLocationAddress(int numberOfPositions, Vacancy dbVacancy,
            int employerPostalAddressId)
        {
            var dbLocation = new VacancyLocation()
            {
                // VacancyId = dbVacancy.VacancyId,
                DirectApplicationUrl = "TODO",
                NumberOfPositions = numberOfPositions,
                PostalAddressId = employerPostalAddressId
            };

            _getOpenConnection.Insert(dbLocation);
        }

        public ApprenticeshipVacancy ShallowSave(ApprenticeshipVacancy entity)
        {
            _logger.Debug("Calling database to shallow save apprenticeship vacancy with id={0}", entity.EntityId);

            UpdateEntityTimestamps(entity);

            var dbVacancy = _mapper.Map<ApprenticeshipVacancy, Entities.Vacancy>(entity);

            //dbVacancy.VacancyLocationTypeCode = "S"; // TODO: Can't get this right unless / until added to ApprenticeshipVacancy or exclude from updates

            PopulateVacancyPartyIds(entity, dbVacancy);
            PopulateFrameworkId(entity, dbVacancy);

            // TODO: This should be in a single call to the database (to avoid a double latency hit)
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

            return entity;
        }

        private int PopulateVacancyPartyIds(ApprenticeshipVacancy vacancy, Entities.Vacancy dbVacancy)
        {
            var results = _getOpenConnection.QueryMultiple<dynamic, int>(@"
SELECT VacancyPartyId, PostalAddressId
FROM   Vacancy.VacancyParty
WHERE  EdsErn = @EdsErn

SELECT VacancyPartyId
FROM   Vacancy.VacancyParty
WHERE  UKPrn = @UKPrn
", new { EdsErn = vacancy.ProviderSiteEmployerLink.Employer.Ern, UkPrn = vacancy.Ukprn });

            //dbVacancy.EmployerVacancyPartyId = results.Item1.Single().VacancyPartyId; 
            //dbVacancy.ContractOwnerVacancyPartyId = results.Item2.Single(); // TODO: Test
            //dbVacancy.DeliveryProviderVacancyPartyId = results.Item2.Single();
            //dbVacancy.ManagerVacancyPartyId = results.Item2.Single();
            //dbVacancy.OwnerVacancyPartyId = results.Item2.Single();
            //dbVacancy.OriginalContractOwnerVacancyPartyId = results.Item2.Single(); // TODO: ???

            return results.Item1.Single().PostalAddressId;
        }
        
        private void PopulateFrameworkId(ApprenticeshipVacancy vacancy, Entities.Vacancy dbVacancy)
        {
            // TODO: use query cached?
            var framework = _getOpenConnection.Query<int>(@"
SELECT FrameworkId
FROM Reference.Framework
WHERE CodeName = @FrameworkCodeName
", new {vacancy.FrameworkCodeName});

            if (framework.Any())
            {
                dbVacancy.FrameworkId = framework.First();
            }
            else
            {
                dbVacancy.FrameworkId = null;
            }
        }

        public ApprenticeshipVacancy ReserveVacancyForQA(int vacancyReferenceNumber)
        {
            _logger.Debug($"Calling database to get and reserve vacancy with reference number: {vacancyReferenceNumber} for QA");

            var userName = Thread.CurrentPrincipal.Identity.Name;

            // TODO: Add QAUserName / TimeStartedToQA. Perhaps a name without QA in would be better?
            // TODO: Possibly need MutatingQueryMulti to get address etc??? Or use join as only one record
            var dbVacancy = _getOpenConnection.MutatingQuery<Entities.Vacancy>(@"
UPDATE Vacancy.Vacancy
SET    QAUserName             = @UserName,
       TimeStartedToQA        = @TimeStartedToQA,
       VacancyStatusCode      = 'RES'
WHERE  VacancyReferenceNumber = @VacancyReferenceNumber
AND    (QAUserName IS NULL OR (QAUserName = @UserName))
-- AND    (TimeStartedToQA IS NULL OR (TimeStartedToQA > @lockExpiryTime))

DECLARE @RowCount INT = @@RowCount

-- IF RowCount > 1
--     RAISERROR etc etc.

SELECT * FROM Vacancy.Vacancy WHERE VacancyReferenceNumber = @VacancyReferenceNumber
--AND    @RowCount = 1 -- what does it mean?
", new { UserName = userName, TimeStartedToQA = DateTime.UtcNow, VacancyReferenceNumber = vacancyReferenceNumber }).SingleOrDefault();

            if (dbVacancy == null)
            {
                _logger.Warn($"Call to database to get and reserve vacancy with reference number: {vacancyReferenceNumber} for QA failed");
                return null;
            }

            _logger.Info($"Called database to get and reserve vacancy with reference number: {vacancyReferenceNumber} for QA successfully");

            return MapVacancy(dbVacancy);
        }

        public ApprenticeshipVacancy ReplaceLocationInformation(Guid vacancyGuid,
            bool? isEmployerLocationMainApprenticeshipLocation, int? numberOfPositions, IEnumerable<VacancyLocationAddress> vacancyLocationAddresses,
            string locationAddressesComment, string additionalLocationInformation, string additionalLocationInformationComment)
        {
            // TODO: isEmployerLocationMainApprenticeshipLocation -> VacancyLocationType => M -> Multiple locations, S -> Specific location

            Condition.Requires(vacancyLocationAddresses, "vacancyLocationAddresses").IsNotNull();

            var vacancyLocationTypeCode = isEmployerLocationMainApprenticeshipLocation != null &&
                                      isEmployerLocationMainApprenticeshipLocation.Value
                ? "S"
                : "M"; //TODO: review. I don't think it's true. We should add a new property and update this one as well.

            var dbVacancy = _getOpenConnection.MutatingQuery<Vacancy>(@"
UPDATE Vacancy.Vacancy
SET    NumberOfPositions                         = @NumberOfPositions,
       LocationAddressesComment                  = @LocationAddressesComment,
       AdditionalLocationInformation             = @AdditionalLocationInformation,
       AdditionalLocationInformationComment      = @AdditionalLocationInformationComment,
       VacancyLocationTypeCode                   = @VacancyLocationTypeCode
WHERE  VacancyId = @VacancyId

DECLARE @RowCount INT = @@RowCount

-- IF RowCount > 1
--     RAISERROR etc etc.

SELECT * FROM Vacancy.Vacancy WHERE VacancyId = @VacancyId
--AND    @RowCount = 1 -- what does it mean?
",
                new
                {
                    NumberOfPositions = numberOfPositions,
                    LocationAddressesComment = locationAddressesComment,
                    VacancyId = vacancyGuid,
                    AdditionalLocationInformation = additionalLocationInformation,
                    AdditionalLocationInformationComment = additionalLocationInformationComment,
                    VacancyLocationTypeCode = vacancyLocationTypeCode
                }).SingleOrDefault();

            var employerPostalAddressId = _getOpenConnection.Query<int>(@"
SELECT PostalAddressId
FROM   Vacancy.VacancyParty
WHERE  VacancyPartyId = @VacancyPartyId",
                new
                {
                    // VacancyPartyId = dbVacancy.EmployerVacancyPartyId
                }).Single();

            RemoveVacancyLocationAddresses(vacancyGuid); // Only if is not the same as the employer

            InsertVacancyLocationAddresses(vacancyLocationAddresses, Get(vacancyGuid).EntityId); // TODO: can be done in another way?

            //if (dbVacancy.VacancyLocationTypeCode == VacancyLocationType.Employer)
            //{
            //    InsertEmployerLocationAddressAsVacancyLocationAddress(numberOfPositions.Value, dbVacancy, employerPostalAddressId);
            //}

            return MapVacancy(dbVacancy);
        }

        private void UpdateEntityTimestamps(ApprenticeshipVacancy entity)
        {
            // determine whether this is a "new" entity being saved for the first time
            if (entity.DateCreated == DateTime.MinValue)
            {
                entity.DateCreated = DateTime.UtcNow;
                entity.DateUpdated = null;
            }
            else
            {
                entity.DateUpdated = DateTime.UtcNow;
            }
        }

        private void InsertVacancyLocationAddresses(IEnumerable<VacancyLocationAddress> vacancyLocationAddresses, Guid vacancyId)
        {
            foreach (var location in vacancyLocationAddresses)
            {
                var dbLocation = new VacancyLocation()
                {
                    VacancyId = vacancyId,
                    DirectApplicationUrl = "TODO",
                    NumberOfPositions = location.NumberOfPositions
                };

                var dbAddress = _mapper.Map<PostalAddress, Schemas.Address.Entities.PostalAddress>(location.Address); //VGA_Address

                dbLocation.PostalAddressId = (int)_getOpenConnection.Insert(dbAddress);

                _getOpenConnection.Insert(dbLocation);
            }
        }

        private void RemoveVacancyLocationAddresses(Guid vacancyId)
        {
            var sql = GetRemoveVacancyLocationAddressesQuery("@VacancyId");
            _getOpenConnection.MutatingQuery<int>(sql, new { VacancyId = vacancyId });
        }

        private string GetRemoveVacancyLocationAddressesQuery(string selectVacancyIdQuery)
        {
            // Should remove all the addresses unless the ones that are the address of an employer.
            return
                $@"
-- TODO: Could be optimised. Locking may possibly be an issue
-- TODO: Should possibly split address into separate repo method

    DECLARE @PostalAddressIdToRemove TABLE (PostalAddressId INT);

    INSERT INTO @PostalAddressIdToRemove
        SELECT PostalAddressId
        FROM   Vacancy.VacancyLocation
        WHERE  VacancyId = {selectVacancyIdQuery}
        AND PostalAddressId NOT IN ( SELECT PostalAddressId FROM Vacancy.VacancyParty )

    DELETE Vacancy.VacancyLocation
    FROM   Vacancy.VacancyLocation
    WHERE  VacancyId = {selectVacancyIdQuery}

    DELETE Address.PostalAddress
    WHERE  PostalAddressId IN (
        SELECT PostalAddressId FROM @PostalAddressIdToRemove
    )";
        }
    }
}
