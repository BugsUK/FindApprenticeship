﻿namespace SFA.Apprenticeships.Infrastructure.Communication.IntegrationTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Communications;

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
                    "Application Vacancy Reference")
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
                new CommunicationToken(CommunicationTokens.ProviderContact,
                    "Provider Contact")
            };
        }

        public static IEnumerable<CommunicationToken> CreateVacanciesAboutToExpireTokens(int numOfVacancies)
        {
            var tokens = new List<CommunicationToken>
            {
                new CommunicationToken(CommunicationTokens.CandidateFirstName, "FirstName"),
                new CommunicationToken(CommunicationTokens.ExpiringDraftsCount, Convert.ToString(numOfVacancies))
            };

            var drafts = string.Join("~", Enumerable.Range(1, numOfVacancies).Select(i => string.Format("{0}|Application Vacancy Title {0}|Employer name {0}|15 Jan 15", i)));

            tokens.Add(new CommunicationToken(CommunicationTokens.ExpiringDrafts, drafts));

            return tokens;
        }

        public static IEnumerable<CommunicationToken> CreateContactMessageTokens()
        {
            return new[]
            {
                    new CommunicationToken(CommunicationTokens.UserEmailAddress, "valtechnas@gmail.com"),
                    new CommunicationToken(CommunicationTokens.UserFullName, "User full name"),
                    new CommunicationToken(CommunicationTokens.UserEnquiry, "User enquiry"),
                    new CommunicationToken(CommunicationTokens.UserEnquiryDetails, "User enquiry details")
            };
        }

        public static IEnumerable<CommunicationToken> CreateContactMessageTokensContainingUnicodeCharacters()
        {
            return new[]
            {
                    new CommunicationToken(CommunicationTokens.UserEmailAddress, TestToEmail),
                    new CommunicationToken(CommunicationTokens.UserFullName, "It’s my full name"),
                    new CommunicationToken(CommunicationTokens.UserEnquiry, "I’ve forgotten my password unicode."),
                    new CommunicationToken(CommunicationTokens.UserEnquiryDetails, "I’ve has unicode.")
            };
        }
    }
}