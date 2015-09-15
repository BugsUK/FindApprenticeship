namespace SFA.Apprenticeships.Metrics.Candidate.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using CsvHelper;
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using Extensions;
    using Infrastructure.Monitor.Repositories;

    public class EShot : IMetricsTask
    {
        private readonly IUserMetricsRepository _userMetricsRepository;
        private readonly ICandidateMetricsRepository _candidateMetricsRepository;
        private readonly IApprenticeshipMetricsRepository _apprenticeshipMetricsRepository;
        private readonly ITraineeshipMetricsRepository _traineeshipMetricsRepository;

        private readonly Dictionary<int, string> _ethnicity = new Dictionary<int, string>
        {
            {0, "Not Specified"},
            {99, "Prefer not to say"},
            {31, "English / Welsh / Scottish / Northern Irish / British"},
            {32, "Irish"},
            {33, "Gypsy or Irish Traveller"},
            {34, "Any other White background"},
            {35, "White and Black Caribbean"},
            {36, "White and Black African"},
            {37, "White and Asian"},
            {38, "Any other Mixed / Multiple ethnic background"},
            {39, "Indian"},
            {40, "Pakistani"},
            {41, "Bangladeshi"},
            {42, "Chinese"},
            {43, "Any other Asian background"},
            {44, "African"},
            {45, "Caribbean"},
            {46, "Any other Black / African / Caribbean background"},
            {47, "Arab"},
            {98, "Any other ethnic group"}
        };

        public EShot(IUserMetricsRepository userMetricsRepository, ICandidateMetricsRepository candidateMetricsRepository, IApprenticeshipMetricsRepository apprenticeshipMetricsRepository, ITraineeshipMetricsRepository traineeshipMetricsRepository)
        {
            _userMetricsRepository = userMetricsRepository;
            _candidateMetricsRepository = candidateMetricsRepository;
            _apprenticeshipMetricsRepository = apprenticeshipMetricsRepository;
            _traineeshipMetricsRepository = traineeshipMetricsRepository;
        }

        public string TaskName { get { return "e-shot"; } }

        public void Run()
        {
            var eShotMetrics = new Dictionary<Guid, EShotMetrics>();

            AddUserActivityMetrics(eShotMetrics);

            AddCandidateActivityMetrics(eShotMetrics);

            AddApplicationMetrics(eShotMetrics);

            var fileDateTime = DateTime.UtcNow;
            WriteEShotCsv(eShotMetrics, fileDateTime);
        }

        private static void WriteEShotCsv(Dictionary<Guid, EShotMetrics> eShotMetrics, DateTime fileDateTime)
        {
            var fileName = string.Format("FAA-candidates-{0}.csv", fileDateTime.ToString("yyyyMMdd"));
            var textWriter = new StreamWriter(fileName);
            var csv = new CsvWriter(textWriter);
            csv.WriteRecords(eShotMetrics.Values);
            textWriter.Flush();
            textWriter.Close();
        }

        private void AddUserActivityMetrics(Dictionary<Guid, EShotMetrics> eShotMetrics)
        {
            var dateTime = DateTime.UtcNow;
            while (dateTime > Constants.OldestValidDate)
            {
                var userActivityMetrics = _userMetricsRepository.GetUserActivityMetrics(dateTime.AddDays(-30), dateTime);
                foreach (var userActivityMetric in userActivityMetrics)
                {
                    var groupComponents = userActivityMetric["_id"].AsBsonDocument;
                    var candidateId = groupComponents["CandidateId"].AsGuid;
                    var status = (UserStatuses)groupComponents["Status"].AsInt32;
                    var lastLogin = groupComponents.ToNullableUniversalTime("LastLogin");

                    if (status == UserStatuses.Active || status == UserStatuses.Inactive || status == UserStatuses.Locked || status == UserStatuses.Dormant)
                    {
                        //Only store if considered active
                        eShotMetrics[candidateId] = new EShotMetrics
                        {
                            Status = status,
                            LastLogin = lastLogin
                        };
                    }
                }

                dateTime = dateTime.AddDays(-30);
            }
        }

        private void AddCandidateActivityMetrics(Dictionary<Guid, EShotMetrics> eShotMetrics)
        {
            var dateTime = DateTime.UtcNow;
            while (dateTime > Constants.OldestValidDate)
            {
                var candidateActivityMetrics = _candidateMetricsRepository.GetCandidateActivityMetrics(dateTime.AddDays(-30), dateTime);
                foreach (var candidate in candidateActivityMetrics)
                {
                    if (eShotMetrics.ContainsKey(candidate.EntityId))
                    {
                        var marketingPreferences = candidate.CommunicationPreferences.MarketingPreferences;
                        if (marketingPreferences.EnableEmail || (marketingPreferences.EnableText && candidate.CommunicationPreferences.VerifiedMobile))
                        {
                            //At least one form of marketing communications is enabled
                            var metrics = eShotMetrics[candidate.EntityId];
                            metrics.FirstName = candidate.RegistrationDetails.FirstName;
                            metrics.LastName = candidate.RegistrationDetails.LastName;
                            metrics.AddressLine1 = candidate.RegistrationDetails.Address.AddressLine1;
                            metrics.AddressLine2 = candidate.RegistrationDetails.Address.AddressLine2;
                            metrics.AddressLine3 = candidate.RegistrationDetails.Address.AddressLine3;
                            metrics.AddressLine4 = candidate.RegistrationDetails.Address.AddressLine4;
                            metrics.Postcode = candidate.RegistrationDetails.Address.Postcode;
                            metrics.DateOfBirth = candidate.RegistrationDetails.DateOfBirth.ToString("dd/MM/yyy");
                            if (candidate.MonitoringInformation != null)
                            {
                                metrics.Gender = candidate.MonitoringInformation.Gender;
                            }
                            metrics.PhoneNumber = "=\"" + candidate.RegistrationDetails.PhoneNumber + "\"";
                            metrics.AllowMarketingTexts = candidate.CommunicationPreferences.MarketingPreferences.EnableText && candidate.CommunicationPreferences.VerifiedMobile;
                            metrics.EmailAddress = candidate.RegistrationDetails.EmailAddress;
                            metrics.AllowMarketingEmails = candidate.CommunicationPreferences.MarketingPreferences.EnableEmail;
                            metrics.SubscriberId = candidate.SubscriberId;
                            if (candidate.MonitoringInformation != null && candidate.MonitoringInformation.Ethnicity != null)
                            {
                                metrics.Ethnicity = _ethnicity[candidate.MonitoringInformation.Ethnicity.Value];
                            }
                            metrics.DateRegistered = candidate.DateCreated;
                            if(candidate.ApplicationTemplate.EducationHistory != null) {
                                metrics.LastSchool = candidate.ApplicationTemplate.EducationHistory.Institution;
                                metrics.LastSchoolFromYear = candidate.ApplicationTemplate.EducationHistory.FromYear;
                                metrics.LastSchoolToYear = candidate.ApplicationTemplate.EducationHistory.ToYear;
                            }
                        }
                        else
                        {
                            eShotMetrics.Remove(candidate.EntityId);
                        }
                    }
                }

                dateTime = dateTime.AddDays(-30);
            }
        }

        private void AddApplicationMetrics(Dictionary<Guid, EShotMetrics> eShotMetrics)
        {
            var dateTime = DateTime.UtcNow;
            while (dateTime > Constants.OldestValidDate)
            {
                var unsubmittedApplicationsCountPerCandidate = _apprenticeshipMetricsRepository.GetUnsubmittedApplicationsCountPerCandidate(dateTime.AddDays(-30), dateTime);
                foreach (var unsubmittedApplicationsCount in unsubmittedApplicationsCountPerCandidate)
                {
                    var idComponents = unsubmittedApplicationsCount["_id"].AsBsonDocument;
                    var candidateId = idComponents["CandidateId"].AsGuid;
                    var count = unsubmittedApplicationsCount["count"].AsInt32;

                    if (eShotMetrics.ContainsKey(candidateId))
                    {
                        eShotMetrics[candidateId].ApprenticeshipDraftCount += count;
                    }
                }
                var submittedApplicationsCountPerCandidate = _apprenticeshipMetricsRepository.GetSubmittedApplicationsCountPerCandidate(dateTime.AddDays(-30), dateTime);
                foreach (var submittedApplicationsCount in submittedApplicationsCountPerCandidate)
                {
                    var idComponents = submittedApplicationsCount["_id"].AsBsonDocument;
                    var candidateId = idComponents["CandidateId"].AsGuid;
                    var count = submittedApplicationsCount["count"].AsInt32;

                    if (eShotMetrics.ContainsKey(candidateId))
                    {
                        eShotMetrics[candidateId].ApprenticeshipSubmittedCount += count;
                    }
                }
                var submittedTraineeshipApplicationsCountPerCandidate = _traineeshipMetricsRepository.GetSubmittedApplicationsCountPerCandidate(dateTime.AddDays(-30), dateTime);
                foreach (var submittedApplicationsCount in submittedTraineeshipApplicationsCountPerCandidate)
                {
                    var idComponents = submittedApplicationsCount["_id"].AsBsonDocument;
                    var candidateId = idComponents["CandidateId"].AsGuid;
                    var count = submittedApplicationsCount["count"].AsInt32;

                    if (eShotMetrics.ContainsKey(candidateId))
                    {
                        eShotMetrics[candidateId].TraineeshipSubmittedCount += count;
                    }
                }

                dateTime = dateTime.AddDays(-30);
            }
        }
    }

    public class EShotMetrics
    {
        public UserStatuses Status { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string AddressLine4 { get; set; }
        public string Postcode { get; set; }
        public string DateOfBirth { get; set; }
        public Gender? Gender { get; set; }
        public string PhoneNumber { get; set; }
        public bool AllowMarketingTexts { get; set; }
        public string EmailAddress { get; set; }
        public bool AllowMarketingEmails { get; set; }
        public Guid SubscriberId { get; set; }
        public string Ethnicity { get; set; }
        public DateTime DateRegistered { get; set; }
        public DateTime? LastLogin { get; set; }
        public string LastSchool { get; set; }
        public int LastSchoolFromYear { get; set; }
        public int LastSchoolToYear { get; set; }
        public int ApprenticeshipDraftCount { get; set; }
        public int ApprenticeshipSubmittedCount { get; set; }
        public int TraineeshipSubmittedCount { get; set; }
    }
}