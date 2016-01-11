namespace SFA.Apprenticeships.Infrastructure.Repositories.Vacancies.Sql
{
    using Domain.Entities.Vacancies.ProviderVacancies;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using SFA.Apprenticeships.Domain.Interfaces.Repositories;
    using SFA.Infrastructure.Interfaces;
    using SFA.Infrastructure.Sql;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Vacancy = SFA.Apprenticeships.NewDB.Domain.Entities.Vacancy;
    using Reference = SFA.Apprenticeships.NewDB.Domain.Entities;
    using Domain.Interfaces.Queries;
    using System.Threading;    // TODO GenericSqlClient??
    public class ApprenticeshipVacancyRepository : IApprenticeshipVacancyReadRepository, IApprenticeshipVacancyWriteRepository
    {
        private IDictionary<string, WageType> _wageTypeMap = new Dictionary<string, WageType>();

        private readonly IMapper _mapper;
        private readonly ILogService _logger;

        private readonly IGetOpenConnection _getOpenConnection;

        /*
        public ApprenticeshipVacancyRepository(
            IConfigurationService configurationService,
            IMapper mapper,
            ILogService logger)
        {
            var config = configurationService.Get<MongoConfiguration>();

            Initialise(config.VacancyDb, "apprenticeshipVacancies");

            _mapper = mapper;
            _logger = logger;
        }
        */

        public ApprenticeshipVacancyRepository(IGetOpenConnection getOpenConnection)
        {
            _getOpenConnection = getOpenConnection;
        }

        public ApprenticeshipVacancy Get(Guid id)
        {
            _logger.Debug("Called Mongodb to get apprenticeship vacancy with Id={0}", id);

            var dbVacancy = _getOpenConnection.Query<Vacancy.Vacancy>("SELECT * FROM Vacancy.Vacancy WHERE VacancyGuid = @VacancyGuid", new { VacancyGuid = id }).SingleOrDefault();

            // TODO: Use mapper (automapper?)
            // return mongoEntity == null ? null : _mapper.Map<MongoApprenticeshipVacancy, ApprenticeshipVacancy>(mongoEntity);

            if (dbVacancy == null)
                return null;

            // Vacancy

            var result = _mapper.Map<Vacancy.Vacancy, ApprenticeshipVacancy>(dbVacancy);

            // TODO: Method which looks up in cache and if not found refreshes cache / loads new record
            result.Ukprn = _getOpenConnection
                    .QueryCached<Vacancy.VacancyParty>(TimeSpan.FromHours(1), "SELECT * FROM Vacancy.VacancyParty")
                    .Single(p => p.VacancyPartyId == dbVacancy.ManagerVacancyPartyId) // TODO: Verify
                    .UKPRN.ToString(); // TODO: Casing. TODO: Type?

            // TODO: Method which looks up in cache and if not found refreshes cache / loads new record
            var employer = _getOpenConnection
                    .QueryCached<Vacancy.VacancyParty>(TimeSpan.FromHours(1), "SELECT * FROM Vacancy.VacancyParty")
                    .Single(p => p.VacancyPartyId == dbVacancy.EmployerVacancyPartyId); // TODO: Verify

            result.ProviderSiteEmployerLink = new Domain.Entities.Providers.ProviderSiteEmployerLink()
            {
                ProviderSiteErn = employer.EDSURN.ToString(), // TODO: Verify. TODO: Case. TODO: Type?
                Description = employer.Description,
                WebsiteUrl = employer.WebsiteUrl,
                Employer = new Domain.Entities.Organisations.Employer()
                {
                    Address = new Domain.Entities.Locations.Address()
                    {
                        // TODO
                    },
                    //DateCreated = employer.DateCreated, TODO
                    //DateUpdated = employer.DateUpdated, TODO
                    //EntityId = employer.VacancyPartyId, // TODO: Verify
                    Ern = employer.EDSURN.ToString(), // TODO: Verify. TODO: Case. TODO: Type?
                    Name = employer.FullName
                }
            };


            // ApprenticeshipVacancy

            if (dbVacancy.FrameworkId != null)
            {
                // TODO: QueryCachedDictionary
                result.FrameworkCodeName = _getOpenConnection
                    .QueryCached<Reference.Framework>(TimeSpan.FromHours(1), "SELECT * FROM Reference.Framework")
                    .Single(f => f.FrameworkId == dbVacancy.FrameworkId)
                    .CodeName;
            }

            result.FrameworkCodeNameComment = dbVacancy.FrameworkIdComment.ToString(); // TODO

            // TODO: Inconsistency of location of comment fields Vacancy vs ApprenticeshipVacancy
            result.WorkingWeekComment = "TODO"; // dbVacancy.WorkingWeekComment;
            result.AdditionalLocationInformation = "TODO";

            // TODO
            result.LocationAddresses = new List<SFA.Apprenticeships.Domain.Entities.Locations.VacancyLocationAddress>();

            result.IsEmployerLocationMainApprenticeshipLocation = true; // TODO

            result.NumberOfPositions = 1; // TODO

            return result;
        }

        public ApprenticeshipVacancy Get(long vacancyReferenceNumber)
        {
            _logger.Debug("Called Mongodb to get apprenticeship vacancy with Vacancy Reference Number={0}", vacancyReferenceNumber);

            var dbVacancy = _getOpenConnection.Query<Vacancy.Vacancy>(
                "SELECT * FROM Vacancy.Vacancy WHERE VacancyReferenceNumber = @VacancyReferenceNumber",
                new { VacancyReferenceNumber = vacancyReferenceNumber }).SingleOrDefault();

            // return mongoEntity == null ? null : _mapper.Map<MongoApprenticeshipVacancy, ApprenticeshipVacancy>(mongoEntity);

            if (dbVacancy == null)
                return null;

            // TODO: Use mapper (automapper?) as above

            var result = new ApprenticeshipVacancy();

            return result;
        }

        public List<ApprenticeshipVacancy> GetForProvider(string ukPrn)
        {
            _logger.Debug("Called Mongodb to get apprenticeship vacancies with Vacancy UkPrn = {0}", ukPrn);

            var dbVacancies = _getOpenConnection.Query<Vacancy.Vacancy>(@"
SELECT *
FROM   Vacancy.Vacancy
WHERE  Vacancy.ManagerVacancyPartyId IN (
    SELECT VacancyPartyId
    FROM   Vacancy.VacancyParty p
    WHERE  p.UKPRN = @UkPrn
)", new { UkPrn = ukPrn });

            _logger.Debug(string.Format("Found {0} apprenticeship vacancies with ukprn ={1}", dbVacancies.Count, ukPrn));

            // TODO: Mapping as above

            return new List<ApprenticeshipVacancy>();
        }

        public List<ApprenticeshipVacancy> GetForProvider(string ukPrn, string providerSiteErn)
        {
            _logger.Debug("Called Mongodb to get apprenticeship vacancies with Vacancy UkPrn = {0}, providerSiteErn = {1}", ukPrn, providerSiteErn);

            var dbVacancies = _getOpenConnection.Query<Vacancy.Vacancy>(@"
SELECT *
FROM   Vacancy.Vacancy
WHERE  Vacancy.ManagerVacancyPartyId IN (
    SELECT VacancyPartyId
    FROM   Vacancy.VacancyParty p
    WHERE  p.UKPRN = @UkPrn
    AND    p.EDSURN = @ProviderSiteUrn
)", new { UkPrn = ukPrn, ProviderSiteUrn = providerSiteErn });

            _logger.Debug(string.Format("Found {0} apprenticeship vacancies with ukprn = {1}, providerSiteErn = {2}", dbVacancies.Count, ukPrn, providerSiteErn));

            return new List<ApprenticeshipVacancy>();
        }

        public List<ApprenticeshipVacancy> GetForProvider(string ukPrn, params ProviderVacancyStatuses[] desiredStatuses)
        {
            _logger.Debug("Called Mongodb to get apprenticeship vacancies with Vacancy UkPrn = {0} and in status {1}", ukPrn, string.Join(",", desiredStatuses));

            var dbVacancies = _getOpenConnection.Query<Vacancy.Vacancy>(@"
SELECT *
FROM   Vacancy.Vacancy
WHERE  Vacancy.ManagerVacancyPartyId IN (
    SELECT VacancyPartyId
    FROM   Vacancy.VacancyParty p
    WHERE  p.UKPRN = @UkPrn)
AND    Vacancy.VacancyStatusCode IN (@VacancyStatusCodes)
)", new { UkPrn = ukPrn /*, TODO VacancyStatusCodes = new List<string>(desiredStatuses.Select(s => _vacancyStatusReverseMap[s])) */} );

            _logger.Debug(string.Format("Found {0} apprenticeship vacancies with ukprn = {1} and statuses in {2}", dbVacancies.Count, ukPrn, string.Join(",", desiredStatuses)));

            return new List<ApprenticeshipVacancy>();
        }

        public List<ApprenticeshipVacancy> GetWithStatus(params ProviderVacancyStatuses[] desiredStatuses)
        {
            _logger.Debug("Called Mongodb to get apprenticeship vacancies in status {0}", string.Join(",", desiredStatuses));

            throw new NotSupportedException("This is likely to use excessive memory. Return type should be IEnumerable.");


            //_logger.Debug(string.Format("Found {0} apprenticeship vacancies with statuses in {1}", mongoEntities.Count, string.Join(",", desiredStatuses)));
        }

        public List<ApprenticeshipVacancy> Find(ApprenticeshipVacancyQuery query, out int totalResultsCount)
        {
            _logger.Debug("Calling repository to find apprenticeship vacancies");

            var coreQuery = @"
FROM   Vacancy.Vacancy
WHERE  Vacancy.VacancyStatus = 'LIV' -- TODO: Probably would want to parameterise from constant
" + (string.IsNullOrWhiteSpace(query.FrameworkCodeName) ? "" : "AND    Vacancy.FrameworkCodeName = @FrameworkCodeName") + @"
" + (query.LiveDate.HasValue ? "AND     Vacancy.DateSubmitted >= @LiveDate" : "");

            // TODO: Vacancy.DateSubmitted should be DateLive (or DatePublished)???
            var data = _getOpenConnection.QueryMultiple<int,Vacancy.Vacancy>(@"
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
            _logger.Debug("Calling repository to delete apprenticeship vacancy with Id={0}", id);

            throw new NotSupportedException("Don't really think vacancies are / should be ever deleted");

            _logger.Debug("Deleted apprenticeship vacancy with Id={0}", id);
        }

        public ApprenticeshipVacancy Save(ApprenticeshipVacancy entity)
        {
            _logger.Debug("Called Mongodb to save apprenticeship vacancy with id={0}", entity.EntityId);

            // UpdateEntityTimestamps(entity);

            // TODO: Map from ApprenticeshipVacancy to Apprenticeship

            /*
            _getOpenConnection.UpdateSingle(entity);
            _getOpenConnection.UpdateSingle(address);
            _getOpenConnection.UpdateSingle(vacancy);
            */

            _logger.Debug("Saved apprenticeship vacancy with to Mongodb with id={0}", entity.EntityId);

            // TODO: Not sure need to map back??
            /*
            return _mapper.Map<MongoApprenticeshipVacancy, ApprenticeshipVacancy>(mongoEntity);
            */

            throw new NotImplementedException();
        }

        public ApprenticeshipVacancy ReserveVacancyForQA(long vacancyReferenceNumber)
        {
            _logger.Debug($"Calling Mongodb to get and reserve vacancy with reference number: {vacancyReferenceNumber} for QA");

            var userName = Thread.CurrentPrincipal.Identity.Name;

            // TODO: Add QAUserName / TimeStartedToQA. Perhaps a name without QA in would be better?
            // TODO: Possibly need MutatingQueryMulti to get address etc??? Or use join as only one record
            var dbVacancy = _getOpenConnection.MutatingQuery<Vacancy.Vacancy>(@"
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
    }
}
