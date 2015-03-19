namespace SFA.Apprenticeships.Infrastructure.Communication.IntegrationTests
{
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Communications;
    using Domain.Entities.Communication;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Newtonsoft.Json;
    using Ploeh.AutoFixture;

    public static class TokenGenerator
    {

        private const string TestActivationCode = "ABC123";
        private const string TestMobileVerificationCode = "1234";
        private const string TestToEmail = "valtechnas@gmail.com";

        public static IEnumerable<CommunicationToken> CreateActivationEmailTokens()
        {
            return new[]
            {
                new CommunicationToken(CommunicationTokens.CandidateFirstName, "Jane"),
                new CommunicationToken(CommunicationTokens.ActivationCode, TestActivationCode),
                new CommunicationToken(CommunicationTokens.Username, TestToEmail),
                new CommunicationToken(CommunicationTokens.ActivationCodeExpiryDays, " 30 days")
            };
        }

        public static IEnumerable<CommunicationToken> CreateAccountUnlockCodeTokens()
        {
            return new[]
            {
                new CommunicationToken(CommunicationTokens.CandidateFirstName, "Jane"),
                new CommunicationToken(CommunicationTokens.Username, TestToEmail),
                new CommunicationToken(CommunicationTokens.AccountUnlockCode, TestActivationCode),
                new CommunicationToken(CommunicationTokens.AccountUnlockCodeExpiryDays, " 1 day")
            };
        }

        public static IEnumerable<CommunicationToken> CreatePasswordResetConfirmationTokens()
        {
            return new[]
            {
                new CommunicationToken(CommunicationTokens.CandidateFirstName, "Jane"),
                new CommunicationToken(CommunicationTokens.Username, TestToEmail)
            };
        }

        public static IEnumerable<CommunicationToken> CreatePasswordResetTokens()
        {
            return new[]
            {
                new CommunicationToken(CommunicationTokens.CandidateFirstName, "Jane"),
                new CommunicationToken(CommunicationTokens.Username, TestToEmail),
                new CommunicationToken(CommunicationTokens.PasswordResetCode, TestActivationCode),
                new CommunicationToken(CommunicationTokens.PasswordResetCodeExpiryDays, "1 day")
            };
        }

        public static IEnumerable<CommunicationToken> CreateApprenticeshipApplicationSubmittedTokens()
        {
            return new[]
            {
                new CommunicationToken(CommunicationTokens.CandidateFirstName, "Jane"),
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
                new CommunicationToken(CommunicationTokens.CandidateFirstName, "Jane"),
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
                new CommunicationToken(CommunicationTokens.CandidateFirstName, "Jane"),
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
                new CommunicationToken(CommunicationTokens.CandidateFirstName, "Jane")
            };

            var savedSearchAlerts = new Fixture()
                .Build<SavedSearchAlert>()
                .CreateMany(noOfAlerts)
                .ToList();

            savedSearchAlerts.ForEach(x => x.Parameters.ApprenticeshipLevel = ApprenticeshipLevel.Advanced.ToString());

            var savedSearchAlertsJson = JsonConvert.SerializeObject(savedSearchAlerts);

            tokens.Add(new CommunicationToken(CommunicationTokens.SavedSearchAlerts, savedSearchAlertsJson));

            return tokens;
       }

        public static IEnumerable<CommunicationToken> CreateApprenticeshipApplicationStatusAlertTokens()
        {
            var tokens = new List<CommunicationToken>
            {
                new CommunicationToken(CommunicationTokens.CandidateFirstName, "Jane"),
            };

            var applicationStatusAlert = new Fixture().Build<ApplicationStatusAlert>().Create();
            var json = JsonConvert.SerializeObject(applicationStatusAlert);

            tokens.Add(new CommunicationToken(CommunicationTokens.ApplicationStatusAlert, json));

            return tokens;
        }

        public static IEnumerable<CommunicationToken> CreateApprenticeshipApplicationStatusAlertsTokens(int count)
        {
            var tokens = new List<CommunicationToken>
            {
                new CommunicationToken(CommunicationTokens.CandidateFirstName, "Jane"),
            };

            var applicationStatusAlerts = new Fixture()
                .Build<ApplicationStatusAlert>()
                .CreateMany(count);

            var json = JsonConvert.SerializeObject(applicationStatusAlerts);

            tokens.Add(new CommunicationToken(CommunicationTokens.ApplicationStatusAlerts, json));

            return tokens;
        }

        public static IEnumerable<CommunicationToken> CreateApprenticeshipApplicationExpiringDraftTokens()
        {
            var tokens = new List<CommunicationToken>
            {
                new CommunicationToken(CommunicationTokens.CandidateFirstName, "Jane"),
            };

            var expiringDraft = new Fixture().Build<ExpiringApprenticeshipApplicationDraft>().Create();
            var json = JsonConvert.SerializeObject(expiringDraft);

            tokens.Add(new CommunicationToken(CommunicationTokens.ExpiringDraft, json));

            return tokens;
        }

        public static IEnumerable<CommunicationToken> CreateApprenticeshipApplicationExpiringDraftsTokens(int count)
        {
            var tokens = new List<CommunicationToken>
            {
                new CommunicationToken(CommunicationTokens.CandidateFirstName, "Jane"),
            };

            var expiringDrafts = new Fixture()
                .Build<ExpiringApprenticeshipApplicationDraft>()
                .CreateMany(count);

            var json = JsonConvert.SerializeObject(expiringDrafts);

            tokens.Add(new CommunicationToken(CommunicationTokens.ExpiringDrafts, json));

            return tokens;
        }

        public static IEnumerable<CommunicationToken> CreateMobileVerificationCodeTokens()
        {
            return new[]
            {
                new CommunicationToken(CommunicationTokens.CandidateFirstName, "Jane"),
                new CommunicationToken(CommunicationTokens.MobileVerificationCode, TestMobileVerificationCode)
            };
        }
    }
}