namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Vacancy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using dbo;
    using Domain.Entities.Raa.Reference;
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
        private const int FirstQuestionId = 1;
        private const int SecondQuestionId = 2;
        private readonly IMapper _mapper;
        private readonly IDateTimeService _dateTimeService;
        private readonly ILogService _logger;
        private readonly ICurrentUserService _currentUserService;

        private readonly IGetOpenConnection _getOpenConnection;
        private readonly TimeSpan _cacheDuration = TimeSpan.FromHours(1);

        private readonly string[] VacancySummaryColumns = new string[] {
            "VacancyId", "VacancyOwnerRelationshipId", "VacancyReferenceNumber", "VacancyStatusId",
            "AddressLine1", "AddressLine2", "AddressLine3", "AddressLine4", "AddressLine5", "Town", "CountyId", "PostCode", "LocalAuthorityId", "Longitude", "Latitude",
            "ApprenticeshipFrameworkId", "Title", "ApprenticeshipType", "ShortDescription", "WeeklyWage", "WageType", "WageText", "NumberofPositions",
            "ApplicationClosingDate", "InterviewsFromDate", "ExpectedStartDate", "ExpectedDuration", "WorkingWeek", "EmployerAnonymousName",
            "ApplyOutsideNAVMS", "LockedForSupportUntil", "NoOfOfflineApplicants", "MasterVacancyId", "VacancyLocationTypeId", "VacancyManagerID",
            "VacancyGuid", "SubmissionCount", "StartedToQADateTime", "StandardId", "HoursPerWeek", "WageUnitId", "DurationTypeId", "DurationValue", "QAUserName",
            "TrainingTypeId", "VacancyTypeId", "SectorId", "UpdatedDateTime"
        };

        private const int StandardsApprenticeshipOccupationId = 100;

        private const string StatusChangeText = "Status Change";

        private readonly Dictionary<string, string> _standardsMap = new Dictionary<string, string>
        {
            { "Motor Vehicle Service and Maintenance Technician (light vehicle) (L3)", "Motor Vehicle Service and Maintenance Technician [light vehicle] (L3)" },
            { "Public Service - Operational delivery officer (Level 3)", "Public Service Operational Delivery Officer"},
            { "Cyber Security", "Cyber security technologist" },
            { "Workplace Pensions (Administrator or Consultant)", "Workplace Pensions (Administrator or Consultant)()"}
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
                _getOpenConnection.Query<VacancyPlus>($@"
SELECT {string.Join(", ", VacancySummaryColumns)},
       (SELECT TOP 1 HistoryDate
        FROM   dbo.VacancyHistory h
        WHERE  h.VacancyId                    = v.VacancyId
        AND    h.VacancyHistoryEventSubTypeId = @VacancyStatusId_Submitted
        ORDER BY HistoryDate ASC) AS DateFirstSubmitted,
       (SELECT TOP 1 HistoryDate
        FROM   dbo.VacancyHistory h
        WHERE  h.VacancyId                    = v.VacancyId
        AND    h.VacancyHistoryEventSubTypeId = @VacancyStatusId_Submitted
        ORDER BY HistoryDate DESC) AS DateSubmitted,
       (SELECT TOP 1 HistoryDate
        FROM   dbo.VacancyHistory h
        WHERE  h.VacancyId                    = v.VacancyId
        AND    h.VacancyHistoryEventSubTypeId = @VacancyStatusId_Live
        ORDER BY HistoryDate DESC) AS DateQAApproved
FROM   dbo.Vacancy v
WHERE  v.VacancyId IN @VacancyIds",
                    new { VacancyIds = vacancyIdsArray, VacancyStatusId_Live = VacancyStatus.Live, VacancyStatusId_Submitted = VacancyStatus.Submitted });

            return MapVacancySummaries((IReadOnlyList<VacancyPlus>)vacancies);
        }

        public List<VacancySummary> GetByOwnerPartyIds(IEnumerable<int> ownerPartyIds)
        {
            var ownerPartyIdsArray = ownerPartyIds as int[] ?? ownerPartyIds.ToArray();
            _logger.Debug("Calling database to get apprenticeship vacancy with VacancyOwnerRelationshipId={0}", string.Join(", ", ownerPartyIdsArray));

            var vacancies =
                _getOpenConnection.Query<Vacancy>($@"
SELECT {string.Join(", ", VacancySummaryColumns)}
FROM   dbo.Vacancy 
WHERE  VacancyOwnerRelationshipId IN @VacancyOwnerRelationshipIds",
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="page">Expects page starting from 0 rather than 1</param>
        /// <param name="filterByProviderBeenMigrated"></param>
        /// <param name="desiredStatuses"></param>
        /// <returns></returns>
        public List<VacancySummary> GetWithStatus(int pageSize, int page, bool filterByProviderBeenMigrated, params VacancyStatus[] desiredStatuses)
        {
            _logger.Debug("Called database to get page {1} of apprenticeship vacancies in status {0}. Page size {2}", string.Join(",", desiredStatuses), page, pageSize);


            var sql = $@"
            SELECT {string.Join(", ", VacancySummaryColumns)}
            FROM dbo.Vacancy";

            if (filterByProviderBeenMigrated)
            {
                sql += " inner join Provider on ContractOwnerId = ProviderId";
            }

            sql += " WHERE VacancyStatusId IN @VacancyStatusCodeIds";

            if (filterByProviderBeenMigrated)
            {
                sql += " AND ProviderToUseFAA = 2";
            }

            if (pageSize > 0)
            {
                var offset = pageSize * page;
                sql += @"
                ORDER BY VacancyId
                OFFSET " + offset + @" ROWS
                FETCH NEXT " + pageSize + @" ROWS ONLY";
            }
            //TODO: adding a timeout of 10 mins. This value needs to change and come from a config file in the future.
            var dbVacancies = _getOpenConnection.Query<Vacancy>(
            sql, new
            {
                VacancyStatusCodeIds = desiredStatuses.Select(s => (int)s)
            },600);


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
                            (query.EditedInRaa
                                ? "AND vac.EditedInRaa = 1"
                                : "") +
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

SELECT " + string.Join(", ", VacancySummaryColumns) + @"
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
            MapCreatedByProviderUsername(dbVacancy, result);
            MapDateSubmitted(dbVacancy, result);

            MapDateQAApproved(dbVacancy, result);

            MapComments(dbVacancy, result);
            MapRegionalTeam(result);
            MapLocalAuthorityCode(dbVacancy, result);
            MapDuration(dbVacancy, result);
            MapCountyId(dbVacancy, result);

            PatchTrainingType(result);
            PatchStandards(dbVacancy, result);

            return result;
        }

        private List<VacancySummary> MapVacancySummaries(IReadOnlyList<Vacancy> dbVacancies)
        {
            var results = _mapper.Map<IReadOnlyList<Vacancy>, List<VacancySummary>>(dbVacancies);

            MapApprenticeshipTypes(dbVacancies, results);
            MapFrameworkIds(dbVacancies, results);
            MapSectorIds(dbVacancies, results);

            for (var i = 0; i < dbVacancies.Count; i++)
            {
                var dbVacancy = dbVacancies[i];
                var vacancySummary = results[i];

                MapDateFirstSubmitted(dbVacancy, vacancySummary);
                MapDateSubmitted(dbVacancy, vacancySummary);
                MapDateQAApproved(dbVacancy, vacancySummary);
                MapRegionalTeam(vacancySummary);
                MapDuration(dbVacancy, vacancySummary);
                PatchStandards(dbVacancy, vacancySummary);
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

                //Changed to try parse as traineeship apprenticeship type is not a recognised apprenticeship level but should be preserved
                ApprenticeshipLevel apprenticeshipLevel;
                if (Enum.TryParse(educationLevelCodeName, out apprenticeshipLevel) && Enum.IsDefined(typeof(ApprenticeshipLevel), apprenticeshipLevel))
                {
                    result.ApprenticeshipLevel = apprenticeshipLevel;
                }
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

        private void MapCountyId(Vacancy dbVacancy, VacancySummary result)
        {
            // Not all the vacancies have CountyId (before being accepted by QA).
            // A multilocation vacancy (more than one location) doesn't have anything in the address fields.
            
            if (dbVacancy.CountyId > 0)
            {
                result.Address.County = _getOpenConnection.QueryCached<string>(_cacheDuration, @"
SELECT FullName
FROM   dbo.County
WHERE  CountyId = @CountyId",
                    new
                    {
                        CountyId = dbVacancy.CountyId
                    }).Single();
            }
        }

        private void MapLocalAuthorityCode(Vacancy dbVacancy, DomainVacancy result)
        {
            if (dbVacancy.LocalAuthorityId.HasValue)
            {
                result.LocalAuthorityCode = _getOpenConnection.QueryCached<string>(_cacheDuration, @"
SELECT CodeName
FROM   dbo.LocalAuthority
WHERE  LocalAuthorityId = @LocalAuthorityId",
                    new
                    {
                        dbVacancy.LocalAuthorityId
                    }).Single();
            }
            else
            {
                result.LocalAuthorityCode = null;
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
            var additionalQuestions = GetAdditionalQuestions(dbVacancy);
            result.FirstQuestion = GetAdditionalQuestion(additionalQuestions, FirstQuestionId);
            result.SecondQuestion = GetAdditionalQuestion(additionalQuestions, SecondQuestionId);
        }

        private static string GetAdditionalQuestion(IReadOnlyDictionary<int, string> additionalQuestions, int questionId)
        {
            return additionalQuestions.ContainsKey(questionId) ? additionalQuestions[questionId] : null;
        }

        private IReadOnlyDictionary<int,string> GetAdditionalQuestions(Vacancy dbVacancy)
        {
            var results = _getOpenConnection.Query<dynamic>(@"
                            SELECT QuestionId, Question
                            FROM   dbo.AdditionalQuestion
                            WHERE  VacancyId = @VacancyId
                            ORDER BY QuestionId ASC
                            ", new { dbVacancy.VacancyId }).ToDictionary(q => (int)q.QuestionId, q => (string)q.Question);
            return results;         
        }             

        private void MapTextFields(Vacancy dbVacancy, DomainVacancy result)
        {
            var textFields = GetTextFields(dbVacancy.VacancyId);
            result.TrainingProvided = GetTextField(textFields, TextFieldCodeName.TrainingProvided);
            result.DesiredQualifications = GetTextField(textFields, TextFieldCodeName.DesiredQualifications);
            result.DesiredSkills = GetTextField(textFields, TextFieldCodeName.DesiredSkills);
            result.PersonalQualities = GetTextField(textFields, TextFieldCodeName.PersonalQualities);
            result.ThingsToConsider = GetTextField(textFields, TextFieldCodeName.ThingsToConsider);
            result.FutureProspects = GetTextField(textFields, TextFieldCodeName.FutureProspects);
            result.OtherInformation = GetTextField(textFields, TextFieldCodeName.OtherInformation);
        }

        private void PatchTrainingType(DomainVacancy result)
        {
            if (result.TrainingType != TrainingType.Unknown) return;

            if (!string.IsNullOrWhiteSpace(result.SectorCodeName))
            {
                result.TrainingType = TrainingType.Sectors;
            }
            else if (result.StandardId != null)
            {
                result.TrainingType = TrainingType.Standards;
            }
            else if (!string.IsNullOrWhiteSpace(result.FrameworkCodeName))
            {
                result.TrainingType = TrainingType.Frameworks;
            }
        }

        private void PatchStandards(Vacancy dbVacancy, VacancySummary result)
        {
            if (dbVacancy.ApprenticeshipFrameworkId.HasValue)
            {
                var apprenticeshipOccupationId =
                    GetApprenticeshipOccupationIdFor(dbVacancy.ApprenticeshipFrameworkId.Value);

                if (apprenticeshipOccupationId == StandardsApprenticeshipOccupationId)
                {
                    // This is a vacancy created in AVMS with an standard codified as framework
                    result.TrainingType = TrainingType.Standards;
                    result.FrameworkCodeName = null;

                    // Try to get the standard id
                    var frameworkFullName = GetFrameworkFullNameFor(dbVacancy.ApprenticeshipFrameworkId.Value);
                    var sanitizedFrameworkFullName = SanitizeFrameworkFullName(frameworkFullName);

                    result.StandardId = GetStandardIdWithFullName(sanitizedFrameworkFullName);

                    _logger.Info(
                        result.StandardId.HasValue
                            ? $"Mapping a vacancy with standard codified as framework (AVMS Vacancy). FrameworkId was {dbVacancy.ApprenticeshipFrameworkId.Value} ({frameworkFullName}) and the new StandardId is {result.StandardId.Value}"
                            : $"Mapping a vacancy with standard codified as framework (AVMS Vacancy). FrameworkId was {dbVacancy.ApprenticeshipFrameworkId.Value} ({frameworkFullName}) but we haven't found any matching standard.");
                }
            }
        }

        private static string SanitizeFrameworkFullName(string frameworkFullName)
        {
            var sanitizedFrameworkFullName = frameworkFullName.LastIndexOf("(") == -1
                ? frameworkFullName
                : frameworkFullName.Substring(0, frameworkFullName.LastIndexOf("(")).Trim().ToLowerInvariant();

            //sanitizedFrameworkFullName = Regex.Replace(sanitizedFrameworkFullName, "\u00a0", " ");
            sanitizedFrameworkFullName = sanitizedFrameworkFullName.Replace("\u00a0", " ");

            return sanitizedFrameworkFullName;
        }

        private int GetApprenticeshipOccupationIdFor(int frameworkId)
        {
            return _getOpenConnection.QueryCached<int>(_cacheDuration, @"
SELECT ApprenticeshipOccupationId
FROM   dbo.ApprenticeshipFramework
WHERE  ApprenticeshipFrameworkId = @ApprenticeshipFrameworkId",
                new
                {
                    ApprenticeshipFrameworkId = frameworkId
                }).Single();
        }

        private string GetFrameworkFullNameFor(int frameworkId)
        {
            var dbFrameworkFullName = _getOpenConnection.QueryCached<string>(_cacheDuration, @"
SELECT FullName
FROM   dbo.ApprenticeshipFramework
WHERE  ApprenticeshipFrameworkId = @ApprenticeshipFrameworkId",
                new
                {
                    ApprenticeshipFrameworkId = frameworkId
                }).Single();

            // Consider edge cases
            return _standardsMap.ContainsKey(dbFrameworkFullName)
                ? _standardsMap[dbFrameworkFullName]
                : dbFrameworkFullName;
        }

        private int? GetStandardIdWithFullName(string fullName)
        {
            return _getOpenConnection.QueryCached<int?>(_cacheDuration, @"
SELECT StandardId
FROM   Reference.Standard
WHERE  FullName = @FrameworkFullName",
                new
                {
                    FrameworkFullName = fullName
                }).SingleOrDefault();
        }

        private string GetTextField(IReadOnlyDictionary<string, string> textFields, string codeName)
        {            
            return textFields.ContainsKey(codeName) ? textFields[codeName] : null;
        }

        private IReadOnlyDictionary<string, string> GetTextFields(int vacancyId)
        {            
            var textFields = _getOpenConnection.Query<dynamic>(@"
                                            SELECT CodeName, Value
                                            FROM   dbo.VacancyTextField VTF
                                            JOIN   dbo.VacancyTextFieldValue VTFV
                                            ON VTF.Field = VTFV.VacancyTextFieldValueId
                                            WHERE  VacancyId = @VacancyId
                                            ", new{
                                                        VacancyId = vacancyId                                                        
                                                    }).ToDictionary(tf => (string)tf.CodeName,tf => (string)tf.Value);
            return textFields;
        }

        private class Comment
        {
            public string CodeName { get; set; }
            public string Comments { get; set; }
        }

        private void MapComments(Vacancy dbVacancy, DomainVacancy result)
        {
            var comments = GetComments(dbVacancy.VacancyId);

            if (comments.Any())
            {
                result.TitleComment = GetComment(comments, ReferralCommentCodeName.TitleComment);
                result.ApprenticeshipLevelComment = GetComment(comments, ReferralCommentCodeName.ApprenticeshipLevelComment);
                result.ClosingDateComment = GetComment(comments, ReferralCommentCodeName.ClosingDateComment);
                result.ContactDetailsComment = GetComment(comments, ReferralCommentCodeName.ContactDetailsComment);
                result.DesiredQualificationsComment = GetComment(comments, ReferralCommentCodeName.DesiredQualificationsComment);
                result.DesiredSkillsComment = GetComment(comments, ReferralCommentCodeName.DesiredSkillsComment);
                result.DurationComment = GetComment(comments, ReferralCommentCodeName.DurationComment);
                result.EmployerDescriptionComment = GetComment(comments, ReferralCommentCodeName.EmployerDescriptionComment);
                result.EmployerWebsiteUrlComment = GetComment(comments, ReferralCommentCodeName.EmployerWebsiteUrlComment);
                result.FirstQuestionComment = GetComment(comments, ReferralCommentCodeName.FirstQuestionComment);
                result.SecondQuestionComment = GetComment(comments, ReferralCommentCodeName.SecondQuestionComment);
                result.FrameworkCodeNameComment = GetComment(comments, ReferralCommentCodeName.FrameworkCodeNameComment);
                result.FutureProspectsComment = GetComment(comments, ReferralCommentCodeName.FutureProspectsComment);
                result.LongDescriptionComment = GetComment(comments, ReferralCommentCodeName.LongDescriptionComment);
                result.NumberOfPositionsComment = GetComment(comments, ReferralCommentCodeName.NumberOfPositionsComment);
                result.OfflineApplicationInstructionsComment = GetComment(comments, ReferralCommentCodeName.OfflineApplicationInstructionsComment);
                result.OfflineApplicationUrlComment = GetComment(comments, ReferralCommentCodeName.OfflineApplicationUrlComment);
                result.PersonalQualitiesComment = GetComment(comments, ReferralCommentCodeName.PersonalQualitiesComment);
                result.PossibleStartDateComment = GetComment(comments, ReferralCommentCodeName.PossibleStartDateComment);
                result.SectorCodeNameComment = GetComment(comments, ReferralCommentCodeName.SectorCodeNameComment);
                result.ShortDescriptionComment = GetComment(comments, ReferralCommentCodeName.ShortDescriptionComment);
                result.StandardIdComment = GetComment(comments, ReferralCommentCodeName.StandardIdComment);
                result.ThingsToConsiderComment = GetComment(comments, ReferralCommentCodeName.ThingsToConsiderComment);
                result.TrainingProvidedComment = GetComment(comments, ReferralCommentCodeName.TrainingProvidedComment);
                result.WageComment = GetComment(comments, ReferralCommentCodeName.WageComment);
                result.WorkingWeekComment = GetComment(comments, ReferralCommentCodeName.WorkingWeekComment);
                result.LocationAddressesComment = GetComment(comments, ReferralCommentCodeName.LocationAddressesComment);
                result.AdditionalLocationInformationComment = GetComment(comments, ReferralCommentCodeName.AdditionalLocationInformationComment);
            }
        }

        private Dictionary<string, Comment> GetComments(int vacancyId)
        {
            return _getOpenConnection.Query<Comment>(@"
SELECT CodeName, Comments 
FROM VacancyReferralComments vfr
JOIN VacancyReferralCommentsFieldType vrcft on vfr.FieldTypeId = vrcft.VacancyReferralCommentsFieldTypeId
WHERE VacancyId = @VacancyId
", new
            {
                VacancyId = vacancyId
            }).ToDictionary(t => t.CodeName);
        }

        private string GetComment(Dictionary<string, Comment> results, string vacancyReferralCommentTypeCodeName)
        {
            return results.ContainsKey(vacancyReferralCommentTypeCodeName)
                ? results[vacancyReferralCommentTypeCodeName].Comments
                : null;
        }
        
        private void MapDateFirstSubmitted(Vacancy dbVacancy, VacancySummary result)
        {
            var vacancyPlus = dbVacancy as VacancyPlus;
            if (vacancyPlus != null)
            {
                result.DateFirstSubmitted = vacancyPlus.DateFirstSubmitted;
            }
            else
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
        }

        private void MapDateSubmitted(Vacancy dbVacancy, VacancySummary result)
        {
            var vacancyPlus = dbVacancy as VacancyPlus;
            if (vacancyPlus != null)
            {
                result.DateSubmitted = vacancyPlus.DateSubmitted;
            }
            else
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

        private void MapCreatedByProviderUsername(Vacancy dbVacancy, DomainVacancy result)
        {
            result.CreatedByProviderUsername = _getOpenConnection.Query<string>(@"
select top 1 UserName
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
            var vacancyPlus = dbVacancy as VacancyPlus;

            if (vacancyPlus != null)
            {
                result.DateQAApproved = vacancyPlus.DateQAApproved;
            }
            else
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
        }

        private void MapRegionalTeam(VacancySummary vacancySummary)
        {
            const string sql = @"
                SELECT ps.PostCode
                FROM dbo.Vacancy v
                INNER JOIN dbo.VacancyOwnerRelationship vor
                ON vor.VacancyOwnerRelationshipId = v.VacancyOwnerRelationshipId
                INNER JOIN dbo.ProviderSite ps
                ON ps.ProviderSiteId = vor.ProviderSiteId
                WHERE v.VacancyId = @vacancyId
                ";

            var sqlParams = new
            {
                vacancySummary.VacancyId,
                vacancySummary.OwnerPartyId
            };

            var postcode = _getOpenConnection.Query<string>(sql, sqlParams).FirstOrDefault();

            vacancySummary.RegionalTeam = RegionalTeamMapper.GetRegionalTeam(postcode);
        }

        private static void MapDuration(Vacancy dbVacancy, VacancySummary result)
        {
            result.DurationType = (DurationType)dbVacancy.DurationTypeId;
            result.Duration = dbVacancy.DurationValue;
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

            CreateVacancyHistoryRow(dbVacancy.VacancyId, _currentUserService.CurrentUserName, VacancyHistoryEventType.StatusChange,
                (int)entity.Status, StatusChangeText);

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
                VacancyHistoryEventTypeId = (int) vacancyHistoryEventType,
                VacancyHistoryEventSubTypeId = vacancyStatus,
                HistoryDate = _dateTimeService.UtcNow
            };

            _getOpenConnection.Insert(vacancyHistory);
        }

        public void Delete(int vacancyId)
        {
            throw new NotImplementedException();
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
WHERE  CodeName = @LocalAuthorityCode",
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

        private void PopulateVacancyLocationTypeId(DomainVacancy entity, Vacancy dbVacancy)
        {
            // A vacancy is multilocation if IsEmployerAddressMainAddress is set to false
            var vacancyLocationTypeCodeName = entity.IsEmployerLocationMainApprenticeshipLocation.HasValue && entity.IsEmployerLocationMainApprenticeshipLocation.Value
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
            if (entity.VacancyType == VacancyType.Traineeship)
            {
                dbVacancy.ApprenticeshipType = _getOpenConnection.QueryCached<int>(_cacheDuration, @"
SELECT ApprenticeshipTypeId
FROM   dbo.ApprenticeshipType
WHERE  CodeName = 'TRA'").Single();
                return;
            }

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

        private void UpsertTextField(int vacancyId, string vacancyTextFieldCodeName, string value)
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

            return MapVacancy(dbVacancy);
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

        public IReadOnlyDictionary<int, IEnumerable<IMinimalVacancyDetails>> GetMinimalVacancyDetails(IEnumerable<int> vacancyPartyIds)
        {            
            var vacancyCollections = new List<dynamic>();                               
            var partyIds = vacancyPartyIds as int[] ?? vacancyPartyIds.ToArray();
            var splitVacancyPartyIds = DbHelpers.SplitIds(partyIds);            
            foreach (var splitVacancyPartyId in splitVacancyPartyIds)
            {
                IList<dynamic> singleCollection = _getOpenConnection.Query<dynamic>(@"
                                SELECT VacancyId, VacancyOwnerRelationshipId, VacancyStatusId, ApplicationClosingDate, UpdatedDateTime, VacancyTypeId
                                FROM   dbo.Vacancy
                                WHERE  VacancyOwnerRelationshipId IN @Ids",
                    new {Ids = splitVacancyPartyId});                                                                                                      
                vacancyCollections.AddRange(singleCollection);                
            }
            return vacancyCollections
                .GroupBy(x => (int) x.VacancyOwnerRelationshipId)
                .ToDictionary(x => x.Key, x => x.Select(y => (IMinimalVacancyDetails)new MinimalVacancyDetails(y)));               
        }

        public IReadOnlyDictionary<int, IEnumerable<Domain.Entities.Raa.Locations.VacancyLocation>> GetVacancyLocationsByVacancyIds(IEnumerable<int> vacancyIds)
        {
            // TODO: Handle >2000 records - Shoma
            return _getOpenConnection.Query<Domain.Entities.Raa.Locations.VacancyLocation> (@"
                        SELECT *
                        FROM   dbo.VacancyLocation
                        WHERE  VacancyId IN @Ids",
                        new { Ids = vacancyIds })
                                    .GroupBy(x => x.VacancyId)
            .ToDictionary(x => x.Key, x => (IEnumerable<Domain.Entities.Raa.Locations.VacancyLocation>)x);
        }

        private class MinimalVacancyDetails : IMinimalVacancyDetails
        {
            public MinimalVacancyDetails(dynamic record)
            {
                VacancyId = record.VacancyId;
                OwnerPartyId = record.VacancyOwnerRelationshipId;
                Status = (VacancyStatus)record.VacancyStatusId;
                _closingDate = record.ApplicationClosingDate;

                // TODO: Should get from history because won't be set for old records, but this is an issue throughout
                SyntheticUpdatedDateTime = record.UpdatedDateTime ?? new DateTime(2016, 6, 1, 0, 0, 0, DateTimeKind.Utc);

                // TODO: Won't be set for uploaded vacancies, but this is an issue throughout
                VacancyType = (VacancyType)record.VacancyTypeId;
            }

            public int VacancyId { get; private set; }

            public int OwnerPartyId { get; private set; }

            private DateTime? _closingDate;
            public DateTime LiveClosingDate
            {
                get
                {
                    if (Status != VacancyStatus.Live && Status != VacancyStatus.Closed && Status != VacancyStatus.Completed)
                        throw new InvalidOperationException(Status.ToString());
                    if (_closingDate == null)
                        throw new InvalidOperationException($"Null closing date found for live vacancy {VacancyId}");
                    return _closingDate.Value;
                }
            }

            public VacancyStatus Status { get; private set; }

            public DateTime SyntheticUpdatedDateTime { get; private set; }

            public VacancyType VacancyType { get; private set; }
        }

        private class VacancyPlus : Vacancy
        {
            public DateTime? DateFirstSubmitted { get; set; }
            public DateTime? DateSubmitted { get; set; }
            public DateTime? DateQAApproved { get; set; }
        }
    }
}