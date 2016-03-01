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
    using Entities;
    using SFA.Infrastructure.Interfaces;
    using Vacancy = Entities.Vacancy;
    using VacancyStatus = Domain.Entities.Raa.Vacancies.VacancyStatus;
    using VacancyType = Domain.Entities.Raa.Vacancies.VacancyType;

    public class VacancyRepository : IVacancyReadRepository, IVacancyWriteRepository
    {
        private const int TraineeshipFrameworkId = 999;
        private readonly IMapper _mapper;
        private readonly IDateTimeService _dateTimeService;
        private readonly ILogService _logger;

        private readonly IGetOpenConnection _getOpenConnection;

        public VacancyRepository(IGetOpenConnection getOpenConnection, IMapper mapper, IDateTimeService dateTimeService, ILogService logger) // Use IDateTimeService
        {
            _getOpenConnection = getOpenConnection;
            _mapper = mapper;
            _dateTimeService = dateTimeService;
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

            var dbVacancy = GetVacancyByVacancyGuid(vacancyGuid);

            return MapVacancy(dbVacancy);
        }

        private Vacancy GetVacancyByVacancyGuid(Guid vacancyGuid)
        {
            var dbVacancy = _getOpenConnection.Query<Vacancy>(
                "SELECT * FROM dbo.Vacancy WHERE VacancyGuid = @VacancyGuid",
                new {VacancyGuid = vacancyGuid}).SingleOrDefault();
            return dbVacancy;
        }


        public List<DomainVacancy> GetByIds(IEnumerable<int> vacancyIds)
        {
            var vacancyIdsArray
                = vacancyIds as int[] ?? vacancyIds.ToArray();
            _logger.Debug("Calling database to get apprenticeship vacancy with Ids={0}", string.Join(", ", vacancyIdsArray));

            var vacancies =
                _getOpenConnection.Query<Vacancy>("SELECT * FROM dbo.Vacancy WHERE VacancyId IN @VacancyIds",
                    new { VacancyIds = vacancyIdsArray });

            return vacancies.Select(MapVacancy).ToList();
        }

        public List<DomainVacancy> GetByOwnerPartyIds(IEnumerable<int> ownerPartyIds)
        {
            var ownerPartyIdsArray = ownerPartyIds as int[] ?? ownerPartyIds.ToArray();
            _logger.Debug("Calling database to get apprenticeship vacancy with VacancyOwnerRelationshipId={0}", string.Join(", ", ownerPartyIdsArray));

            var vacancies =
                _getOpenConnection.Query<Vacancy>("SELECT * FROM dbo.Vacancy WHERE VacancyOwnerRelationshipId IN @VacancyOwnerRelationshipIds",
                    new { VacancyOwnerRelationshipIds = ownerPartyIdsArray });

            return vacancies.Select(MapVacancy).ToList();
        }

        private DomainVacancy MapVacancy(Vacancy dbVacancy)
        {
            if (dbVacancy == null)
                return null;
            
            var result = _mapper.Map<Vacancy, DomainVacancy>(dbVacancy);
            MapAdditionalQuestions(dbVacancy, result);
            MapTextFields(dbVacancy, result);
            MapIsEmployerLocationMainApprenticeshipLocation(dbVacancy, result);
            MapApprenticeshipType(dbVacancy, result);
            MapFrameworkId(dbVacancy, result);
            MapSectorId(dbVacancy, result);

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
                var educationLevelCodeName =
                    _getOpenConnection.QueryCached<string>(TimeSpan.FromHours(1), @"
SELECT el.CodeName
FROM   Reference.EducationLevel as el JOIN dbo.ApprenticeshipType as at ON el.EducationLevelId = at.EducationLevelId
WHERE  at.ApprenticeshipTypeId = @ApprenticeshipTypeId",
                        new
                        {
                            ApprenticeshipTypeId = dbVacancy.ApprenticeshipType.Value
                        }).Single();

                result.ApprenticeshipLevel =
                    (ApprenticeshipLevel) Enum.Parse(typeof (ApprenticeshipLevel), educationLevelCodeName);
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

        private void MapSectorId(Vacancy dbVacancy, DomainVacancy result)
        {
            if (dbVacancy.SectorId.HasValue)
            {
                result.SectorCodeName = _getOpenConnection.QueryCached<string>(TimeSpan.FromHours(1), @"
SELECT CodeName
FROM   dbo.ApprenticeshipOccupation
WHERE  ApprenticeshipOccupationId = @ApprenticeshipOccupationId",
                    new
                    {
                        ApprenticeshipOccupationId = dbVacancy.SectorId
                    }).Single();
            }
            else
            {
                result.SectorCodeName = null;
            }
        }

        private void MapAdditionalQuestions(Vacancy dbVacancy, DomainVacancy result)
        {
            var results = _getOpenConnection.Query<dynamic>(@"
SELECT QuestionId, Question
FROM   dbo.AdditionalQuestion
WHERE  VacancyId = @VacancyId
ORDER BY QuestionId ASC
", new {dbVacancy.VacancyId});

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
        
        public List<DomainVacancy> GetWithStatus(params VacancyStatus[] desiredStatuses)
        {
            _logger.Debug("Called database to get apprenticeship vacancies in status {0}", string.Join(",", desiredStatuses));

            var dbVacancies = _getOpenConnection.Query<Vacancy>(@"
            SELECT *
            FROM   dbo.Vacancy
            WHERE  VacancyStatusId IN @VacancyStatusCodeIds", new
            {
                VacancyStatusCodeIds = desiredStatuses.Select(s => (int)s)
            });


            _logger.Debug(
                $"Found {dbVacancies.Count} apprenticeship vacancies with statuses in {string.Join(",", desiredStatuses)}");

            return dbVacancies.Select(MapVacancy).ToList();
        }

        public List<DomainVacancy> Find(ApprenticeshipVacancyQuery query, out int totalResultsCount)
        {
            _logger.Debug("Calling database to find apprenticeship vacancies");
            var liveStatus =
                _mapper.Map<VacancyStatus, string>(VacancyStatus.Live);

            var paramObject =
                new
                {
                    query.FrameworkCodeName,
                    query.LiveDate,
                    query.LatestClosingDate,
                    VacancyStatusCode = liveStatus,
                    query.CurrentPage,
                    query.PageSize
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
            var data = _getOpenConnection.QueryMultiple<int, Vacancy>(@"
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

            CreateVacancyHistoryRow(dbVacancy.VacancyId, GetUserName(), VacancyHistoryEventType.StatusChange,
                (int)entity.Status);

            _logger.Debug("Shallow saved apprenticeship vacancy with to database with id={0}", entity.VacancyId);

            return _mapper.Map<Vacancy, DomainVacancy>(dbVacancy);
        }

        

        public DomainVacancy Update(DomainVacancy entity)
        {
            _logger.Debug("Calling database to shallow save apprenticeship vacancy with id={0}", entity.VacancyId);

            UpdateEntityTimestamps(entity); // Do we need this?

            var dbVacancy = _mapper.Map<DomainVacancy, Vacancy>(entity);

            PopulateIds(entity, dbVacancy);

            var previousVacancyState = GetVacancyByVacancyGuid(entity.VacancyGuid);

            _getOpenConnection.UpdateSingle(dbVacancy);

            SaveTextFieldsFor(dbVacancy.VacancyId, entity);
            SaveAdditionalQuestionsFor(dbVacancy.VacancyId, entity); // TODO: check if we need it

            UpdateVacancyHistory(previousVacancyState, dbVacancy);

            _logger.Debug("Shallow saved apprenticeship vacancy with to database with id={0}", entity.VacancyId);

            return MapVacancy(dbVacancy);
        }

        private void UpdateVacancyHistory(Vacancy previousVacancyState, Vacancy actualVacancyState)
        {
            if (previousVacancyState.VacancyStatusId != actualVacancyState.VacancyStatusId)
            {
                CreateVacancyHistoryRow(actualVacancyState.VacancyId, GetUserName(), VacancyHistoryEventType.StatusChange, actualVacancyState.VacancyStatusId);
            }
        }

        private void CreateVacancyHistoryRow(int vacancyId, string userName, VacancyHistoryEventType vacancyHistoryEventType, int vacancyStatus)
        {
            var vacancyHistory = new VacancyHistory
            {
                VacancyId = vacancyId,
                Comment = "Status Change",
                UserName = userName,
                VacancyHistoryEventTypeId = (int) vacancyHistoryEventType,
                VacancyHistoryEventSubTypeId = vacancyStatus,
                HistoryDate = _dateTimeService.UtcNow
            };

            _getOpenConnection.Insert(vacancyHistory);
        }

        private static string GetUserName()
        {
            return Thread.CurrentPrincipal.Identity.Name;
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
            PopulateSectorId(entity, dbVacancy);
        }

        private void PopulateSectorId(DomainVacancy entity, Vacancy dbVacancy)
        {
            if (!string.IsNullOrWhiteSpace(entity.SectorCodeName))
            {
                dbVacancy.SectorId = _getOpenConnection.QueryCached<int>(TimeSpan.FromHours(1), @"
SELECT ApprenticeshipOccupationId
FROM   dbo.ApprenticeshipOccupation
WHERE  CodeName = @SectorCodeName",
                    new
                    {
                        entity.SectorCodeName
                    }).Single();
            }
            else
            {
                dbVacancy.SectorId = null;
            }
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
                        entity.FrameworkCodeName
                    }).Single();
            }
            else
            {
                dbVacancy.ApprenticeshipFrameworkId = null;
            }

            if (entity.VacancyType == VacancyType.Traineeship)
            {
                dbVacancy.ApprenticeshipFrameworkId = TraineeshipFrameworkId;
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
FROM   dbo.ApprenticeshipType at JOIN Reference.EducationLevel el ON at.EducationLevelId = el.EducationLevelId
WHERE  el.CodeName = @EducationLevel",
                new
                {
                    EducationLevel = (int)entity.ApprenticeshipLevel
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
", new {UserName = userName, TimeStartedToQA = _dateTimeService.UtcNow, VacancyReferenceNumber = vacancyReferenceNumber})
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
            if (entity.CreatedDateTime == _dateTimeService.MinValue)
            {
                entity.CreatedDateTime = _dateTimeService.UtcNow;
                entity.UpdatedDateTime = null;
            }
            else
            {
                entity.UpdatedDateTime = _dateTimeService.UtcNow;
            }
        }
    }
}