namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Vacancy
{
    using Application.Interfaces;
    using Common;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Raa.Interfaces.Repositories;
    using Entities;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DomainVacancy = Domain.Entities.Raa.Vacancies.Vacancy;
    using Vacancy = Entities.Vacancy;
    using VacancyStatus = Domain.Entities.Raa.Vacancies.VacancyStatus;
    using VacancyType = Domain.Entities.Raa.Vacancies.VacancyType;

    public class VacancyRepository : IVacancyReadRepository, IVacancyWriteRepository
    {
        private const int TraineeshipFrameworkId = 999;
        private readonly IMapper _mapper;
        private readonly IDateTimeService _dateTimeService;
        private readonly ILogService _logger;
        private readonly ICurrentUserService _currentUserService;

        private readonly IGetOpenConnection _getOpenConnection;
        private readonly TimeSpan _cacheDuration = TimeSpan.FromHours(1);

        private const string StatusChangeText = "Status Change";

        private static readonly IDictionary<string, int> VacancyTextFieldValues = new Dictionary<string, int>
        {
            {"TBP", 1},
            {"QR", 2},
            {"SR", 3},
            {"PQ", 4},
            {"OII", 5},
            {"FP", 6},
            {"RC", 7}
        };

        private static readonly IDictionary<string, int> VacancyReferalCommentsFieldType = new Dictionary<string, int>
        {
            {"TIT", 1},
            {"SDE", 2},
            {"FDE", 3},
            {"WWK", 4},
            {"WWG", 5},
            {"FUT", 6},
            {"CFS", 7},
            {"EAN", 8},
            {"EDE", 9},
            {"EWB", 10},
            {"APO", 11},
            {"APF", 12},
            {"VTP", 13},
            {"TRP", 14},
            {"EAD", 15},
            {"SKL", 16},
            {"QUA", 17},
            {"PER", 18},
            {"REA", 19},
            {"IOI", 20},
            {"QU1", 21},
            {"QU2", 22},
            {"CLD", 23},
            {"ISF", 24},
            {"PSD", 25},
            {"EAI", 26},
            {"AWA", 27},
            {"DRA", 28},
            {"NPO", 29},
            {"OAI", 30},
            {"OAU", 31},
            {"SID", 32},
            {"ALE", 33},
            {"CDE", 34},
            {"LAD", 35},
            {"ALI", 36},
            {"AED", 37},
            {"AER", 38},
            {"AAE", 39},
            {"OIN", 40}
        };

        public VacancyRepository(IGetOpenConnection getOpenConnection, IMapper mapper, IDateTimeService dateTimeService,
            ILogService logger, ICurrentUserService currentUserService)
        {
            _getOpenConnection = getOpenConnection;
            _mapper = mapper;
            _dateTimeService = dateTimeService;
            _logger = logger;
            _currentUserService = currentUserService;
        }

        private DomainVacancy GetByMapped(int vacancyId)
        {
            var vacancy = GetBy(vacancyId);

            var mapped = _mapper.Map<Vacancy, DomainVacancy>(vacancy);

            PatchTrainingType(mapped);

            return mapped;
        }

        private Vacancy GetBy(int vacancyId)
        {
            var sqlParams = new
            {
                vacancyId
            };

            var vacancy = _getOpenConnection.Query<Vacancy>(SelectByIdSql, sqlParams).SingleOrDefault();

            return vacancy;
        }

        public DomainVacancy Get(int vacancyId)
        {
            return GetByMapped(vacancyId);
        }

        public DomainVacancy GetByReferenceNumber(int vacancyReferenceNumber)
        {
            _logger.Debug("Calling database to get apprenticeship vacancy with Vacancy Reference Number={0}",
                vacancyReferenceNumber);

            var vacancyId = _getOpenConnection.QueryCached<int?>(_cacheDuration, SelectVacancyIdFromReferenceNumberSql,
                    new
                    {
                        vacancyReferenceNumber
                    }).SingleOrDefault();

            return vacancyId == null ? null : Get(vacancyId.Value);
        }

        public DomainVacancy GetByVacancyGuid(Guid vacancyGuid)
        {
            _logger.Debug("Calling database to get apprenticeship vacancy with VacancyGuid={0}",
                vacancyGuid);

            var vacancyId = _getOpenConnection.QueryCached<int?>(_cacheDuration, SelectVacancyIdFromGuidSql,
                    new
                    {
                        vacancyGuid
                    }).SingleOrDefault();

            return vacancyId == null ? null : Get(vacancyId.Value);
        }

        public int CountWithStatus(params VacancyStatus[] desiredStatuses)
        {
            _logger.Debug("Called database to count apprenticeship vacancies in status {0}", string.Join(",", desiredStatuses));

            var count = _getOpenConnection.Query<int>(
            @"SELECT Count(*)
            FROM   dbo.Vacancy
            WHERE  VacancyStatusId IN @VacancyStatusCodeIds", new
            {
                VacancyStatusCodeIds = desiredStatuses.Select(s => (int)s)
            }).Single();


            _logger.Debug(
                $"Found {count} apprenticeship vacancies with statuses in {string.Join(",", desiredStatuses)}");

            return count;
        }

        private void PatchTrainingType(DomainVacancy result)
        {
            if (result?.TrainingType != TrainingType.Unknown) return;

            if (!string.IsNullOrWhiteSpace(result.SectorCodeName))
            {
                result.TrainingType = TrainingType.Sectors;
            }
            else if (result?.StandardId != null)
            {
                result.TrainingType = TrainingType.Standards;
            }
            else if (!string.IsNullOrWhiteSpace(result.FrameworkCodeName))
            {
                result.TrainingType = TrainingType.Frameworks;
            }
        }

        public DomainVacancy Create(DomainVacancy entity)
        {
            _logger.Debug("Calling database to save apprenticeship vacancy with id={0}", entity.VacancyId);

            _logger.Info(
                $"[{entity.VacancyGuid}] Calling database to create the following domain vacancy: {JsonConvert.SerializeObject(entity, Formatting.Indented, new JsonSerializerSettings { ContractResolver = new ExcludeLiveClosingDateResolver() })}");

            UpdateEntityTimestamps(entity);

            var dbVacancy = _mapper.Map<DomainVacancy, Vacancy>(entity);

            PopulateIds(entity, dbVacancy);

            _logger.Info(
                $"[{entity.VacancyGuid}] Calling database to create the following database vacancy: {JsonConvert.SerializeObject(dbVacancy, Formatting.Indented, new JsonSerializerSettings { ContractResolver = new ExcludeLiveClosingDateResolver() })}");

            dbVacancy.VacancyId = (int)_getOpenConnection.Insert(dbVacancy);

            SaveTextFieldsFor(dbVacancy.VacancyId, entity);
            SaveAdditionalQuestionsFor(dbVacancy.VacancyId, entity);

            CreateVacancyHistoryRow(dbVacancy.VacancyId, _currentUserService.CurrentUserName, VacancyHistoryEventType.StatusChange,
                (int)entity.Status, StatusChangeText);

            _logger.Debug("Saved apprenticeship vacancy to database with id={0}", entity.VacancyId);

            return _mapper.Map<Vacancy, DomainVacancy>(dbVacancy);
        }

        public DomainVacancy Update(DomainVacancy entity)
        {
            _logger.Debug("Calling database to update apprenticeship vacancy with id={0}", entity.VacancyId);

            _logger.Info(
                $"[{entity.VacancyGuid}] Calling database to update the following vacancy: {JsonConvert.SerializeObject(entity, Formatting.Indented, new JsonSerializerSettings { ContractResolver = new ExcludeLiveClosingDateResolver() })}");

            UpdateEntityTimestamps(entity); // Do we need this?

            var dbVacancy = _mapper.Map<DomainVacancy, Vacancy>(entity);

            PopulateIds(entity, dbVacancy);

            var previousVacancyState = GetBy(entity.VacancyId);

            _logger.Info(
                $"[{entity.VacancyGuid}] Calling database to update the following vacancy: {JsonConvert.SerializeObject(dbVacancy, Formatting.Indented, new JsonSerializerSettings { ContractResolver = new ExcludeLiveClosingDateResolver() })}");

            _getOpenConnection.UpdateSingle(dbVacancy);

            SaveTextFieldsFor(dbVacancy.VacancyId, entity);
            SaveAdditionalQuestionsFor(dbVacancy.VacancyId, entity);
            SaveCommentsFor(dbVacancy.VacancyId, entity);

            UpdateVacancyHistory(previousVacancyState, dbVacancy);

            _logger.Debug("Updated apprenticeship vacancy with to database with id={0}", entity.VacancyId);

            return GetByMapped(entity.VacancyId);
        }

        private void UpdateVacancyHistory(Vacancy previousVacancyState, Vacancy actualVacancyState)
        {
            if (StatusHasChanged(previousVacancyState, actualVacancyState))
            {
                CreateVacancyHistoryRow(actualVacancyState.VacancyId, _currentUserService.CurrentUserName,
                    VacancyHistoryEventType.StatusChange, actualVacancyState.VacancyStatusId, StatusChangeText);
            }

            if (ClosingDateHasBeenUpdated(previousVacancyState, actualVacancyState))
            {
                CreateVacancyHistoryRow(actualVacancyState.VacancyId, _currentUserService.CurrentUserName,
                    VacancyHistoryEventType.StatusChange, actualVacancyState.VacancyStatusId, StatusChangeText);

                CreateVacancyHistoryRow(actualVacancyState.VacancyId, _currentUserService.CurrentUserName,
                    VacancyHistoryEventType.Note, actualVacancyState.VacancyStatusId, previousVacancyState.ApplicationClosingDate.Value.ToString("yyyy-MM-dd"));
            }
        }

        private static bool StatusHasChanged(Vacancy previousVacancy, Vacancy actualVacancy)
        {
            return previousVacancy.VacancyStatusId != actualVacancy.VacancyStatusId;
        }

        private static bool ClosingDateHasBeenUpdated(Vacancy previousVacancy, Vacancy actualVacancy)
        {
            return previousVacancy.VacancyStatusId == (int)VacancyStatus.Live &&
                actualVacancy.VacancyStatusId == (int)VacancyStatus.Live &&
                previousVacancy.ApplicationClosingDate != actualVacancy.ApplicationClosingDate;
        }

        private void CreateVacancyHistoryRow(int vacancyId, string userName,
            VacancyHistoryEventType vacancyHistoryEventType, int vacancyStatus, string comment)
        {
            var vacancyHistory = new VacancyHistory
            {
                VacancyId = vacancyId,
                Comment = comment,
                UserName = userName,
                VacancyHistoryEventTypeId = (int)vacancyHistoryEventType,
                VacancyHistoryEventSubTypeId = vacancyStatus,
                HistoryDate = _dateTimeService.UtcNow
            };

            _getOpenConnection.Insert(vacancyHistory);
        }

        public void Delete(int vacancyId)
        {
            _getOpenConnection.MutatingQuery<int>(@"
                UPDATE dbo.Vacancy
                SET VacancyStatusId = @VacancyStatus
                WHERE VacancyId = @VacancyId",
                new
                {
                    VacancyId = vacancyId,
                    VacancyStatus = VacancyStatus.Deleted
                });
        }

        public void IncrementOfflineApplicationClickThrough(int vacancyId)
        {
            _getOpenConnection.MutatingQuery<object>(
                    $@"UPDATE dbo.Vacancy 
SET NoOfOfflineApplicants = NoOfOfflineApplicants + 1
WHERE VacancyId = @vacancyId and NoOfOfflineApplicants is not null

UPDATE dbo.Vacancy 
SET NoOfOfflineApplicants = 1
WHERE VacancyId = @vacancyId and NoOfOfflineApplicants is null
", new
                    {
                        VacancyId = vacancyId
                    });
        }

        private void PopulateIds(DomainVacancy entity, Vacancy dbVacancy)
        {
            PopulateCountyId(entity, dbVacancy);
            PopulateVacancyLocationTypeId(entity, dbVacancy);
            PopulateApprenticeshipTypeId(entity, dbVacancy);
            PopulateFrameworkId(entity, dbVacancy);
            PopulateSectorId(entity, dbVacancy);
            PopulateLocalAuthorityId(entity, dbVacancy);
        }

        private void PopulateLocalAuthorityId(DomainVacancy entity, Vacancy dbVacancy)
        {
            if (!string.IsNullOrWhiteSpace(entity.LocalAuthorityCode))
            {
                dbVacancy.LocalAuthorityId = _getOpenConnection.QueryCached<int>(_cacheDuration, @"
SELECT LocalAuthorityId
FROM   dbo.LocalAuthority
WHERE  CodeName LIKE '%' + @LocalAuthorityCode",
                    new
                    {
                        entity.LocalAuthorityCode
                    }).Single();
            }
            else
            {
                dbVacancy.LocalAuthorityId = null;
            }
        }

        private void PopulateSectorId(DomainVacancy entity, Vacancy dbVacancy)
        {
            if (!string.IsNullOrWhiteSpace(entity.SectorCodeName))
            {
                dbVacancy.SectorId = _getOpenConnection.QueryCached<int>(_cacheDuration, @"
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
                dbVacancy.ApprenticeshipFrameworkId = _getOpenConnection.QueryCached<int>(_cacheDuration, @"
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
            if (!string.IsNullOrWhiteSpace(entity.Address?.County))
            {
                dbVacancy.CountyId = _getOpenConnection.QueryCached<int>(_cacheDuration, @"
SELECT CountyId
FROM   dbo.County
WHERE  FullName = @CountyFullName",
                    new
                    {
                        CountyFullName = entity.Address.County
                    }).SingleOrDefault();
            }
        }

        private static void PopulateVacancyLocationTypeId(DomainVacancy entity, Vacancy dbVacancy)
        {
            // A vacancy is multilocation if IsEmployerAddressMainAddress is set to false
            if (entity.VacancyLocationType == VacancyLocationType.SpecificLocation)
                dbVacancy.VacancyLocationTypeId = 1;
            else if (entity.VacancyLocationType == VacancyLocationType.MultipleLocations)
                dbVacancy.VacancyLocationTypeId = 2;
            else if (entity.VacancyLocationType == VacancyLocationType.Nationwide)
                dbVacancy.VacancyLocationTypeId = 3;
        }

        private void PopulateApprenticeshipTypeId(DomainVacancy entity, Vacancy dbVacancy)
        {
            if (entity.VacancyType == VacancyType.Traineeship)
            {
                dbVacancy.ApprenticeshipType = 4;
                return;
            }

            switch (entity.ApprenticeshipLevel)
            {
                case ApprenticeshipLevel.Unknown:
                    dbVacancy.ApprenticeshipType = 0;
                    return;
                case ApprenticeshipLevel.Intermediate:
                    dbVacancy.ApprenticeshipType = 1;
                    return;
                case ApprenticeshipLevel.Advanced:
                    dbVacancy.ApprenticeshipType = 2;
                    return;
                case ApprenticeshipLevel.Higher:
                    dbVacancy.ApprenticeshipType = 3;
                    return;
                case ApprenticeshipLevel.FoundationDegree:
                    dbVacancy.ApprenticeshipType = 5;
                    return;
                case ApprenticeshipLevel.Degree:
                    dbVacancy.ApprenticeshipType = 6;
                    return;
                case ApprenticeshipLevel.Masters:
                    dbVacancy.ApprenticeshipType = 7;
                    return;
            }
        }

        private void SaveTextFieldsFor(int vacancyId, DomainVacancy entity)
        {
            SaveTextField(vacancyId, TextFieldCodeName.TrainingProvided, entity.TrainingProvided);
            SaveTextField(vacancyId, TextFieldCodeName.DesiredQualifications, entity.DesiredQualifications);
            SaveTextField(vacancyId, TextFieldCodeName.DesiredSkills, entity.DesiredSkills);
            SaveTextField(vacancyId, TextFieldCodeName.PersonalQualities, entity.PersonalQualities);
            SaveTextField(vacancyId, TextFieldCodeName.ThingsToConsider, entity.ThingsToConsider);
            SaveTextField(vacancyId, TextFieldCodeName.FutureProspects, entity.FutureProspects);
            SaveTextField(vacancyId, TextFieldCodeName.OtherInformation, entity.OtherInformation);
        }

        private void SaveTextField(int vacancyId, string vacancyTextFieldCodeName, string value)
        {
            UpsertTextField(vacancyId, vacancyTextFieldCodeName, value);
        }

        private void SaveAdditionalQuestionsFor(int vacancyId, DomainVacancy entity)
        {
            UpsertAdditionalQuestion(vacancyId, 1, entity.FirstQuestion);
            UpsertAdditionalQuestion(vacancyId, 2, entity.SecondQuestion);
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
                Question = question ?? string.Empty
            });
        }

        private void UpsertTextField(int vacancyId, string vacancyTextFieldCodeName, string value)
        {
            if (!VacancyTextFieldValues.ContainsKey(vacancyTextFieldCodeName))
            {
                throw new ArgumentException($"{vacancyTextFieldCodeName} was not recognised as a valid vacancy text field code name");
            }
            var vacancyTextFieldValueId = VacancyTextFieldValues[vacancyTextFieldCodeName];

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

        private void SaveCommentsFor(int vacancyId, DomainVacancy entity)
        {
            SaveComment(vacancyId, ReferralCommentCodeName.TitleComment, entity.TitleComment);
            SaveComment(vacancyId, ReferralCommentCodeName.ApprenticeshipLevelComment, entity.ApprenticeshipLevelComment);
            SaveComment(vacancyId, ReferralCommentCodeName.ClosingDateComment, entity.ClosingDateComment);
            SaveComment(vacancyId, ReferralCommentCodeName.ContactDetailsComment, entity.ContactDetailsComment); // Maybe CFS
            SaveComment(vacancyId, ReferralCommentCodeName.DesiredQualificationsComment, entity.DesiredQualificationsComment);
            SaveComment(vacancyId, ReferralCommentCodeName.DesiredSkillsComment, entity.DesiredSkillsComment);
            SaveComment(vacancyId, ReferralCommentCodeName.DurationComment, entity.DurationComment);
            SaveComment(vacancyId, ReferralCommentCodeName.EmployerDescriptionComment, entity.EmployerDescriptionComment);
            SaveComment(vacancyId, ReferralCommentCodeName.EmployerWebsiteUrlComment, entity.EmployerWebsiteUrlComment);
            SaveComment(vacancyId, ReferralCommentCodeName.FirstQuestionComment, entity.FirstQuestionComment);
            SaveComment(vacancyId, ReferralCommentCodeName.SecondQuestionComment, entity.SecondQuestionComment);
            SaveComment(vacancyId, ReferralCommentCodeName.FrameworkCodeNameComment, entity.FrameworkCodeNameComment);
            SaveComment(vacancyId, ReferralCommentCodeName.FutureProspectsComment, entity.FutureProspectsComment);
            SaveComment(vacancyId, ReferralCommentCodeName.LongDescriptionComment, entity.LongDescriptionComment);
            SaveComment(vacancyId, ReferralCommentCodeName.NumberOfPositionsComment, entity.NumberOfPositionsComment);
            SaveComment(vacancyId, ReferralCommentCodeName.OfflineApplicationInstructionsComment, entity.OfflineApplicationInstructionsComment);
            SaveComment(vacancyId, ReferralCommentCodeName.OfflineApplicationUrlComment, entity.OfflineApplicationUrlComment);
            SaveComment(vacancyId, ReferralCommentCodeName.PersonalQualitiesComment, entity.PersonalQualitiesComment);
            SaveComment(vacancyId, ReferralCommentCodeName.PossibleStartDateComment, entity.PossibleStartDateComment);
            SaveComment(vacancyId, ReferralCommentCodeName.SectorCodeNameComment, entity.SectorCodeNameComment); // Or needs a new one?
            SaveComment(vacancyId, ReferralCommentCodeName.ShortDescriptionComment, entity.ShortDescriptionComment);
            SaveComment(vacancyId, ReferralCommentCodeName.StandardIdComment, entity.StandardIdComment);
            SaveComment(vacancyId, ReferralCommentCodeName.ThingsToConsiderComment, entity.ThingsToConsiderComment);
            SaveComment(vacancyId, ReferralCommentCodeName.TrainingProvidedComment, entity.TrainingProvidedComment);
            SaveComment(vacancyId, ReferralCommentCodeName.WageComment, entity.WageComment);
            SaveComment(vacancyId, ReferralCommentCodeName.WorkingWeekComment, entity.WorkingWeekComment);
            SaveComment(vacancyId, ReferralCommentCodeName.LocationAddressesComment, entity.LocationAddressesComment);
            SaveComment(vacancyId, ReferralCommentCodeName.AdditionalLocationInformationComment, entity.AdditionalLocationInformationComment);
            SaveComment(vacancyId, ReferralCommentCodeName.AnonymousEmployerReasonComment, entity.AnonymousEmployerReasonComment);
            SaveComment(vacancyId, ReferralCommentCodeName.AnonymousAboutTheEmployerComment, entity.AnonymousAboutTheEmployerComment);
            SaveComment(vacancyId, ReferralCommentCodeName.AnonymousEmployerDescriptionComment, entity.AnonymousEmployerDescriptionComment);
            SaveComment(vacancyId, ReferralCommentCodeName.OtherInformationComment, entity.OtherInformationComment);
        }

        private void SaveComment(int vacancyId, string vacancyReferralCommentsFieldTypeCodeName, string comment)
        {
            if (!string.IsNullOrWhiteSpace(comment))
            {
                UpsertComment(vacancyId, vacancyReferralCommentsFieldTypeCodeName, comment);
            }
            else
            {
                DeleteComment(vacancyId, vacancyReferralCommentsFieldTypeCodeName);
            }
        }

        private void UpsertComment(int vacancyId, string vacancyReferralCommentsFieldTypeCodeName, string comment)
        {
            if(!VacancyReferalCommentsFieldType.ContainsKey(vacancyReferralCommentsFieldTypeCodeName))
            {
                throw new ArgumentException($"{vacancyReferralCommentsFieldTypeCodeName} was not recognised as a valid vacancy referral comments field type code name");
            }

            var vacancyReferralCommentsFieldTypeId = VacancyReferalCommentsFieldType[vacancyReferralCommentsFieldTypeCodeName];

            const string sql = @"
merge dbo.VacancyReferralComments as target
using (values (@Comments))
    as source (Comments)
    on target.VacancyId = @VacancyId and FieldTypeId = @FieldTypeId
when matched then
    update
    set Comments = source.Comments
when not matched then
    insert ( VacancyId, FieldTypeId, Comments)
    values ( @VacancyId, @FieldTypeId, @Comments);";

            _getOpenConnection.MutatingQuery<object>(sql, new
            {
                VacancyId = vacancyId,
                FieldTypeId = vacancyReferralCommentsFieldTypeId,
                Comments = comment
            });
        }

        private void DeleteComment(int vacancyId, string vacancyReferralCommentsFieldTypeCodeName)
        {
            var vacancyReferralCommentsFieldTypeCodeNameId =
                _getOpenConnection.Query<int>(
                    $@"
	SELECT TOP 1 VacancyReferralCommentsFieldTypeId FROM dbo.VacancyReferralCommentsFieldType
	WHERE CodeName = '{vacancyReferralCommentsFieldTypeCodeName}'
").Single(); // TODO: Hardcode the ID?

            var sql = @"
    DELETE from dbo.VacancyReferralComments
    WHERE VacancyId = @VacancyId and FieldTypeId = @FieldTypeId";

            _getOpenConnection.MutatingQuery<object>(sql, new
            {
                VacancyId = vacancyId,
                FieldTypeId = vacancyReferralCommentsFieldTypeCodeNameId
            });
        }

        public DomainVacancy ReserveVacancyForQA(int vacancyReferenceNumber)
        {
            _logger.Debug(
                $"Calling database to get and reserve vacancy with reference number: {vacancyReferenceNumber} for QA");

            var userName = _currentUserService.CurrentUserName;

            // TODO: Add QAUserName / TimeStartedToQA. Perhaps a name without QA in would be better?
            // TODO: Possibly need MutatingQueryMulti to get address etc??? Or use join as only one record
            var dbVacancy = _getOpenConnection.MutatingQuery<Vacancy>(@"
UPDATE dbo.Vacancy
SET    QAUserName             = @UserName,
       StartedToQADateTime    = @StartedToQADateTime,
       VacancyStatusId        = @VacancyStatusId
WHERE  VacancyReferenceNumber = @VacancyReferenceNumber
-- AND    (QAUserName IS NULL OR (QAUserName = @UserName))
-- AND    (TimeStartedToQA IS NULL OR (TimeStartedToQA > @lockExpiryTime))

SELECT * FROM dbo.Vacancy WHERE VacancyReferenceNumber = @VacancyReferenceNumber
",
                new
                {
                    UserName = userName,
                    StartedToQADateTime = _dateTimeService.UtcNow,
                    VacancyStatusId = VacancyStatus.ReservedForQA,
                    VacancyReferenceNumber = vacancyReferenceNumber
                })
                .SingleOrDefault();
            // What should happen if QAUserName != UserName. Should we throw an exception?
            if (dbVacancy == null)
            {
                _logger.Warn(
                    $"Call to database to get and reserve vacancy with reference number: {vacancyReferenceNumber} for QA failed");
                return null;
            }

            _logger.Info(
                $"Called database to get and reserve vacancy with reference number: {vacancyReferenceNumber} for QA successfully");

            return GetByReferenceNumber(vacancyReferenceNumber);
        }

        public void UnReserveVacancyForQa(int vacancyReferenceNumber)
        {
            _logger.Debug(
                $"Calling database to get and unreserve vacancy with reference number: {vacancyReferenceNumber} for QA");

            var dbVacancy = _getOpenConnection.MutatingQuery<Vacancy>(@"
UPDATE dbo.Vacancy
SET    QAUserName             = null,
       VacancyStatusId        = @VacancyStatusId
WHERE  VacancyReferenceNumber = @VacancyReferenceNumber

SELECT * FROM dbo.Vacancy WHERE VacancyReferenceNumber = @VacancyReferenceNumber
",
                new
                {
                    VacancyStatusId = VacancyStatus.Submitted,
                    VacancyReferenceNumber = vacancyReferenceNumber
                })
                .SingleOrDefault();
            if (dbVacancy == null)
            {
                _logger.Warn(
                    $"Call to database to get and unreserve vacancy with reference number: {vacancyReferenceNumber} for QA failed");
                return;
            }

            _logger.Info(
                $"Called database to get and unreserve vacancy with reference number: {vacancyReferenceNumber} for QA successfully");
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

        public int GetVacancyIdByReferenceNumber(int vacancyReferenceNumber)
        {
            _logger.Debug("Calling database to get vacancy id for Vacancy Reference Number={0}",
                vacancyReferenceNumber);

            var vacancyId = _getOpenConnection.Query<int>(
                "SELECT VacancyId FROM dbo.Vacancy WHERE VacancyReferenceNumber = @VacancyReferenceNumber",
                new { VacancyReferenceNumber = vacancyReferenceNumber }).SingleOrDefault();

            return vacancyId;
        }

        public static readonly string SelectByIdSql = $@"SELECT	v.*,
        --CASE v.ApplyOutsideNAVMS
 		--    WHEN 1 THEN 0
        --    ELSE dbo.GetApplicantCount(v.VacancyId) 
        --END
        --AS ApplicantCount,
		--CASE v.ApplyOutsideNAVMS
 		--    WHEN 1 THEN 0
		--    ELSE dbo.GetNewApplicantCount(v.VacancyId)
		--END
		--AS NewApplicantCount,
        dbo.GetFirstSubmittedDate(v.VacancyID) AS DateFirstSubmitted,
		dbo.GetSubmittedDate(v.VacancyID) AS DateSubmitted,
		dbo.GetCreatedDate(v.VacancyID) AS CreatedDate,
        e.FullName AS EmployerName,
        e.EmployerId,
        e.Town AS EmployerLocation,
		af.CodeName AS FrameworkCodeName,
		el.CodeName AS ApprenticeshipLevel,
		ao.CodeName AS SectorCodeName,
		dbo.GetCreatedByProviderUsername(v.VacancyId) AS CreatedByProviderUsername,
		dbo.GetDateQAApproved(v.VacancyId) AS DateQAApproved,
		rt.TeamName AS RegionalTeam,
		af.StandardId,
		aq1.Question AS FirstQuestion,
		aq2.Question AS SecondQuestion,
		TBP.Value AS TrainingProvided,
		QR.Value AS DesiredQualifications,
		SR.Value AS DesiredSkills,
		PQ.Value AS PersonalQualities,
		RC.Value AS ThingsToConsider,
		FP.Value AS FutureProspects,
		OII.Value AS OtherInformation,
		TIT.Comments AS TitleComment,
		ALE.Comments AS ApprenticeshipLevelComment,
		CLD.Comments AS ClosingDateComment,
		CDE.Comments AS ContactDetailsComment,
		QUA.Comments AS DesiredQualificationsComment,
		SKL.Comments AS DesiredSkillsComment,
		EAD.Comments AS DurationComment,
		EDE.Comments AS EmployerDescriptionComment,
		EWB.Comments AS EmployerWebsiteUrlComment,
		QU1.Comments AS FirstQuestionComment,
		QU2.Comments AS SecondQuestionComment,
		APF.Comments AS FrameworkCodeNameComment,
		FUT.Comments AS FutureProspectsComment,
		FDE.Comments AS LongDescriptionComment,
		NPO.Comments AS NumberOfPositionsComment,
		OAI.Comments AS OfflineApplicationInstructionsComment,
		OAU.Comments AS OfflineApplicationUrlComment,
		PER.Comments AS PersonalQualitiesComment,
		PSD.Comments AS PossibleStartDateComment,
		APO.Comments AS SectorCodeNameComment,
		SDE.Comments AS ShortDescriptionComment,
		[SID].Comments AS StandardIdComment,
		IOI.Comments AS ThingsToConsiderComment,
		TRP.Comments AS TrainingProvidedComment,
		WWG.Comments AS WageComment,
		WWK.Comments AS WorkingWeekComment,
		LAD.Comments AS LocationAddressesComment,
		ALI.Comments AS AdditionalLocationInformationComment,
        AED.Comments AS AnonymousEmployerDescriptionComment,
        AER.Comments AS AnonymousEmployerReasonComment,
        AAE.Comments AS AnonymousAboutTheEmployerComment,
        OIN.Comments AS OtherInformationComment,
		la.CodeName AS LocalAuthorityCode,
		v.DurationTypeId AS DurationType,
		v.DurationValue AS Duration,
		c.FullName AS County
FROM	Vacancy v
JOIN	VacancyOwnerRelationship o
ON		o.VacancyOwnerRelationshipId = v.VacancyOwnerRelationshipId
JOIN	Employer e
ON		o.EmployerId = e.EmployerId
JOIN	ProviderSite s
ON      s.ProviderSiteId = v.VacancyManagerId
LEFT OUTER JOIN	ApprenticeshipFramework af
ON		af.ApprenticeshipFrameworkId = v.ApprenticeshipFrameworkId
LEFT OUTER JOIN	ApprenticeshipType AS at
ON		at.ApprenticeshipTypeId = v.ApprenticeshipType
LEFT OUTER JOIN	Reference.EducationLevel el
ON		el.EducationLevelId = at.EducationLevelId
LEFT OUTER JOIN	ApprenticeshipOccupation ao
ON		v.SectorId = ao.ApprenticeshipOccupationId
LEFT OUTER JOIN Reference.[Standard] rs
ON		rs.FullName = af.FullName
LEFT OUTER JOIN	RegionalTeamMappings t
ON		s.PostCode LIKE t.PostcodeStart + '[0-9]%'
LEFT OUTER JOIN	RegionalTeams rt
ON		rt.Id = t.RegionalTeam_Id
LEFT OUTER JOIN AdditionalQuestion aq1
ON		aq1.VacancyId = v.VacancyId AND aq1.QuestionId = 1
LEFT OUTER JOIN AdditionalQuestion aq2
ON		aq2.VacancyId = v.VacancyId AND aq2.QuestionId = 2
LEFT OUTER JOIN LocalAuthority la
ON		la.LocalAuthorityId = v.LocalAuthorityId
LEFT OUTER JOIN County c
ON		c.CountyId = v.CountyId
LEFT OUTER JOIN VacancyTextField TBP ON TBP.VacancyId = v.VacancyId AND TBP.Field = dbo.GetTextFieldId('TBP')
LEFT OUTER JOIN VacancyTextField QR ON QR.VacancyId = v.VacancyId AND QR.Field = dbo.GetTextFieldId('QR')
LEFT OUTER JOIN VacancyTextField SR ON SR.VacancyId = v.VacancyId AND SR.Field = dbo.GetTextFieldId('SR')
LEFT OUTER JOIN VacancyTextField PQ ON PQ.VacancyId = v.VacancyId AND PQ.Field = dbo.GetTextFieldId('PQ')
LEFT OUTER JOIN VacancyTextField RC ON RC.VacancyId = v.VacancyId AND RC.Field = dbo.GetTextFieldId('RC')
LEFT OUTER JOIN VacancyTextField FP ON FP.VacancyId = v.VacancyId AND FP.Field = dbo.GetTextFieldId('FP')
LEFT OUTER JOIN VacancyTextField OII ON OII.VacancyId = v.VacancyId AND OII.Field = dbo.GetTextFieldId('OII')
LEFT OUTER JOIN VacancyReferralComments TIT ON TIT.VacancyId = v.VacancyId AND TIT.FieldTypeId = dbo.GetCommentFieldId('TIT')
LEFT OUTER JOIN VacancyReferralComments ALE ON ALE.VacancyId = v.VacancyId AND ALE.FieldTypeId = dbo.GetCommentFieldId('ALE')
LEFT OUTER JOIN VacancyReferralComments CLD ON CLD.VacancyId = v.VacancyId AND CLD.FieldTypeId = dbo.GetCommentFieldId('CLD')
LEFT OUTER JOIN VacancyReferralComments CDE ON CDE.VacancyId = v.VacancyId AND CDE.FieldTypeId = dbo.GetCommentFieldId('CDE')
LEFT OUTER JOIN VacancyReferralComments QUA ON QUA.VacancyId = v.VacancyId AND QUA.FieldTypeId = dbo.GetCommentFieldId('QUA')
LEFT OUTER JOIN VacancyReferralComments SKL ON SKL.VacancyId = v.VacancyId AND SKL.FieldTypeId = dbo.GetCommentFieldId('SKL')
LEFT OUTER JOIN VacancyReferralComments EAD ON EAD.VacancyId = v.VacancyId AND EAD.FieldTypeId = dbo.GetCommentFieldId('EAD')
LEFT OUTER JOIN VacancyReferralComments EDE ON EDE.VacancyId = v.VacancyId AND EDE.FieldTypeId = dbo.GetCommentFieldId('EDE')
LEFT OUTER JOIN VacancyReferralComments EWB ON EWB.VacancyId = v.VacancyId AND EWB.FieldTypeId = dbo.GetCommentFieldId('EWB')
LEFT OUTER JOIN VacancyReferralComments QU1 ON QU1.VacancyId = v.VacancyId AND QU1.FieldTypeId = dbo.GetCommentFieldId('QU1')
LEFT OUTER JOIN VacancyReferralComments QU2 ON QU2.VacancyId = v.VacancyId AND QU2.FieldTypeId = dbo.GetCommentFieldId('QU2')
LEFT OUTER JOIN VacancyReferralComments APF ON APF.VacancyId = v.VacancyId AND APF.FieldTypeId = dbo.GetCommentFieldId('APF')
LEFT OUTER JOIN VacancyReferralComments FUT ON FUT.VacancyId = v.VacancyId AND FUT.FieldTypeId = dbo.GetCommentFieldId('FUT')
LEFT OUTER JOIN VacancyReferralComments FDE ON FDE.VacancyId = v.VacancyId AND FDE.FieldTypeId = dbo.GetCommentFieldId('FDE')
LEFT OUTER JOIN VacancyReferralComments NPO ON NPO.VacancyId = v.VacancyId AND NPO.FieldTypeId = dbo.GetCommentFieldId('NPO')
LEFT OUTER JOIN VacancyReferralComments OAI ON OAI.VacancyId = v.VacancyId AND OAI.FieldTypeId = dbo.GetCommentFieldId('OAI')
LEFT OUTER JOIN VacancyReferralComments OAU ON OAU.VacancyId = v.VacancyId AND OAU.FieldTypeId = dbo.GetCommentFieldId('OAU')
LEFT OUTER JOIN VacancyReferralComments PER ON PER.VacancyId = v.VacancyId AND PER.FieldTypeId = dbo.GetCommentFieldId('PER')
LEFT OUTER JOIN VacancyReferralComments PSD ON PSD.VacancyId = v.VacancyId AND PSD.FieldTypeId = dbo.GetCommentFieldId('PSD')
LEFT OUTER JOIN VacancyReferralComments APO ON APO.VacancyId = v.VacancyId AND APO.FieldTypeId = dbo.GetCommentFieldId('APO')
LEFT OUTER JOIN VacancyReferralComments SDE ON SDE.VacancyId = v.VacancyId AND SDE.FieldTypeId = dbo.GetCommentFieldId('SDE')
LEFT OUTER JOIN VacancyReferralComments [SID] ON [SID].VacancyId = v.VacancyId AND [SID].FieldTypeId = dbo.GetCommentFieldId('SID')
LEFT OUTER JOIN VacancyReferralComments IOI ON IOI.VacancyId = v.VacancyId AND IOI.FieldTypeId = dbo.GetCommentFieldId('IOI')
LEFT OUTER JOIN VacancyReferralComments TRP ON TRP.VacancyId = v.VacancyId AND TRP.FieldTypeId = dbo.GetCommentFieldId('TRP')
LEFT OUTER JOIN VacancyReferralComments WWG ON WWG.VacancyId = v.VacancyId AND WWG.FieldTypeId = dbo.GetCommentFieldId('WWG')
LEFT OUTER JOIN VacancyReferralComments WWK ON WWK.VacancyId = v.VacancyId AND WWK.FieldTypeId = dbo.GetCommentFieldId('WWK')
LEFT OUTER JOIN VacancyReferralComments LAD ON LAD.VacancyId = v.VacancyId AND LAD.FieldTypeId = dbo.GetCommentFieldId('LAD')
LEFT OUTER JOIN VacancyReferralComments ALI ON ALI.VacancyId = v.VacancyId AND ALI.FieldTypeId = dbo.GetCommentFieldId('ALI')
LEFT OUTER JOIN VacancyReferralComments AED ON AED.VacancyId = v.VacancyId AND AED.FieldTypeId = dbo.GetCommentFieldId('AED')
LEFT OUTER JOIN VacancyReferralComments AER ON AER.VacancyId = v.VacancyId AND AER.FieldTypeId = dbo.GetCommentFieldId('AER')
LEFT OUTER JOIN VacancyReferralComments AAE ON AAE.VacancyId = v.VacancyId AND AAE.FieldTypeId = dbo.GetCommentFieldId('AAE')
LEFT OUTER JOIN VacancyReferralComments OIN ON OIN.VacancyId = v.VacancyId AND OIN.FieldTypeId = dbo.GetCommentFieldId('OIN')
WHERE v.VacancyID = @vacancyId";

        public static readonly string SelectVacancyIdFromReferenceNumberSql = @"
SELECT VacancyId
FROM   dbo.Vacancy
WHERE  VacancyReferenceNumber = @vacancyReferenceNumber";

        public static readonly string SelectVacancyIdFromGuidSql = @"
SELECT VacancyId
FROM   dbo.Vacancy
WHERE  VacancyGuid = @vacancyGuid";
    }
}