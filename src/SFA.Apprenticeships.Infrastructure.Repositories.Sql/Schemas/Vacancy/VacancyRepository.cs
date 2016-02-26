namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Vacancy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Common;
    using Domain.Entities.Raa.Vacancies;
    using DomainVacancy = Domain.Entities.Raa.Vacancies.Vacancy;
    using Domain.Raa.Interfaces.Queries;
    using Domain.Raa.Interfaces.Repositories;
    using SFA.Infrastructure.Interfaces;
    using Vacancy = Entities.Vacancy;

    public class VacancyRepository : IVacancyReadRepository, IVacancyWriteRepository
    {
        private readonly IMapper _mapper;
        private readonly ILogService _logger;

        private readonly IGetOpenConnection _getOpenConnection;

        public VacancyRepository(IGetOpenConnection getOpenConnection, IMapper mapper, ILogService logger)
        {
            _getOpenConnection = getOpenConnection;
            _mapper = mapper;
            _logger = logger;
        }

        public DomainVacancy Get(int vacancyId)
        {
            _logger.Debug("Calling database to get apprenticeship vacancy with VacancyId={0}",
                vacancyId);

            var dbVacancy = _getOpenConnection.Query<Vacancy>(
                "SELECT * FROM dbo.Vacancy WHERE VacancyId = @VacancyId",
                new { VacancyId = vacancyId}).SingleOrDefault();

            return MapVacancy(dbVacancy);
        }

        public DomainVacancy GetByReferenceNumber(int vacancyReferenceNumber)
        {
            _logger.Debug("Calling database to get apprenticeship vacancy with Vacancy Reference Number={0}",
                vacancyReferenceNumber);

            var dbVacancy = _getOpenConnection.Query<Vacancy>(
                "SELECT * FROM dbo.Vacancy WHERE VacancyReferenceNumber = @VacancyReferenceNumber",
                new { VacancyReferenceNumber = vacancyReferenceNumber }).SingleOrDefault();

            return MapVacancy(dbVacancy);
        }

        public DomainVacancy GetByVacancyGuid(Guid vacancyGuid)
        {
            _logger.Debug("Calling database to get apprenticeship vacancy with VacancyGuid={0}",
                vacancyGuid);

            var dbVacancy = _getOpenConnection.Query<Vacancy>(
                "SELECT * FROM dbo.Vacancy WHERE VacancyGuid = @VacancyGuid",
                new { VacancyGuid = vacancyGuid }).SingleOrDefault();

            return MapVacancy(dbVacancy);
        }

        public List<DomainVacancy> GetByIds(IEnumerable<int> vacancyIds)
        {
            _logger.Debug("Calling database to get apprenticeship vacancy with Ids={0}", string.Join(", ", vacancyIds));

            var vacancies =
                _getOpenConnection.Query<Vacancy>("SELECT * FROM dbo.Vacancy WHERE VacancyId IN @VacancyIds",
                    new { VacancyIds = vacancyIds });

            return vacancies.Select(MapVacancy).ToList();
        }

        public List<DomainVacancy> GetByOwnerPartyIds(IEnumerable<int> ownerPartyIds)
        {
            _logger.Debug("Calling database to get apprenticeship vacancy with VacancyOwnerRelationshipId={0}", string.Join(", ", ownerPartyIds));

            var vacancies =
                _getOpenConnection.Query<Vacancy>("SELECT * FROM dbo.Vacancy WHERE VacancyOwnerRelationshipId IN @VacancyOwnerRelationshipIds",
                    new { VacancyOwnerRelationshipIds = ownerPartyIds });

            return vacancies.Select(MapVacancy).ToList();
        }

        private DomainVacancy MapVacancy(Vacancy dbVacancy)
        {
            if (dbVacancy == null)
                return null;

            // Locations and providersiteemployerlink

            var result = _mapper.Map<Vacancy, DomainVacancy>(dbVacancy);
            MapAdditionalQuestions(dbVacancy, result);
            MapTextFields(dbVacancy, result);
            MapIsEmployerLocationMainApprenticeshipLocation(dbVacancy, result);
            MapApprenticeshipType(dbVacancy, result);
            MapFrameworkId(dbVacancy, result);

            return result;
        }

        private void MapFrameworkId(Vacancy dbVacancy, DomainVacancy result)
        {
            if (dbVacancy.ApprenticeshipFrameworkId.HasValue)
            {
                result.FrameworkCodeName =
                    _getOpenConnection.QueryCached<string>(TimeSpan.FromHours(1), @"
SELECT CodeName
FROM   dbo.ApprenticeshipFramework
WHERE  ApprenticeshipFrameworkId = @ApprenticeshipFrameworkId",
                        new
                        {
                            ApprenticeshipFrameworkId = dbVacancy.ApprenticeshipFrameworkId.Value
                        }).Single();
            }
        }

        private void MapApprenticeshipType(Vacancy dbVacancy, DomainVacancy result)
        {
            if (dbVacancy.ApprenticeshipType.HasValue)
            {
                result.ApprenticeshipLevel =
                    (ApprenticeshipLevel) _getOpenConnection.QueryCached<int>(TimeSpan.FromHours(1), @"
SELECT EducationLevel
FROM   dbo.ApprenticeshipType
WHERE  ApprenticeshipTypeId = @ApprenticeshipTypeId",
                        new
                        {
                            ApprenticeshipTypeId = dbVacancy.ApprenticeshipType.Value
                        }).Single();
            }
        }

        private void MapIsEmployerLocationMainApprenticeshipLocation(Vacancy dbVacancy, DomainVacancy result)
        {
            var locationTypeCodeName = _getOpenConnection.QueryCached<string>(TimeSpan.FromHours(1), @"
SELECT CodeName
FROM   dbo.VacancyLocationType
WHERE  VacancyLocationTypeId = @VacancyLocationTypeId",
                new
                {
                    VacancyLocationTypeId = dbVacancy.VacancyLocationTypeId.Value
                }).Single();

            result.IsEmployerLocationMainApprenticeshipLocation = locationTypeCodeName != "MUL"; // Probably is not true
        }

        private void MapAdditionalQuestions(Vacancy dbVacancy, DomainVacancy result)
        {
            var results = _getOpenConnection.Query<dynamic>(@"
SELECT QuestionId, Question
FROM   dbo.AdditionalQuestion
WHERE  VacancyId = @VacancyId
ORDER BY QuestionId ASC
", new {VacancyId = dbVacancy.VacancyId});

            foreach (var question in results)
            {
                if (question.QuestionId == 1)
                {
                    result.FirstQuestion = question.Question;
                }
                if (question.QuestionId == 2)
                {
                    result.SecondQuestion = question.Question;
                }
            }
        }

        private void MapTextFields(Vacancy dbVacancy, DomainVacancy result)
        {
            result.TrainingProvided = GetTextField(dbVacancy.VacancyId, "TBP");
            result.DesiredQualifications = GetTextField(dbVacancy.VacancyId, "QR");
            result.DesiredSkills = GetTextField(dbVacancy.VacancyId, "SR");
            result.PersonalQualities = GetTextField(dbVacancy.VacancyId, "PQ");
            result.ThingsToConsider = GetTextField(dbVacancy.VacancyId, "OII");
            result.FutureProspects = GetTextField(dbVacancy.VacancyId, "FP");
        }

        private string GetTextField(int vacancyId, string vacancyTextFieldCodeName)
        {
            var vacancyTextFieldValueId =
                _getOpenConnection.Query<int>(
                    $@"
	SELECT TOP 1 VacancyTextFieldValueId FROM VacancyTextFieldValue
	WHERE CodeName = '{
                        vacancyTextFieldCodeName}'
").Single(); // TODO: Hardcode the ID?

            return _getOpenConnection.Query<string>(@"
SELECT Value
FROM   dbo.VacancyTextField
WHERE  VacancyId = @VacancyId AND Field = @Field
", new
            {
                VacancyId = vacancyId,
                Field = vacancyTextFieldValueId
            }).SingleOrDefault();

        }

        /*public List<DomainVacancy> GetForProvider(string ukPrn, string providerSiteErn)
        {
            const string parentVacancyStatusCode = "PAR";
            _logger.Debug(
                "Calling database to get apprenticeship vacancies with Vacancy UkPrn = {0}, providerSiteErn = {1}",
                ukPrn, providerSiteErn);

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
                    VacancyStatusCodes = new[] {parentVacancyStatusCode}
                });

            _logger.Debug(
                $"Found {dbVacancies.Count} apprenticeship vacancies with ukprn = {ukPrn}, providerSiteErn = {providerSiteErn}");

            return dbVacancies.Select(MapVacancy).ToList();
        }*/

        public List<DomainVacancy> GetWithStatus(params Domain.Entities.Raa.Vacancies.VacancyStatus[] desiredStatuses)
        {
            return new List<DomainVacancy>();

            //            Dapper.SqlMapper.AddTypeMap(typeof(string), System.Data.DbType.AnsiString);
            //            _logger.Debug("Called database to get apprenticeship vacancies in status {0}", string.Join(",", desiredStatuses));

            //            var statuses = desiredStatuses.Select(_mapper.Map<ProviderVacancyStatuses, string>).ToList();

            //            var dbVacancies = _getOpenConnection.Query<Repositories.Sql.Schemas.Vacancy.Entities.Vacancy>(@"
            //SELECT *
            //FROM   Vacancy.Vacancy
            //WHERE  Vacancy.VacancyStatusCode IN @VacancyStatusCodes", new
            //            {
            //                VacancyStatusCodes = statuses
            //            });

            //            // throw new NotSupportedException("This is likely to use excessive memory. Return type should be IEnumerable.");
            //            _logger.Debug(
            //                $"Found {dbVacancies.Count} apprenticeship vacancies with statuses in {string.Join(",", desiredStatuses)}");

            //            return dbVacancies.Select(MapVacancy).ToList();
        }

        public List<DomainVacancy> Find(ApprenticeshipVacancyQuery query, out int totalResultsCount)
        {
            _logger.Debug("Calling database to find apprenticeship vacancies");
            var liveStatus =
                _mapper.Map<Domain.Entities.Raa.Vacancies.VacancyStatus, string>(
                    Domain.Entities.Raa.Vacancies.VacancyStatus.Live);

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
" +
                            (string.IsNullOrWhiteSpace(query.FrameworkCodeName)
                                ? ""
                                : "AND Vacancy.FrameworkId = (SELECT FrameworkId FROM Reference.Framework where CodeName = @FrameworkCodeName) ") +
                            @"
" + (query.LiveDate.HasValue ? "AND Vacancy.PublishedDateTime >= @LiveDate" : "") + @" 
" + (query.LatestClosingDate.HasValue ? "AND Vacancy.ClosingDate <= @LatestClosingDate" : "");
                // Vacancy.PublishedDateTime >= @LiveDate was Vacancy.DateSubmitted >= @LiveDate

            // TODO: Vacancy.DateSubmitted should be DateLive (or DatePublished)???
            var data = _getOpenConnection.QueryMultiple<int, Repositories.Sql.Schemas.Vacancy.Entities.Vacancy>(@"
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

        public DomainVacancy Save(DomainVacancy entity)
        {
            _logger.Debug("Calling database to shallow save apprenticeship vacancy with id={0}", entity.VacancyId);

            UpdateEntityTimestamps(entity); // Do we need this?

            var dbVacancy = _mapper.Map<DomainVacancy, Vacancy>(entity);

            PopulateIds(entity, dbVacancy);

            dbVacancy.VacancyId = (int) _getOpenConnection.Insert(dbVacancy);

            SaveTextFieldsFor(dbVacancy.VacancyId, entity);
            SaveAdditionalQuestionsFor(dbVacancy.VacancyId, entity); // TODO: check if we need it

            _logger.Debug("Shallow saved apprenticeship vacancy with to database with id={0}", entity.VacancyId);

            return _mapper.Map<Vacancy, DomainVacancy>(dbVacancy);
        }

        public DomainVacancy Update(DomainVacancy entity)
        {
            _logger.Debug("Calling database to shallow save apprenticeship vacancy with id={0}", entity.VacancyId);

            UpdateEntityTimestamps(entity); // Do we need this?

            var dbVacancy = _mapper.Map<DomainVacancy, Vacancy>(entity);

            PopulateIds(entity, dbVacancy);

            _getOpenConnection.UpdateSingle(dbVacancy);

            SaveTextFieldsFor(dbVacancy.VacancyId, entity);
            SaveAdditionalQuestionsFor(dbVacancy.VacancyId, entity); // TODO: check if we need it

            _logger.Debug("Shallow saved apprenticeship vacancy with to database with id={0}", entity.VacancyId);

            return MapVacancy(dbVacancy);
        }

        public void Delete(int vacancyId)
        {
            throw new NotImplementedException();
        }

        public void IncrementOfflineApplicationClickThrough(int vacancyReferenceNumber)
        {
            throw new NotImplementedException();
        }

        private void PopulateIds(DomainVacancy entity, Vacancy dbVacancy)
        {
            PopulateCountyId(entity, dbVacancy);
            PopulateVacancyLocationTypeId(entity, dbVacancy);
            PopulateApprenticeshipTypeId(entity, dbVacancy);
            PopulateFrameworkId(entity, dbVacancy);
        }

        private void PopulateFrameworkId(DomainVacancy entity, Vacancy dbVacancy)
        {
            if (!string.IsNullOrWhiteSpace(entity.FrameworkCodeName))
            {
                dbVacancy.ApprenticeshipFrameworkId = _getOpenConnection.QueryCached<int>(TimeSpan.FromHours(1), @"
SELECT ApprenticeshipFrameworkId
FROM   dbo.ApprenticeshipFramework
WHERE  CodeName = @FrameworkCodeName",
                    new
                    {
                        FrameworkCodeName = entity.FrameworkCodeName
                    }).Single();
            }
        }

        private void PopulateCountyId(DomainVacancy entity, Vacancy dbVacancy)
        {
            if (entity.Address?.County != null)
            {
                dbVacancy.CountyId = _getOpenConnection.QueryCached<int>(TimeSpan.FromHours(1), @"
SELECT CountyId
FROM   dbo.County
WHERE  CodeName = @CountyCodeName",
                    new
                    {
                        CountyCodeName = entity.Address.County
                    }).Single(); 
            }
        }

        private void PopulateVacancyLocationTypeId(DomainVacancy entity, Vacancy dbVacancy)
        {
            // A vacancy is multilocation if IsEmployerAddressMainAddress is set to false
            string vacancyLocationTypeCodeName = null;

            if (entity.IsEmployerLocationMainApprenticeshipLocation.HasValue &&
                entity.IsEmployerLocationMainApprenticeshipLocation.Value == true)
            {
                vacancyLocationTypeCodeName = "STD";
            }
            else if (entity.IsEmployerLocationMainApprenticeshipLocation.HasValue &&
                     entity.IsEmployerLocationMainApprenticeshipLocation.Value == false)
            {
                vacancyLocationTypeCodeName = "MUL";
            }

            dbVacancy.VacancyLocationTypeId = _getOpenConnection.QueryCached<int>(TimeSpan.FromHours(1), @"
SELECT VacancyLocationTypeId
FROM   dbo.VacancyLocationType
WHERE  CodeName = @VacancyLocationTypeCodeName",
                new
                {
                    VacancyLocationTypeCodeName = vacancyLocationTypeCodeName
                }).Single();
        }

        private void PopulateApprenticeshipTypeId(DomainVacancy entity, Vacancy dbVacancy)
        {
            dbVacancy.ApprenticeshipType = _getOpenConnection.QueryCached<int>(TimeSpan.FromHours(1), @"
SELECT ApprenticeshipTypeId
FROM   dbo.ApprenticeshipType
WHERE  EducationLevel = @EducationLevel",
                new
                {
                    EducationLevel = entity.ApprenticeshipLevel
                }).Single(); // There's a better way to do this?
        }

        private void SaveTextFieldsFor(int vacancyId, DomainVacancy entity)
        {
            SaveTextField(vacancyId, "TBP", entity.TrainingProvided);
            SaveTextField(vacancyId, "QR", entity.DesiredQualifications);
            SaveTextField(vacancyId, "SR", entity.DesiredSkills);
            SaveTextField(vacancyId, "PQ", entity.PersonalQualities);
            SaveTextField(vacancyId, "OII", entity.ThingsToConsider);
            SaveTextField(vacancyId, "FP", entity.FutureProspects);
        }

        private void SaveTextField(int vacancyId, string vacancyTextFieldCodeName, string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                InsertTextField(vacancyId, vacancyTextFieldCodeName, value);
            }
            else
            {
                DeleteTextField(vacancyId, vacancyTextFieldCodeName);
            }
        }

        private void SaveAdditionalQuestionsFor(int vacancyId, DomainVacancy entity)
        {
            if (!string.IsNullOrWhiteSpace(entity.FirstQuestion))
            {
                UpsertAdditionalQuestion(vacancyId, 1, entity.FirstQuestion);
            }

            if (!string.IsNullOrWhiteSpace(entity.SecondQuestion))
            {
                UpsertAdditionalQuestion(vacancyId, 2, entity.SecondQuestion);
            }
        }

        private void UpsertAdditionalQuestion(int vacancyId, short questionId, string question)
        {
            var sql = @"
merge dbo.AdditionalQuestion as target
using (values (@Question))
    as source (Question)
    on target.VacancyId = @VacancyId and QuestionId = @QuestionId
when matched then
    update
    set Question = source.Question
when not matched then
    insert ( VacancyId, QuestionId, Question )
    values ( @VacancyId, @QuestionId, @Question );";

            _getOpenConnection.MutatingQuery<object>(sql, new
            {
                VacancyId = vacancyId,
                QuestionId = questionId,
                Question = question
            });
        }

        private void DeleteTextField(int vacancyId, string vacancyTextFieldCodeName)
        {
            var vacancyTextFieldValueId =
                _getOpenConnection.Query<int>(
                    $@"
	SELECT TOP 1 VacancyTextFieldValueId FROM VacancyTextFieldValue
	WHERE CodeName = '{
                        vacancyTextFieldCodeName}'
").Single(); // TODO: Hardcode the ID?

            var sql = @"
    DELETE from dbo.VacancyTextField
    WHERE VacancyId = @VacancyId and Field = @FieldId";

            _getOpenConnection.MutatingQuery<object>(sql, new
            {
                VacancyId = vacancyId,
                FieldId = vacancyTextFieldValueId
            });
        }

        private void InsertTextField(int vacancyId, string vacancyTextFieldCodeName, string value)
        {
            var vacancyTextFieldValueId =
                _getOpenConnection.Query<int>(
                    $@"
	SELECT TOP 1 VacancyTextFieldValueId FROM VacancyTextFieldValue
	WHERE CodeName = '{
                        vacancyTextFieldCodeName}'
").Single(); // TODO: Hardcode the ID?

            var sql = @"
merge dbo.VacancyTextField as target
using (values (@Value))
    as source (Value)
    on target.VacancyId = @VacancyId and Field = @FieldId
when matched then
    update
    set Value = source.Value
when not matched then
    insert ( VacancyId, Field, Value)
    values ( @VacancyId, @FieldId, @Value);";

            _getOpenConnection.MutatingQuery<object>(sql, new
            {
                VacancyId = vacancyId,
                FieldId = vacancyTextFieldValueId,
                Value = value
            });
        }

        /*private void UpdateTextFieldsFor(int vacancyId, ApprenticeshipVacancy entity)
        {
            if (!string.IsNullOrWhiteSpace(entity.TrainingProvided))
            {
                UpdateTextField(vacancyId, "TBP", entity.TrainingProvided);
            }
            else
            {
                DeleteTextField(vacancyId, "TBP");
            }
            
            UpdateTextField(vacancyId, "QR", entity.DesiredQualifications);
            UpdateTextField(vacancyId, "SR", entity.DesiredSkills);
            UpdateTextField(vacancyId, "PQ", entity.PersonalQualities);
            UpdateTextField(vacancyId, "OII", entity.ThingsToConsider);
            UpdateTextField(vacancyId, "FP", entity.FutureProspects);
        }

        private void UpdateTextField(int vacancyId, string vacancyTextFieldCodeName, string value)
        {
            var vacancyTextFieldValueId = _getOpenConnection.Query<int>($@"
	            SELECT TOP 1 VacancyTextFieldValueId FROM VacancyTextFieldValue
	            WHERE CodeName = '{vacancyTextFieldCodeName}'
            ").Single(); // TODO: Hardcode the ID?

            _getOpenConnection.MutatingQuery<VacancyTextField>(@"
UPDATE [dbo].[VacancyTextField] 
SET Value = @Value
WHERE VacancyId = @VacancyId AND Field = @FieldId

SELECT * 
FROM [dbo].[VacancyTextField] 
WHERE VacancyId = @VacancyId AND Field = @FieldId",
                new
                {
                    VacancyId = vacancyId,
                    FieldId = vacancyTextFieldValueId,
                    Value = value
                });
        }
        */
        /*
        private int PopulateVacancyPartyIds(DomainVacancy vacancy, Entities.Vacancy dbVacancy)
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
", new { vacancy.FrameworkCodeName });

            //if (framework.Any())
            //{
            //    dbVacancy.FrameworkId = framework.First();
            //}
            //else
            //{
            //    dbVacancy.FrameworkId = null;
            //}
        }*/

        public DomainVacancy ReserveVacancyForQA(int vacancyReferenceNumber)
        {
            _logger.Debug(
                $"Calling database to get and reserve vacancy with reference number: {vacancyReferenceNumber} for QA");

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
", new {UserName = userName, TimeStartedToQA = DateTime.UtcNow, VacancyReferenceNumber = vacancyReferenceNumber})
                .SingleOrDefault();

            if (dbVacancy == null)
            {
                _logger.Warn(
                    $"Call to database to get and reserve vacancy with reference number: {vacancyReferenceNumber} for QA failed");
                return null;
            }

            _logger.Info(
                $"Called database to get and reserve vacancy with reference number: {vacancyReferenceNumber} for QA successfully");

            return MapVacancy(dbVacancy);
        }

        private void UpdateEntityTimestamps(DomainVacancy entity)
        {
            // determine whether this is a "new" entity being saved for the first time
            if (entity.CreatedDateTime == DateTime.MinValue)
            {
                entity.CreatedDateTime = DateTime.UtcNow;
                entity.UpdatedDateTime = null;
            }
            else
            {
                entity.UpdatedDateTime = DateTime.UtcNow;
            }
        }
    }
}