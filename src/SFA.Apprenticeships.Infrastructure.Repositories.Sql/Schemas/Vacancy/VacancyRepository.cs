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
        private readonly TimeSpan _cacheDuration = TimeSpan.FromHours(1);

        private const string VacancySummarySelect = @"
SELECT VacancyId, VacancyOwnerRelationshipId, VacancyReferenceNumber, VacancyStatusId, 
AddressLine1, AddressLine2, AddressLine3, AddressLine4, AddressLine5, Town, CountyId, PostCode, LocalAuthorityId, Longitude, Latitude, 
ApprenticeshipFrameworkId, Title, ApprenticeshipType, ShortDescription, WeeklyWage, WageType, WageText, NumberofPositions, 
ApplicationClosingDate, InterviewsFromDate, ExpectedStartDate, ExpectedDuration, WorkingWeek, EmployerAnonymousName, 
ApplyOutsideNAVMS, LockedForSupportUntil, NoOfOfflineApplicants, MasterVacancyId, VacancyLocationTypeId, VacancyManagerID, 
VacancyGuid, SubmissionCount, StartedToQADateTime, StandardId, HoursPerWeek, WageUnitId, DurationTypeId, DurationValue, QAUserName, 
TrainingTypeId, VacancyTypeId, SectorId, UpdatedDateTime";

        public VacancyRepository(IGetOpenConnection getOpenConnection, IMapper mapper, IDateTimeService dateTimeService, ILogService logger)
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

        private Vacancy GetVacancyByVacancyId(int vacancyId)
        {
            var dbVacancy = _getOpenConnection.Query<Vacancy>(
                "SELECT * FROM dbo.Vacancy WHERE VacancyId = @VacancyId",
                new { VacancyId = vacancyId }).SingleOrDefault();
            return dbVacancy;
        }

        public List<VacancySummary> GetByIds(IEnumerable<int> vacancyIds)
        {
            var vacancyIdsArray
                = vacancyIds as int[] ?? vacancyIds.ToArray();
            _logger.Debug("Calling database to get apprenticeship vacancy with Ids={0}", string.Join(", ", vacancyIdsArray));

            var vacancies =
                _getOpenConnection.Query<Vacancy>(VacancySummarySelect + " FROM dbo.Vacancy WHERE VacancyId IN @VacancyIds",
                    new { VacancyIds = vacancyIdsArray });

            return MapVacancySummaries(vacancies.ToList());
        }

        public List<VacancySummary> GetByOwnerPartyIds(IEnumerable<int> ownerPartyIds)
        {
            var ownerPartyIdsArray = ownerPartyIds as int[] ?? ownerPartyIds.ToArray();
            _logger.Debug("Calling database to get apprenticeship vacancy with VacancyOwnerRelationshipId={0}", string.Join(", ", ownerPartyIdsArray));

            var vacancies =
                _getOpenConnection.Query<Vacancy>(
VacancySummarySelect + @"
FROM dbo.Vacancy 
WHERE VacancyOwnerRelationshipId IN @VacancyOwnerRelationshipIds",
                    new { VacancyOwnerRelationshipIds = ownerPartyIdsArray });

            return MapVacancySummaries(vacancies.ToList());
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

        public List<VacancySummary> GetWithStatus(int pageSize, int page, params VacancyStatus[] desiredStatuses)
        {
            _logger.Debug("Called database to get page {1} of apprenticeship vacancies in status {0}. Page size {2}", string.Join(",", desiredStatuses), page, pageSize);


            var sql = VacancySummarySelect + @"
            FROM   dbo.Vacancy
            WHERE  VacancyStatusId IN @VacancyStatusCodeIds";

            if (pageSize > 0)
            {
                var offset = pageSize * page;
                sql += @"
                ORDER BY VacancyId
                OFFSET " + offset + @" ROWS
                FETCH NEXT " + pageSize + @" ROWS ONLY";
            }

            var dbVacancies = _getOpenConnection.Query<Vacancy>(
            sql, new
            {
                VacancyStatusCodeIds = desiredStatuses.Select(s => (int)s)
            });


            _logger.Debug(
                $"Found {dbVacancies.Count} apprenticeship vacancies in page {page} with statuses in {string.Join(",", desiredStatuses)}. Page size {pageSize}");

            return MapVacancySummaries(dbVacancies.ToList());
        }

        public List<VacancySummary> Find(ApprenticeshipVacancyQuery query, out int totalResultsCount)
        {
            _logger.Debug("Calling database to find apprenticeship vacancies");

            var paramObject =
                new
                {
                    query.FrameworkCodeName,
                    query.LiveDate,
                    query.LatestClosingDate,
                    VacancyStatusCodeIds = query.DesiredStatuses.Select(s => (int)s),
                    LiveStatusId = (int)VacancyStatus.Live,
                    query.CurrentPage,
                    query.PageSize
                };

            var coreQuery = @"
FROM   dbo.Vacancy as vac
WHERE  VacancyStatusId IN @VacancyStatusCodeIds
" +
                            (string.IsNullOrWhiteSpace(query.FrameworkCodeName)
                                ? ""
                                : "AND FrameworkId = (SELECT ApprenticeshipFrameworkId FROM dbo.ApprenticeshipFramework where CodeName = @FrameworkCodeName) ") +
                            @"
" + (query.LiveDate.HasValue ? @"AND (select top 1 HistoryDate
from dbo.VacancyHistory as vh
where vh.VacancyId = vac.VacancyId and VacancyHistoryEventSubTypeId = @LiveStatusId
order by HistoryDate desc) >= @LiveDate" : "") + @"  
" + (query.LatestClosingDate.HasValue ? "AND ApplicationClosingDate <= @LatestClosingDate" : ""); // check these dates
                                                                                                  // Vacancy.PublishedDateTime >= @LiveDate was Vacancy.DateSubmitted >= @LiveDate

            // TODO: Vacancy.DateSubmitted should be DateLive (or DatePublished)???
            var data = _getOpenConnection.QueryMultiple<int, Vacancy>(@"
SELECT COUNT(*)
" + coreQuery + @"

" + VacancySummarySelect + @"
" + coreQuery + @"
ORDER BY VacancyReferenceNumber
OFFSET ((@CurrentPage - 1) * @PageSize) ROWS
FETCH NEXT @PageSize ROWS ONLY
", paramObject);

            totalResultsCount = data.Item1.Single();

            var dbVacancies = data.Item2;

            _logger.Debug("Found {0} apprenticeship vacanc(ies)", dbVacancies.Count);

            return MapVacancySummaries(dbVacancies.ToList());
        }

        private DomainVacancy MapVacancy(Vacancy dbVacancy)
        {
            if (dbVacancy == null)
                return null;
            
            var result = _mapper.Map<Vacancy, DomainVacancy>(dbVacancy);
            MapAdditionalQuestions(dbVacancy, result);
            MapTextFields(dbVacancy, result);
            MapApprenticeshipType(dbVacancy, result);
            MapFrameworkId(dbVacancy, result);
            MapSectorId(dbVacancy, result);
            MapDateFirstSubmitted(dbVacancy, result);
            MapCreatedDateTime(dbVacancy, result);
            MapDateSubmitted(dbVacancy, result);
            MapDateQAApproved(dbVacancy, result);
            MapComments(dbVacancy, result);

            return result;
        }

        private List<VacancySummary> MapVacancySummaries(List<Vacancy> dbVacancies)
        {
            var results = _mapper.Map<List<Vacancy>, List<VacancySummary>>(dbVacancies);

            MapApprenticeshipTypes(dbVacancies, results);
            MapFrameworkIds(dbVacancies, results);
            MapSectorIds(dbVacancies, results);
            for (int i = 0; i < dbVacancies.Count; i++)
            {
                MapDateFirstSubmitted(dbVacancies[i], results[i]);
                MapDateSubmitted(dbVacancies[i], results[i]);
                MapDateQAApproved(dbVacancies[i], results[i]);
            }

            return results;
        }

        private void MapFrameworkId(Vacancy dbVacancy, VacancySummary result)
        {
            if (dbVacancy.ApprenticeshipFrameworkId.HasValue)
            {
                result.FrameworkCodeName =
                    _getOpenConnection.QueryCached<string>(_cacheDuration, @"
SELECT CodeName
FROM   dbo.ApprenticeshipFramework
WHERE  ApprenticeshipFrameworkId = @ApprenticeshipFrameworkId",
                        new
                        {
                            ApprenticeshipFrameworkId = dbVacancy.ApprenticeshipFrameworkId.Value
                        }).Single();
            }
        }

        private void MapFrameworkIds(IEnumerable<Vacancy> dbVacancies, IEnumerable<VacancySummary> results)
        {
            var ids = dbVacancies.Select(v => v.ApprenticeshipFrameworkId).Distinct().Where(id => id.HasValue);
            var map = _getOpenConnection.QueryCached<PropertyMapItem>(_cacheDuration, @"
SELECT ApprenticeshipFrameworkId as Map, CodeName as Value
FROM   dbo.ApprenticeshipFramework
WHERE  ApprenticeshipFrameworkId IN @Ids",
                        new
                        {
                            Ids = ids
                        }).ToDictionary(t => t.Map.ToString(), t => t.Value);

            foreach (var vacancySummary in results.Where(v => v.FrameworkCodeName != null))
            {
                var value = map[vacancySummary.FrameworkCodeName];
                vacancySummary.FrameworkCodeName = value;
            }
        }

        private void MapApprenticeshipType(Vacancy dbVacancy, VacancySummary result)
        {
            if (dbVacancy.ApprenticeshipType.HasValue)
            {
                var educationLevelCodeName =
                    _getOpenConnection.QueryCached<string>(_cacheDuration, @"
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

        private void MapApprenticeshipTypes(IEnumerable<Vacancy> dbVacancies, IEnumerable<VacancySummary> results)
        {
            var ids = dbVacancies.Select(v => v.ApprenticeshipType).Distinct().Where(id => id.HasValue);
            var map = _getOpenConnection.QueryCached<PropertyMapItem>(_cacheDuration, @"
SELECT at.ApprenticeshipTypeId as Map, el.CodeName as Value
FROM   Reference.EducationLevel as el JOIN dbo.ApprenticeshipType as at ON el.EducationLevelId = at.EducationLevelId
WHERE  at.ApprenticeshipTypeId IN @Ids",
                        new
                        {
                            Ids = ids
                        }).ToDictionary(t => t.Map, t => (ApprenticeshipLevel)Enum.Parse(typeof(ApprenticeshipLevel), t.Value));

            foreach (var vacancySummary in results.Where(v => v.ApprenticeshipLevel != ApprenticeshipLevel.Unknown))
            {
                var apprenticeshipLevel = map[(int)vacancySummary.ApprenticeshipLevel];
                vacancySummary.ApprenticeshipLevel = apprenticeshipLevel;
            }
        }

        private void MapSectorId(Vacancy dbVacancy, VacancySummary result)
        {
            if (dbVacancy.SectorId.HasValue)
            {
                result.SectorCodeName = _getOpenConnection.QueryCached<string>(_cacheDuration, @"
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

        private void MapSectorIds(IEnumerable<Vacancy> dbVacancies, IEnumerable<VacancySummary> results)
        {
            var ids = dbVacancies.Select(v => v.SectorId).Distinct().Where(id => id.HasValue);
            var map = _getOpenConnection.QueryCached<PropertyMapItem>(_cacheDuration, @"
SELECT ApprenticeshipOccupationId as Map, CodeName as Value
FROM   dbo.ApprenticeshipOccupation
WHERE  ApprenticeshipOccupationId IN @Ids",
                        new
                        {
                            Ids = ids
                        }).ToDictionary(t => t.Map.ToString(), t => t.Value);

            foreach (var vacancySummary in results.Where(v => v.SectorCodeName != null))
            {
                var value = map[vacancySummary.SectorCodeName];
                vacancySummary.SectorCodeName = value;
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
            result.TrainingProvided = GetTextField(dbVacancy.VacancyId, TextFieldCodeName.TrainingProvided);
            result.DesiredQualifications = GetTextField(dbVacancy.VacancyId, TextFieldCodeName.DesiredQualifications);
            result.DesiredSkills = GetTextField(dbVacancy.VacancyId, TextFieldCodeName.DesiredSkills);
            result.PersonalQualities = GetTextField(dbVacancy.VacancyId, TextFieldCodeName.PersonalQualities);
            result.ThingsToConsider = GetTextField(dbVacancy.VacancyId, TextFieldCodeName.ThingsToConsider);
            result.FutureProspects = GetTextField(dbVacancy.VacancyId, TextFieldCodeName.FutureProspects);
        }

        private void MapComments(Vacancy dbVacancy, DomainVacancy result)
        {
            result.TitleComment = GetComment(dbVacancy.VacancyId, ReferralCommentCodeName.TitleComment);
            result.ApprenticeshipLevelComment = GetComment(dbVacancy.VacancyId, ReferralCommentCodeName.ApprenticeshipLevelComment);
            result.ClosingDateComment = GetComment(dbVacancy.VacancyId, ReferralCommentCodeName.ClosingDateComment);
            result.ContactDetailsComment = GetComment(dbVacancy.VacancyId, ReferralCommentCodeName.ContactDetailsComment);
            result.DesiredQualificationsComment = GetComment(dbVacancy.VacancyId, ReferralCommentCodeName.DesiredQualificationsComment);
            result.DesiredSkillsComment = GetComment(dbVacancy.VacancyId, ReferralCommentCodeName.DesiredSkillsComment);
            result.DurationComment = GetComment(dbVacancy.VacancyId, ReferralCommentCodeName.DurationComment);
            result.EmployerDescriptionComment = GetComment(dbVacancy.VacancyId, ReferralCommentCodeName.EmployerDescriptionComment);
            result.EmployerWebsiteUrlComment = GetComment(dbVacancy.VacancyId, ReferralCommentCodeName.EmployerWebsiteUrlComment);
            result.FirstQuestionComment = GetComment(dbVacancy.VacancyId, ReferralCommentCodeName.FirstQuestionComment);
            result.SecondQuestionComment = GetComment(dbVacancy.VacancyId, ReferralCommentCodeName.SecondQuestionComment);
            result.FrameworkCodeNameComment = GetComment(dbVacancy.VacancyId, ReferralCommentCodeName.FrameworkCodeNameComment);
            result.FutureProspectsComment = GetComment(dbVacancy.VacancyId, ReferralCommentCodeName.FutureProspectsComment);
            result.LongDescriptionComment = GetComment(dbVacancy.VacancyId, ReferralCommentCodeName.LongDescriptionComment);
            result.NumberOfPositionsComment = GetComment(dbVacancy.VacancyId, ReferralCommentCodeName.NumberOfPositionsComment);
            result.OfflineApplicationInstructionsComment = GetComment(dbVacancy.VacancyId, ReferralCommentCodeName.OfflineApplicationInstructionsComment);
            result.OfflineApplicationUrlComment = GetComment(dbVacancy.VacancyId, ReferralCommentCodeName.OfflineApplicationUrlComment);
            result.PersonalQualitiesComment = GetComment(dbVacancy.VacancyId, ReferralCommentCodeName.PersonalQualitiesComment);
            result.PossibleStartDateComment = GetComment(dbVacancy.VacancyId, ReferralCommentCodeName.PossibleStartDateComment);
            result.SectorCodeNameComment = GetComment(dbVacancy.VacancyId, ReferralCommentCodeName.SectorCodeNameComment);
            result.ShortDescriptionComment = GetComment(dbVacancy.VacancyId, ReferralCommentCodeName.ShortDescriptionComment);
            result.StandardIdComment = GetComment(dbVacancy.VacancyId, ReferralCommentCodeName.StandardIdComment);
            result.ThingsToConsiderComment = GetComment(dbVacancy.VacancyId, ReferralCommentCodeName.ThingsToConsiderComment);
            result.TrainingProvidedComment = GetComment(dbVacancy.VacancyId, ReferralCommentCodeName.TrainingProvidedComment);
            result.WageComment = GetComment(dbVacancy.VacancyId, ReferralCommentCodeName.WageComment);
            result.WorkingWeekComment = GetComment(dbVacancy.VacancyId, ReferralCommentCodeName.WorkingWeekComment);
            result.LocationAddressesComment = GetComment(dbVacancy.VacancyId, ReferralCommentCodeName.LocationAddressesComment);
            result.AdditionalLocationInformationComment = GetComment(dbVacancy.VacancyId, ReferralCommentCodeName.AdditionalLocationInformationComment);
        }

        private string GetComment(int vacancyId, string vacancyReferralCommentTypeCodeName)
        {
            var vacancyReferralCommentTypeId =
                _getOpenConnection.Query<int>(
                    $@"
	SELECT TOP 1 VacancyReferralCommentsFieldTypeId FROM dbo.VacancyReferralCommentsFieldType
	WHERE CodeName = '{vacancyReferralCommentTypeCodeName}'
").Single(); // TODO: Hardcode the ID?

            return _getOpenConnection.Query<string>(@"
SELECT Comments
FROM   dbo.VacancyReferralComments
WHERE  VacancyId = @VacancyId AND FieldTypeId = @FieldTypeId
", new
            {
                VacancyId = vacancyId,
                FieldTypeId = vacancyReferralCommentTypeId
            }).SingleOrDefault();
        }

        private void MapDateFirstSubmitted(Vacancy dbVacancy, VacancySummary result)
        {
            result.DateFirstSubmitted = _getOpenConnection.Query<DateTime>(@"
select top 1 HistoryDate
from dbo.VacancyHistory
where VacancyId = @VacancyId and VacancyHistoryEventSubTypeId = @VacancyStatus
order by HistoryDate
",
                new
                {
                    VacancyId = dbVacancy.VacancyId,
                    VacancyStatus = VacancyStatus.Submitted
                }
                ).SingleOrDefault();
        }

        private void MapDateSubmitted(Vacancy dbVacancy, VacancySummary result)
        {
            result.DateSubmitted = _getOpenConnection.Query<DateTime?>(@"
select top 1 HistoryDate
from dbo.VacancyHistory
where VacancyId = @VacancyId and VacancyHistoryEventSubTypeId = @VacancyStatus
order by HistoryDate desc
",
                new
                {
                    VacancyId = dbVacancy.VacancyId,
                    VacancyStatus = VacancyStatus.Submitted
                }
                ).SingleOrDefault();
        }

        private void MapCreatedDateTime(Vacancy dbVacancy, DomainVacancy result)
        {
            result.CreatedDateTime = _getOpenConnection.Query<DateTime>(@"
select top 1 HistoryDate
from dbo.VacancyHistory
where VacancyId = @VacancyId and VacancyHistoryEventSubTypeId = @VacancyStatus
order by HistoryDate
",
                new
                {
                    VacancyId = dbVacancy.VacancyId,
                    VacancyStatus = VacancyStatus.Draft
                }
                ).SingleOrDefault();
        }

        private void MapDateQAApproved(Vacancy dbVacancy, VacancySummary result)
        {
            result.DateQAApproved = _getOpenConnection.Query<DateTime?>(@"
select top 1 HistoryDate
from dbo.VacancyHistory
where VacancyId = @VacancyId and VacancyHistoryEventSubTypeId = @VacancyStatus
order by HistoryDate desc
",
                new
                {
                    VacancyId = dbVacancy.VacancyId,
                    VacancyStatus = VacancyStatus.Live
                }
                ).SingleOrDefault();
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

        public DomainVacancy Create(DomainVacancy entity)
        {
            _logger.Debug("Calling database to save apprenticeship vacancy with id={0}", entity.VacancyId);

            UpdateEntityTimestamps(entity);

            var dbVacancy = _mapper.Map<DomainVacancy, Vacancy>(entity);

            PopulateIds(entity, dbVacancy);

            dbVacancy.VacancyId = (int) _getOpenConnection.Insert(dbVacancy);

            SaveTextFieldsFor(dbVacancy.VacancyId, entity);
            SaveAdditionalQuestionsFor(dbVacancy.VacancyId, entity);

            CreateVacancyHistoryRow(dbVacancy.VacancyId, GetUserName(), VacancyHistoryEventType.StatusChange,
                (int)entity.Status);

            _logger.Debug("Saved apprenticeship vacancy to database with id={0}", entity.VacancyId);

            return _mapper.Map<Vacancy, DomainVacancy>(dbVacancy);
        }

        public DomainVacancy Update(DomainVacancy entity)
        {
            _logger.Debug("Calling database to update apprenticeship vacancy with id={0}", entity.VacancyId);

            UpdateEntityTimestamps(entity); // Do we need this?

            var dbVacancy = _mapper.Map<DomainVacancy, Vacancy>(entity);

            PopulateIds(entity, dbVacancy);

            var previousVacancyState = GetVacancyByVacancyId(entity.VacancyId);

            _getOpenConnection.UpdateSingle(dbVacancy);

            SaveTextFieldsFor(dbVacancy.VacancyId, entity);
            SaveAdditionalQuestionsFor(dbVacancy.VacancyId, entity);
            SaveCommentsFor(dbVacancy.VacancyId, entity);

            UpdateVacancyHistory(previousVacancyState, dbVacancy);

            _logger.Debug("Updated apprenticeship vacancy with to database with id={0}", entity.VacancyId);

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
            if (entity.Address?.County != null)
            {
                dbVacancy.CountyId = _getOpenConnection.QueryCached<int>(_cacheDuration, @"
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
            var vacancyLocationTypeCodeName = entity.IsEmployerLocationMainApprenticeshipLocation.Value == true
                ? "STD"
                : "MUL";

            dbVacancy.VacancyLocationTypeId = _getOpenConnection.QueryCached<int>(_cacheDuration, @"
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
            dbVacancy.ApprenticeshipType = _getOpenConnection.QueryCached<int>(_cacheDuration, @"
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
            SaveTextField(vacancyId, TextFieldCodeName.TrainingProvided, entity.TrainingProvided);
            SaveTextField(vacancyId, TextFieldCodeName.DesiredQualifications, entity.DesiredQualifications);
            SaveTextField(vacancyId, TextFieldCodeName.DesiredSkills, entity.DesiredSkills);
            SaveTextField(vacancyId, TextFieldCodeName.PersonalQualities, entity.PersonalQualities);
            SaveTextField(vacancyId, TextFieldCodeName.ThingsToConsider, entity.ThingsToConsider);
            SaveTextField(vacancyId, TextFieldCodeName.FutureProspects, entity.FutureProspects);
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
            else
            {
                DeleteAdditionalQuestion(vacancyId, 1);
            }

            if (!string.IsNullOrWhiteSpace(entity.SecondQuestion))
            {
                UpsertAdditionalQuestion(vacancyId, 2, entity.SecondQuestion);
            }
            else
            {
                DeleteAdditionalQuestion(vacancyId, 2);
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

        private void DeleteAdditionalQuestion(int vacancyId, int additionalQuestionId)
        {
            var sql = @"
    DELETE from dbo.AdditionalQuestion
    WHERE VacancyId = @VacancyId and QuestionId = @QuestionId";

            _getOpenConnection.MutatingQuery<object>(sql, new
            {
                VacancyId = vacancyId,
                QuestionId = additionalQuestionId
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
            var vacancyReferralCommentsFieldTypeId =
                _getOpenConnection.Query<int>(
                    $@"
	SELECT TOP 1 VacancyReferralCommentsFieldTypeId FROM dbo.VacancyReferralCommentsFieldType
	WHERE CodeName = '{
                        vacancyReferralCommentsFieldTypeCodeName}'
").Single(); // TODO: Hardcode the ID?

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

            var userName = Thread.CurrentPrincipal.Identity.Name; // use function/service

            // TODO: Add QAUserName / TimeStartedToQA. Perhaps a name without QA in would be better?
            // TODO: Possibly need MutatingQueryMulti to get address etc??? Or use join as only one record
            var dbVacancy = _getOpenConnection.MutatingQuery<Vacancy>(@"
UPDATE dbo.Vacancy
SET    QAUserName             = @UserName,
       StartedToQADateTime    = @StartedToQADateTime,
       VacancyStatusId        = @VacancyStatusId
WHERE  VacancyReferenceNumber = @VacancyReferenceNumber
AND    (QAUserName IS NULL OR (QAUserName = @UserName))
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