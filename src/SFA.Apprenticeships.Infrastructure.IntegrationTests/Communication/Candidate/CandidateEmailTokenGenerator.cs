namespace SFA.Apprenticeships.Infrastructure.IntegrationTests.Communication.Candidate
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Application.Interfaces.Communications;
    using Domain.Entities.Candidates;
    using Domain.Entities.Communication;
    using Domain.Entities.Vacancies;
    using Newtonsoft.Json;
    using Ploeh.AutoFixture;

    public static class CandidateEmailTokenGenerator
    {

        private const string TestActivationCode = "ABC123";
        private const string TestMobileVerificationCode = "1234";
        private const string TestPendingUsernameCode = "XYZ456";
        private const string TestUsername = "valtechnas@gmail.com";
        private const string TestPendingUsername = "valtechnas+awesome@gmail.com";

        public static IEnumerable<CommunicationToken> CreateActivationEmailTokens()
        {
            return new[]
            {
                new CommunicationToken(CommunicationTokens.CandidateSiteDomainName, "int.findapprenticeship.service.gov.uk"),
                new CommunicationToken(CommunicationTokens.CandidateFirstName, "Jane"),
                new CommunicationToken(CommunicationTokens.ActivationCode, TestActivationCode),
                new CommunicationToken(CommunicationTokens.Username, TestUsername),
                new CommunicationToken(CommunicationTokens.ActivationCodeExpiryDays, " 30 days")
            };
        }

        public static IEnumerable<CommunicationToken> CreateAccountUnlockCodeTokens()
        {
            return new[]
            {
                new CommunicationToken(CommunicationTokens.CandidateSiteDomainName, "int.findapprenticeship.service.gov.uk"),
                new CommunicationToken(CommunicationTokens.CandidateFirstName, "Jane"),
                new CommunicationToken(CommunicationTokens.Username, TestUsername),
                new CommunicationToken(CommunicationTokens.AccountUnlockCode, TestActivationCode),
                new CommunicationToken(CommunicationTokens.AccountUnlockCodeExpiryDays, " 1 day")
            };
        }

        public static IEnumerable<CommunicationToken> CreatePasswordResetConfirmationTokens()
        {
            return new[]
            {
                new CommunicationToken(CommunicationTokens.CandidateSiteDomainName, "int.findapprenticeship.service.gov.uk"),
                new CommunicationToken(CommunicationTokens.CandidateFirstName, "Jane"),
                new CommunicationToken(CommunicationTokens.Username, TestUsername)
            };
        }

        public static IEnumerable<CommunicationToken> CreatePasswordResetTokens()
        {
            return new[]
            {
                new CommunicationToken(CommunicationTokens.CandidateSiteDomainName, "int.findapprenticeship.service.gov.uk"),
                new CommunicationToken(CommunicationTokens.CandidateFirstName, "Jane"),
                new CommunicationToken(CommunicationTokens.Username, TestUsername),
                new CommunicationToken(CommunicationTokens.PasswordResetCode, TestActivationCode),
                new CommunicationToken(CommunicationTokens.PasswordResetCodeExpiryDays, "1 day")
            };
        }

        public static IEnumerable<CommunicationToken> CreateApprenticeshipApplicationSubmittedTokens()
        {
            return new[]
            {
                new CommunicationToken(CommunicationTokens.CandidateSiteDomainName, "int.findapprenticeship.service.gov.uk"),
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
                new CommunicationToken(CommunicationTokens.CandidateSiteDomainName, "int.findapprenticeship.service.gov.uk"),
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
                new CommunicationToken(CommunicationTokens.CandidateSiteDomainName, "int.findapprenticeship.service.gov.uk"),
                new CommunicationToken(CommunicationTokens.CandidateFirstName, "Jane"),
                new CommunicationToken(CommunicationTokens.ApplicationStatusAlerts, string.Empty),
                new CommunicationToken(CommunicationTokens.CandidateSubscriberId, Guid.NewGuid().ToString()),
                new CommunicationToken(CommunicationTokens.CandidateSubscriptionType, ((int)SubscriptionTypes.DailyDigestViaEmail).ToString(CultureInfo.InvariantCulture))
            };

            var drafts = new Fixture().Build<ExpiringApprenticeshipApplicationDraft>()
                .CreateMany(numOfVacancies)
                .ToList();

            var draftsJson = JsonConvert.SerializeObject(drafts);

            tokens.Add(new CommunicationToken(CommunicationTokens.ExpiringDrafts, draftsJson));

            return tokens;
        }

        public static IEnumerable<CommunicationToken> CreateContactUsMessageTokensWithDetails(string details)
        {
            return new[]
            {
                new CommunicationToken(CommunicationTokens.CandidateSiteDomainName, "int.findapprenticeship.service.gov.uk"),
                new CommunicationToken(CommunicationTokens.UserEmailAddress, TestUsername),
                new CommunicationToken(CommunicationTokens.UserFullName, "User full name"),
                new CommunicationToken(CommunicationTokens.UserEnquiry, "User enquiry"),
                new CommunicationToken(CommunicationTokens.UserEnquiryDetails, details)
            };
        }

        public static IEnumerable<CommunicationToken> CreateSavedSearchAlertTokens(int noOfAlerts, bool includeSubCategories = false)
        {
            var tokens = new List<CommunicationToken>
            {
                new CommunicationToken(CommunicationTokens.CandidateSiteDomainName, "int.findapprenticeship.service.gov.uk"),
                new CommunicationToken(CommunicationTokens.CandidateFirstName, "Jane"),
                new CommunicationToken(CommunicationTokens.CandidateSubscriberId, Guid.NewGuid().ToString()),
                new CommunicationToken(CommunicationTokens.CandidateSubscriptionType, ((int)SubscriptionTypes.SavedSearchAlertsViaEmail).ToString(CultureInfo.InvariantCulture))
            };

            var subCategories = new[]
            {
                "Emergency Fire Service Operations",
                "Employment Related Services",
                "Health - Allied Health Profession Support"
            };

            var subCategoriesFullName = string.Join(", ", subCategories);

            var savedSearchAlerts = new Fixture()
                .Build<SavedSearchAlert>()
                .With(each => each.Parameters, new Fixture()
                    .Build<SavedSearch>()
                    .With(each => each.SubCategories, includeSubCategories ? subCategories : null)
                    .With(each => each.SubCategoriesFullName, includeSubCategories ? subCategoriesFullName : null)
                    .Create())
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
                new CommunicationToken(CommunicationTokens.CandidateSiteDomainName, "int.findapprenticeship.service.gov.uk"),
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
                new CommunicationToken(CommunicationTokens.CandidateSiteDomainName, "int.findapprenticeship.service.gov.uk"),
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
                new CommunicationToken(CommunicationTokens.CandidateSiteDomainName, "int.findapprenticeship.service.gov.uk"),
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
                new CommunicationToken(CommunicationTokens.CandidateSiteDomainName, "int.findapprenticeship.service.gov.uk"),
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
                new CommunicationToken(CommunicationTokens.CandidateSiteDomainName, "int.findapprenticeship.service.gov.uk"),
                new CommunicationToken(CommunicationTokens.CandidateFirstName, "Jane"),
                new CommunicationToken(CommunicationTokens.MobileVerificationCode, TestMobileVerificationCode)
            };
        }

        public static IEnumerable<CommunicationToken> CreateSendPendingUsernameCodeTokens()
        {
            return new[]
            {
                new CommunicationToken(CommunicationTokens.CandidateSiteDomainName, "int.findapprenticeship.service.gov.uk"),
                new CommunicationToken(CommunicationTokens.CandidateFirstName, "Jane"),
                new CommunicationToken(CommunicationTokens.UserPendingUsername, TestPendingUsername),
                new CommunicationToken(CommunicationTokens.UserPendingUsernameCode, TestPendingUsernameCode)
            };
        }

        public static IEnumerable<CommunicationToken> CreateFeedbackTokens()
        {
            return new[]
            {
                new CommunicationToken(CommunicationTokens.CandidateSiteDomainName, "int.findapprenticeship.service.gov.uk"),
                new CommunicationToken(CommunicationTokens.UserEmailAddress, TestUsername),
                new CommunicationToken(CommunicationTokens.UserFullName, "User full name"),
                new CommunicationToken(CommunicationTokens.UserEnquiryDetails, "Some details")
            };
        }
    }
}
