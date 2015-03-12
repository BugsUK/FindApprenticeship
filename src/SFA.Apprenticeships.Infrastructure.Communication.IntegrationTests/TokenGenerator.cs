namespace SFA.Apprenticeships.Infrastructure.Communication.IntegrationTests
{
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Communications;
    using Domain.Entities.Communication;
    using Newtonsoft.Json;
    using Ploeh.AutoFixture;

    public static class TokenGenerator
    {

        private const string TestActivationCode = "ABC123";
        private const string TestToEmail = "valtechnas@gmail.com";

        public static IEnumerable<CommunicationToken> CreateActivationEmailTokens()
        {
            return new[]
            {
                new CommunicationToken(CommunicationTokens.CandidateFirstName, "FirstName"),
                new CommunicationToken(CommunicationTokens.ActivationCode, TestActivationCode),
                new CommunicationToken(CommunicationTokens.Username, TestToEmail),
                new CommunicationToken(CommunicationTokens.ActivationCodeExpiryDays, " 30 days")
            };
        }

        public static IEnumerable<CommunicationToken> CreateAccountUnlockCodeTokens()
        {
            return new[]
            {
                new CommunicationToken(CommunicationTokens.CandidateFirstName, "FirstName"),
                new CommunicationToken(CommunicationTokens.Username, TestToEmail),
                new CommunicationToken(CommunicationTokens.AccountUnlockCode, TestActivationCode),
                new CommunicationToken(CommunicationTokens.AccountUnlockCodeExpiryDays, " 1 day")
            };
        }

        public static IEnumerable<CommunicationToken> CreatePasswordResetConfirmationTokens()
        {
            return new[]
            {
                new CommunicationToken(CommunicationTokens.CandidateFirstName, "FirstName"),
                new CommunicationToken(CommunicationTokens.Username, TestToEmail)
            };
        }

        public static IEnumerable<CommunicationToken> CreatePasswordResetTokens()
        {
            return new[]
            {
                new CommunicationToken(CommunicationTokens.CandidateFirstName, "FirstName"),
                new CommunicationToken(CommunicationTokens.Username, TestToEmail),
                new CommunicationToken(CommunicationTokens.PasswordResetCode, TestActivationCode),
                new CommunicationToken(CommunicationTokens.PasswordResetCodeExpiryDays, "1 day")
            };
        }

        public static IEnumerable<CommunicationToken> CreateApprenticeshipApplicationSubmittedTokens()
        {
            return new[]
            {
                new CommunicationToken(CommunicationTokens.CandidateFirstName, "FirstName"),
                new CommunicationToken(CommunicationTokens.ApplicationVacancyTitle,
                    "Application Vacancy Title"),
                new CommunicationToken(CommunicationTokens.ApplicationVacancyReference,
                    "Application Vacancy Reference"),
                new CommunicationToken(CommunicationTokens.ApplicationVacancyEmployerName,
                    "Application Vacancy Employer Name")
            };
        }

        public static IEnumerable<CommunicationToken> CreateTraineeshipApplicationSubmittedTokens()
        {
            return new[]
            {
                new CommunicationToken(CommunicationTokens.CandidateFirstName, "FirstName"),
                new CommunicationToken(CommunicationTokens.ApplicationVacancyTitle,
                    "Application Vacancy Title"),
                new CommunicationToken(CommunicationTokens.ApplicationVacancyReference,
                    "Application Vacancy Reference"),
                new CommunicationToken(CommunicationTokens.ApplicationVacancyEmployerName,
                    "Application Vacancy Employer Name"),
                new CommunicationToken(CommunicationTokens.ProviderContact,
                    "Provider Contact")
            };
        }

        public static IEnumerable<CommunicationToken> CreateDailyDigestTokens(int numOfVacancies)
        {
            var tokens = new List<CommunicationToken>
            {
                new CommunicationToken(CommunicationTokens.CandidateFirstName, "FirstName"),
            };

            tokens.Add(new CommunicationToken(CommunicationTokens.ApplicationStatusAlerts, string.Empty));

            var drafts = new Fixture().Build<ExpiringApprenticeshipApplicationDraft>()
                .CreateMany(numOfVacancies)
                .ToList();

            var draftsJson = JsonConvert.SerializeObject(drafts);

            tokens.Add(new CommunicationToken(CommunicationTokens.ExpiringDrafts, draftsJson));

            return tokens;
        }

        public static IEnumerable<CommunicationToken> CreateContactMessageTokensWithDetails(string details)
        {
            return new[]
            {
                    new CommunicationToken(CommunicationTokens.UserEmailAddress, TestToEmail),
                    new CommunicationToken(CommunicationTokens.UserFullName, "User full name"),
                    new CommunicationToken(CommunicationTokens.UserEnquiry, "User enquiry"),
                    new CommunicationToken(CommunicationTokens.UserEnquiryDetails, details)
            };
        }

        public static IEnumerable<CommunicationToken> CreateSavedSearchAlertTokens(int noOfAlerts)
        {
            var tokens = new List<CommunicationToken>
            {
                new CommunicationToken(CommunicationTokens.CandidateFirstName, "FirstName")
            };

            var savedSearchAlerts = new Fixture().Build<SavedSearchAlert>()
                .CreateMany(noOfAlerts)
                .ToList();

            var savedSearchAlertsJson = JsonConvert.SerializeObject(savedSearchAlerts);

            tokens.Add(new CommunicationToken(CommunicationTokens.SavedSearchAlerts, savedSearchAlertsJson));

            return tokens;
       }
    }
}