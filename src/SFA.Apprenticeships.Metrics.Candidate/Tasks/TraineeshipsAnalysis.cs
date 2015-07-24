namespace SFA.Apprenticeships.Metrics.Candidate.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using CsvHelper;
    using Extensions;
    using Infrastructure.Monitor.Repositories;

    public class TraineeshipsAnalysis : IMetricsTask
    {
        private readonly IUserMetricsRepository _userMetricsRepository;
        private readonly ICandidateMetricsRepository _candidateMetricsRepository;
        private readonly IApprenticeshipMetricsRepository _apprenticeshipMetricsRepository;
        private readonly ITraineeshipMetricsRepository _traineeshipMetricsRepository;

        public TraineeshipsAnalysis(IUserMetricsRepository userMetricsRepository, ICandidateMetricsRepository candidateMetricsRepository, IApprenticeshipMetricsRepository apprenticeshipMetricsRepository, ITraineeshipMetricsRepository traineeshipMetricsRepository)
        {
            _userMetricsRepository = userMetricsRepository;
            _candidateMetricsRepository = candidateMetricsRepository;
            _apprenticeshipMetricsRepository = apprenticeshipMetricsRepository;
            _traineeshipMetricsRepository = traineeshipMetricsRepository;
        }

        public string TaskName
        {
            get { return "Traineeships Analysis"; }
        }

        public void Run()
        {
            var userApplicationMetrics = new Dictionary<Guid, UserApplicationMetrics>();

            AddUserActivityMetrics(userApplicationMetrics);

            AddApprenticeshipApplicationStatusCounts(userApplicationMetrics);

            AddTraineeshipApplicationStatusCounts(userApplicationMetrics);

            AddTraineeshipPromptMetrics(userApplicationMetrics);

            var apprenticeshipApplicationMetrics = GetApprenticeshipApplicationMetrics();

            var traineeshipApplicationMetrics = GetTraineeshipApplicationMetrics();

            var fileDateTime = DateTime.Now;
            WriteTraineeshipAnalysisCsv(userApplicationMetrics, fileDateTime);
            WriteApprenticeshipApplicationMetricsCsv(apprenticeshipApplicationMetrics, fileDateTime);
            WriteTraineeshipApplicationMetricsCsv(traineeshipApplicationMetrics, fileDateTime);
        }

        private static void WriteTraineeshipAnalysisCsv(Dictionary<Guid, UserApplicationMetrics> userApplicationMetrics, DateTime fileDateTime)
        {
            var fileName = string.Format("TraineeshipAnalysis_{0}.csv", fileDateTime.ToString("s").Replace(":", "-"));
            var textWriter = new StreamWriter(fileName);
            var csv = new CsvWriter(textWriter);
            csv.WriteRecords(userApplicationMetrics.Values);
            textWriter.Flush();
            textWriter.Close();
        }

        private static void WriteApprenticeshipApplicationMetricsCsv(ApprenticeshipApplicationMetrics userApplicationMetrics, DateTime fileDateTime)
        {
            var fileName = string.Format("ApprenticeshipApplicationMetrics_{0}.csv", fileDateTime.ToString("s").Replace(":", "-"));
            var textWriter = new StreamWriter(fileName);
            var csv = new CsvWriter(textWriter);
            csv.WriteRecords(userApplicationMetrics.ApprenticeshipMetrics.Values);
            textWriter.Flush();
            textWriter.Close();
        }

        private static void WriteTraineeshipApplicationMetricsCsv(TraineeshipApplicationMetrics userApplicationMetrics, DateTime fileDateTime)
        {
            var fileName = string.Format("TraineeshipApplicationMetrics_{0}.csv", fileDateTime.ToString("s").Replace(":", "-"));
            var textWriter = new StreamWriter(fileName);
            var csv = new CsvWriter(textWriter);
            csv.WriteRecords(userApplicationMetrics.TraineeshipMetrics.Values);
            textWriter.Flush();
            textWriter.Close();
        }

        private void AddUserActivityMetrics(Dictionary<Guid, UserApplicationMetrics> userApplicationMetrics)
        {
            var dateTime = DateTime.UtcNow;
            while (dateTime > Constants.OldestValidDate)
            {
                var userActivityMetrics = _userMetricsRepository.GetUserActivityMetrics(dateTime.AddDays(-30), dateTime);
                foreach (var userActivityMetric in userActivityMetrics)
                {
                    var groupComponents = userActivityMetric["_id"].AsBsonDocument;
                    var candidateId = groupComponents["CandidateId"].AsGuid;
                    var dateCreated = groupComponents["DateCreated"].ToUniversalTime();
                    var activated = userActivityMetric["Activated"].AsBoolean;
                    var activateCodeExpiry = groupComponents["ActivateCodeExpiry"].ToNullableUniversalTime();
                    var activationDate = groupComponents.ToNullableUniversalTime("ActivationDate");
                    var lastLogin = groupComponents.ToNullableUniversalTime("LastLogin");

                    userApplicationMetrics[candidateId] = new UserApplicationMetrics
                    {
                        CandidateId = candidateId,
                        DateCreated = dateCreated,
                        Activated = activated,
                        ActivateCodeExpiry = activateCodeExpiry,
                        ActivationDate = activationDate,
                        LastLogin = lastLogin
                    };
                }

                dateTime = dateTime.AddDays(-30);
            }
        }

        private void AddApprenticeshipApplicationStatusCounts(Dictionary<Guid, UserApplicationMetrics> userApplicationMetrics)
        {
            var apprenticeshipApplicationsStatusCounts = _apprenticeshipMetricsRepository.GetApplicationStatusCounts();

            foreach (var apprenticeshipApplicationsStatusCount in apprenticeshipApplicationsStatusCounts)
            {
                var candidateId = apprenticeshipApplicationsStatusCount["_id"].AsGuid;
                var savedApplicationCount = apprenticeshipApplicationsStatusCount["Saved"].AsInt32;
                var draftApplicationCount = apprenticeshipApplicationsStatusCount["Draft"].AsInt32;
                var submittedApplicationCount = apprenticeshipApplicationsStatusCount["Submitted"].AsInt32;
                var unsuccessfulApplicationCount = apprenticeshipApplicationsStatusCount["Unsuccessful"].AsInt32;
                var successfulApplicationCount = apprenticeshipApplicationsStatusCount["Successful"].AsInt32;

                var apprenticeshipApplicationMetrics = new CandidateApprenticeshipApplicationMetrics
                {
                    Saved = savedApplicationCount,
                    Draft = draftApplicationCount,
                    Submitted = submittedApplicationCount,
                    Unsuccessful = unsuccessfulApplicationCount,
                    Successful = successfulApplicationCount,
                };

                if (userApplicationMetrics.ContainsKey(candidateId))
                {
                    userApplicationMetrics[candidateId].CandidateApprenticeshipApplicationMetrics = apprenticeshipApplicationMetrics;
                }
                else
                {
                    userApplicationMetrics[candidateId] = new UserApplicationMetrics
                    {
                        CandidateApprenticeshipApplicationMetrics = apprenticeshipApplicationMetrics
                    };
                }
            }
        }

        private void AddTraineeshipApplicationStatusCounts(Dictionary<Guid, UserApplicationMetrics> userApplicationMetrics)
        {
            var traineeshipApplicationsStatusCounts = _traineeshipMetricsRepository.GetApplicationStatusCounts();

            foreach (var traineeshipApplicationsStatusCount in traineeshipApplicationsStatusCounts)
            {
                var candidateId = traineeshipApplicationsStatusCount["_id"].AsGuid;
                var submittedApplicationCount = traineeshipApplicationsStatusCount["Submitted"].AsInt32;
                var unsuccessfulApplicationCount = traineeshipApplicationsStatusCount["Unsuccessful"].AsInt32;
                var successfulApplicationCount = traineeshipApplicationsStatusCount["Successful"].AsInt32;

                var traineeshipApplicationMetrics = new CandidateTraineeshipApplicationMetrics
                {
                    Submitted = submittedApplicationCount,
                    Unsuccessful = unsuccessfulApplicationCount,
                    Successful = successfulApplicationCount
                };

                userApplicationMetrics[candidateId].CandidateTraineeshipApplicationMetrics = traineeshipApplicationMetrics;
            }
        }

        private void AddTraineeshipPromptMetrics(Dictionary<Guid, UserApplicationMetrics> userApplicationMetrics)
        {
            var candidatesThatWouldHaveSeenTraineeshipPrompt = _apprenticeshipMetricsRepository.GetCandidatesThatWouldHaveSeenTraineeshipPrompt();
            foreach (var candidateId in candidatesThatWouldHaveSeenTraineeshipPrompt)
            {
                userApplicationMetrics[candidateId].WouldHaveSeenTraineeshipPrompt = true;
            }

            var candidatesThatHaveDismissedTheTraineeshipPrompt = _candidateMetricsRepository.GetCandidatesThatHaveDismissedTheTraineeshipPrompt();
            foreach (var candidateId in candidatesThatHaveDismissedTheTraineeshipPrompt)
            {
                userApplicationMetrics[candidateId].HaveDismissedTheTraineeshipPrompt = true;
            }
        }

        private ApprenticeshipApplicationMetrics GetApprenticeshipApplicationMetrics()
        {
            var averageApplicationCountPerApprenticeship = _apprenticeshipMetricsRepository.GetAverageApplicationCountPerApprenticeship();
            var apprenticeshipApplicationMetrics = new ApprenticeshipApplicationMetrics
            {
                ApprenticeshipsWithApplicationsCount = averageApplicationCountPerApprenticeship["apprenticeshipsWithApplicationsCount"].AsInt32,
                TotalApplicationsCount = averageApplicationCountPerApprenticeship["count"].AsInt32,
                AverageApplicationsPerApprenticeship = averageApplicationCountPerApprenticeship["average"].AsDouble
            };

            var applicationCountPerApprenticeship = _apprenticeshipMetricsRepository.GetApplicationCountPerApprenticeship();
            foreach (var applicationCount in applicationCountPerApprenticeship)
            {
                var vacancyId = applicationCount["_id"]["VacancyId"].AsInt32;
                var title = applicationCount["_id"]["Title"].AsString;
                var count = applicationCount["count"].AsInt32;

                var apprenticeshipMetrics = new ApprenticeshipMetrics
                {
                    Id = vacancyId,
                    Title = title,
                    ApplicationCount = count
                };

                apprenticeshipApplicationMetrics.ApprenticeshipMetrics[vacancyId] = apprenticeshipMetrics;
            }
            return apprenticeshipApplicationMetrics;
        }

        private TraineeshipApplicationMetrics GetTraineeshipApplicationMetrics()
        {
            var averageApplicationCountPerTraineeship = _traineeshipMetricsRepository.GetAverageApplicationCountPerTraineeship();
            var traineeshipApplicationMetrics = new TraineeshipApplicationMetrics
            {
                TraineeshipsWithApplicationsCount = averageApplicationCountPerTraineeship["traineeshipsWithApplicationsCount"].AsInt32,
                TotalApplicationsCount = averageApplicationCountPerTraineeship["count"].AsInt32,
                AverageApplicationsPerTraineeship = averageApplicationCountPerTraineeship["average"].AsDouble
            };

            var applicationCountPerTraineeship = _traineeshipMetricsRepository.GetApplicationCountPerTraineeship();
            foreach (var applicationCount in applicationCountPerTraineeship)
            {
                var vacancyId = applicationCount["_id"]["VacancyId"].AsInt32;
                var title = applicationCount["_id"]["Title"].AsString;
                var count = applicationCount["count"].AsInt32;

                var traineeshipMetrics = new TraineeshipMetrics
                {
                    Id = vacancyId,
                    Title = title,
                    ApplicationCount = count
                };

                traineeshipApplicationMetrics.TraineeshipMetrics[vacancyId] = traineeshipMetrics;
            }
            return traineeshipApplicationMetrics;
        }
    }

    public class UserApplicationMetrics
    {
        public Guid CandidateId { get; set; }
        public DateTime DateCreated { get; set; }
        public bool Activated { get; set; }
        public DateTime? ActivateCodeExpiry { get; set; }
        public DateTime? ActivationDate { get; set; }
        public DateTime? LastLogin { get; set; }
        public CandidateApprenticeshipApplicationMetrics CandidateApprenticeshipApplicationMetrics { get; set; }
        public CandidateTraineeshipApplicationMetrics CandidateTraineeshipApplicationMetrics { get; set; }
        public bool WouldHaveSeenTraineeshipPrompt { get; set; }
        public bool HaveDismissedTheTraineeshipPrompt { get; set; }
    }

    public class CandidateApprenticeshipApplicationMetrics
    {
        public int Saved { get; set; }
        public int Draft { get; set; }
        public int Submitted { get; set; }
        public int Unsuccessful { get; set; }
        public int Successful { get; set; }

        public int Total
        {
            get { return Saved + Draft + Submitted + Unsuccessful + Successful; }
        }
    }

    public class CandidateTraineeshipApplicationMetrics
    {
        public int Submitted { get; set; }
        public int Unsuccessful { get; set; }
        public int Successful { get; set; }

        public int Total
        {
            get { return Submitted + Unsuccessful + Successful; }
        }
    }

    public class ApprenticeshipApplicationMetrics
    {
        public ApprenticeshipApplicationMetrics()
        {
            ApprenticeshipMetrics = new Dictionary<int, ApprenticeshipMetrics>();
        }

        public int ApprenticeshipsWithApplicationsCount { get; set; }
        public int TotalApplicationsCount { get; set; }
        public double AverageApplicationsPerApprenticeship { get; set; }
        public IDictionary<int, ApprenticeshipMetrics> ApprenticeshipMetrics { get; private set; }
    }

    public class ApprenticeshipMetrics
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int ApplicationCount { get; set; }
    }

    public class TraineeshipApplicationMetrics
    {
        public TraineeshipApplicationMetrics()
        {
            TraineeshipMetrics = new Dictionary<int, TraineeshipMetrics>();
        }

        public int TraineeshipsWithApplicationsCount { get; set; }
        public int TotalApplicationsCount { get; set; }
        public double AverageApplicationsPerTraineeship { get; set; }
        public IDictionary<int, TraineeshipMetrics> TraineeshipMetrics { get; set; }
    }

    public class TraineeshipMetrics
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int ApplicationCount { get; set; }
    }
}