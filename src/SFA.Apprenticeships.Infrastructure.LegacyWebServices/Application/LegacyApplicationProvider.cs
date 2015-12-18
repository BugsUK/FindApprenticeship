namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.Application
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Apprenticeships.Application.Candidate;
    using Apprenticeships.Application.Interfaces.Applications;
    using SFA.Infrastructure.Interfaces;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Entities.Exceptions;
    using Domain.Interfaces.Repositories;
    using GatewayServiceProxy;
    using Newtonsoft.Json;
    using Wcf;

    public class LegacyApplicationProvider : ILegacyApplicationProvider
    {
        private readonly ILogService _logger;
        private readonly IWcfService<GatewayServiceContract> _service;
        private readonly ICandidateReadRepository _candidateReadRepository;

        public LegacyApplicationProvider(
            IWcfService<GatewayServiceContract> service,
            ICandidateReadRepository candidateReadRepository,
            ILogService logger)
        {
            _service = service;
            _candidateReadRepository = candidateReadRepository;
            _logger = logger;
        }

        public int CreateApplication(ApprenticeshipApplicationDetail apprenticeshipApplicationDetail)
        {
            var legacyCandidateId = GetLegacyCandidateId(apprenticeshipApplicationDetail.CandidateId);
            var request = CreateRequest(apprenticeshipApplicationDetail, legacyCandidateId);

            return InternalCreateApplication(apprenticeshipApplicationDetail.CandidateId, request);
        }

        public int CreateApplication(TraineeshipApplicationDetail traineeshipApplicationDetail)
        {
            var legacyCandidateId = GetLegacyCandidateId(traineeshipApplicationDetail.CandidateId);
            var request = CreateRequest(traineeshipApplicationDetail, legacyCandidateId);

            return InternalCreateApplication(traineeshipApplicationDetail.CandidateId, request);
        }

        #region Helpers

        private int InternalCreateApplication(Guid candidateId, CreateApplicationRequest request)
        {
            var context = new {candidateId, vacancyId = request.Application.VacancyId};

            try
            {
                _logger.Debug(
                    "Calling Legacy.CreateApplication for candidate id='{0}' and apprenticeship vacancy id='{1}'",
                    candidateId, request.Application.VacancyId);

                var legacyApplicationId = SendRequest(candidateId, request);

                _logger.Debug(
                    "Legacy.CreateApplication succeeded for candidate id='{0}', apprenticeship vacancy id='{1}', legacy application id='{2}'",
                    candidateId, request.Application.VacancyId,
                    legacyApplicationId);

                return legacyApplicationId;
            }
            catch (DomainException e)
            {
                if (e.Code == ErrorCodes.ApplicationCreationFailed)
                    _logger.Error(e);
                else
                    _logger.Warn(e);

                throw;
            }
            catch (BoundaryException e)
            {
                _logger.Error(e, context);
                throw new DomainException(ErrorCodes.ApplicationCreationFailed, e, context);
            }
            catch (Exception e)
            {
                _logger.Error(e, context);
                throw;
            }
        }

        private int SendRequest(Guid candidateId, CreateApplicationRequest request)
        {
            CreateApplicationResponse response = null;

            _service.Use("SecureService", client => response = client.CreateApplication(request));

            if (response == null || (response.ValidationErrors != null && response.ValidationErrors.Any()))
            {
                string message;
                string errorCode;

                if (response == null)
                {
                    message = "No response";
                    errorCode = ErrorCodes.ApplicationCreationFailed;
                }
                else if (IsDuplicateError(response))
                {
                    message = "Duplicate application";
                    errorCode = ErrorCodes.ApplicationDuplicatedError;
                }
                else
                {
                    ParseValidationError(response, out message, out errorCode);
                }

                throw new DomainException(errorCode, new { message, candidateId, vacancyId = request.Application.VacancyId });
            }

            return response.ApplicationId;
        }


        private static bool IsDuplicateError(CreateApplicationResponse response)
        {
            return response.ValidationErrors.Any(e => e.ErrorCode == ValidationErrorCodes.DuplicateApplication);
        }

        private static void ParseValidationError(CreateApplicationResponse response, out string message, out string errorCode)
        {
            var map = new Dictionary<string, string>
            {
                { ValidationErrorCodes.InvalidCandidateState, Apprenticeships.Application.Interfaces.Candidates.ErrorCodes.CandidateStateError },
                { ValidationErrorCodes.CandidateNotFound, Apprenticeships.Application.Interfaces.Candidates.ErrorCodes.CandidateNotFoundError },
                { ValidationErrorCodes.UnknownCandidate, Apprenticeships.Application.Interfaces.Candidates.ErrorCodes.CandidateNotFoundError },
                { ValidationErrorCodes.InvalidVacancyState, Apprenticeships.Application.Interfaces.Vacancies.ErrorCodes.LegacyVacancyStateError },
                { ValidationErrorCodes.SchoolNotEntered, Apprenticeships.Application.Interfaces.Vacancies.ErrorCodes.LegacyVacancyStateError }
            };

            foreach (var pair in map)
            {
                var validationError = response.ValidationErrors.FirstOrDefault(each => each.ErrorCode == pair.Key);

                if (validationError != null)
                {
                    message = string.Format("{0} (ErrorCode='{1}')", validationError.Message, pair.Key);
                    errorCode = pair.Value;
                    return;
                }
            }

            // Failed to parse expected validation error.
            message = string.Format("{0} unexpected validation error(s): {1}",
                response.ValidationErrors.Count(), JsonConvert.SerializeObject(response, Formatting.None));

            errorCode = ErrorCodes.ApplicationCreationFailed;
        }

        private static CreateApplicationRequest CreateRequest(
            ApprenticeshipApplicationDetail apprenticeshipApplicationDetail,
            int legacyCandidateId)
        {
            var candidateInformation = apprenticeshipApplicationDetail.CandidateInformation;

            return new CreateApplicationRequest
            {
                Application = new Application
                {
                    VacancyId = apprenticeshipApplicationDetail.Vacancy.Id,
                    VacancyRef = null, // not required if VacancyId is supplied.
                    CandidateId = legacyCandidateId,
                    School = MapSchool(apprenticeshipApplicationDetail),
                    EducationResults = MapQualifications(candidateInformation.Qualifications),
                    WorkExperiences = MapWorkExperience(candidateInformation.WorkExperience, candidateInformation.TrainingCourses),
                    AdditionalQuestion1Answer = apprenticeshipApplicationDetail.AdditionalQuestion1Answer ?? string.Empty,
                    AdditionalQuestion2Answer = apprenticeshipApplicationDetail.AdditionalQuestion2Answer ?? string.Empty,
                    Strengths = candidateInformation.AboutYou.Strengths ?? string.Empty,
                    Improvements = candidateInformation.AboutYou.Improvements ?? string.Empty,
                    HobbiesAndInterests = candidateInformation.AboutYou.HobbiesAndInterests ?? string.Empty,
                    InterviewSupport = candidateInformation.AboutYou.Support ?? string.Empty
                }
            };
        }

        private static CreateApplicationRequest CreateRequest(
            TraineeshipApplicationDetail traineeshipApplicationDetail,
            int legacyCandidateId)
        {
            var candidateInformation = traineeshipApplicationDetail.CandidateInformation;

            return new CreateApplicationRequest
            {
                Application = new Application
                {
                    VacancyId = traineeshipApplicationDetail.Vacancy.Id,
                    VacancyRef = null, // not required if VacancyId is supplied.
                    CandidateId = legacyCandidateId,
                    School = MapSchool(),
                    EducationResults = MapQualifications(candidateInformation.Qualifications),
                    WorkExperiences = MapWorkExperience(candidateInformation.WorkExperience, candidateInformation.TrainingCourses),
                    AdditionalQuestion1Answer = traineeshipApplicationDetail.AdditionalQuestion1Answer ?? string.Empty,
                    AdditionalQuestion2Answer = traineeshipApplicationDetail.AdditionalQuestion2Answer ?? string.Empty
                }
            };
        }

        private static School MapSchool(ApplicationDetail applicationDetail)
        {
            var educationHistory = applicationDetail.CandidateInformation.EducationHistory;

            if (educationHistory == null)
            {
                return null;
            }

            return new School
            {
                Name = educationHistory.Institution,
                Town = null,
                AttendedFrom = MapYearToDate(educationHistory.FromYear),
                AttendedTo = MapYearToDate(educationHistory.ToYear)
            };
        }

        private static School MapSchool()
        {
            var fakeAttendanceYear = MapYearToDate(2000);

            return new School
            {
                Name = "N/A",
                AttendedFrom = fakeAttendanceYear,
                AttendedTo = fakeAttendanceYear
            };
        }

        private static EducationResult[] MapQualifications(IEnumerable<Qualification> qualifications)
        {
            const int maxLevelLength = 100;

            return qualifications.Select(each => new EducationResult
            {
                Subject = each.Subject,
                DateAchieved = MapYearToDate(each.Year),
                Level = each.QualificationType.Substring(0, Math.Min(each.QualificationType.Length, maxLevelLength)),
                Grade = MapGrade(each.Grade, each.IsPredicted)
            }).ToArray();
        }

        private static string MapGrade(string grade, bool isPredicted)
        {
            return isPredicted ? string.Format("{0}-Pred", grade) : grade;
        }

        private static GatewayServiceProxy.WorkExperience[] MapWorkExperience(
            IEnumerable<Domain.Entities.Candidates.WorkExperience> workExperience,
            IEnumerable<TrainingCourse> trainingCourses)
        {
            return workExperience.Select(each => new GatewayServiceProxy.WorkExperience
            {
                Employer = each.Employer,
                FromDate = each.FromDate,
                ToDate = each.ToDate,
                TypeOfWork = MapWorkExperienceDescription(each.Description),
                PartialCompletion = false, // no mapping available.
                Voluntary = false // no mapping available.
            })
            .Union(trainingCourses.Select(each => new GatewayServiceProxy.WorkExperience
            {
                Employer = each.Provider,
                FromDate = each.FromDate,
                ToDate = each.ToDate,
                TypeOfWork = MapWorkExperienceDescription(each.Title),
                PartialCompletion = false, // no mapping available.
                Voluntary = false // no mapping available.
            }))
            .ToArray();
        }

        private static string MapWorkExperienceDescription(string description)
        {
            const int maxTypeOfWorkLength = 200;

            return description.Substring(0, Math.Min(description.Length, maxTypeOfWorkLength));
        }

        private static DateTime MapYearToDate(int year)
        {
            return new DateTime(year, 1, 1);
        }

        private int GetLegacyCandidateId(Guid candidateId)
        {
            return _candidateReadRepository.Get(candidateId, true).LegacyCandidateId;
        }

        #endregion
    }
}