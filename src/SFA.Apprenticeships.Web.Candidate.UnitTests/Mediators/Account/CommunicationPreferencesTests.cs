﻿using SFA.Apprenticeships.Web.Common.UnitTests.Mediators;

namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.Account
{
    using System;
    using Application.Interfaces.Candidates;
    using Builders;
    using Candidate.Mediators.Account;
    using Common.Constants;
    using Constants.Pages;
    using Domain.Entities.UnitTests.Builder;
    using Moq;
    using NUnit.Framework;
    using Providers.AccountProvider;

    [TestFixture]
    public class CommunicationPreferencesTests
    {
        [TestCase("0123456789", false, false, AccountMediatorCodes.Settings.Success)]
        [TestCase("0123456789", true, false, AccountMediatorCodes.Settings.Success)]
        [TestCase("0123456789", false, true, AccountMediatorCodes.Settings.MobileVerificationRequired)]
        [TestCase("0123456789", true, true, AccountMediatorCodes.Settings.Success)]
        [TestCase("9876543210", false, false, AccountMediatorCodes.Settings.Success)]
        [TestCase("9876543210", true, false, AccountMediatorCodes.Settings.Success)]
        [TestCase("9876543210", false, true, AccountMediatorCodes.Settings.MobileVerificationRequired)]
        [TestCase("9876543210", true, true, AccountMediatorCodes.Settings.MobileVerificationRequired)]
        public void MobileVerificationRequiredCommunications(string newPhoneNumber, bool verifiedMobile, bool enableAnyTextCommunication, string expectedCode)
        {
            var candidateId = Guid.NewGuid();
            var candidate = new CandidateBuilder(candidateId).PhoneNumber("0123456789").VerifiedMobile(verifiedMobile).Build();
            var candidateService = new Mock<ICandidateService>();

            candidateService.Setup(cs => cs.GetCandidate(candidateId)).Returns(candidate);

            var viewModel = new SettingsViewModelBuilder().PhoneNumber(newPhoneNumber).EnableAnyTextCommunication(enableAnyTextCommunication).EnableApplicationStatusChangeAlertsViaEmail(true).Build();
            var accountProvider = new AccountProviderBuilder().With(candidateService).Build();
            var mediator = new AccountMediatorBuilder().With(accountProvider).Build();

            var result = mediator.SaveSettings(candidateId, viewModel);

            if (expectedCode == AccountMediatorCodes.Settings.MobileVerificationRequired)
            {
                result.AssertMessage(expectedCode, AccountPageMessages.MobileVerificationRequired, UserMessageLevel.Success, true);
            }
            else
            {
                result.AssertCodeAndMessage(expectedCode);
            }
        }

        [TestCase("0123456789", false, false, AccountMediatorCodes.Settings.Success)]
        [TestCase("0123456789", true, false, AccountMediatorCodes.Settings.Success)]
        [TestCase("0123456789", false, true, AccountMediatorCodes.Settings.MobileVerificationRequired)]
        [TestCase("0123456789", true, true, AccountMediatorCodes.Settings.Success)]
        [TestCase("9876543210", false, false, AccountMediatorCodes.Settings.Success)]
        [TestCase("9876543210", true, false, AccountMediatorCodes.Settings.Success)]
        [TestCase("9876543210", false, true, AccountMediatorCodes.Settings.MobileVerificationRequired)]
        [TestCase("9876543210", true, true, AccountMediatorCodes.Settings.MobileVerificationRequired)]
        public void MobileVerificationRequiredMarketing(string newPhoneNumber, bool verifiedMobile, bool enableAnyTextCommunication, string expectedCode)
        {
            var candidateId = Guid.NewGuid();
            var candidate = new CandidateBuilder(candidateId).PhoneNumber("0123456789").VerifiedMobile(verifiedMobile).Build();
            var candidateService = new Mock<ICandidateService>();
            candidateService.Setup(cs => cs.GetCandidate(candidateId)).Returns(candidate);
            var viewModel = new SettingsViewModelBuilder().PhoneNumber(newPhoneNumber).EnableAnyTextCommunication(enableAnyTextCommunication).EnableMarketingViaEmail(true).Build();
            var accountProvider = new AccountProviderBuilder().With(candidateService).Build();
            var mediator = new AccountMediatorBuilder().With(accountProvider).Build();

            var result = mediator.SaveSettings(candidateId, viewModel);

            if (expectedCode == AccountMediatorCodes.Settings.MobileVerificationRequired)
            {
                result.AssertMessage(expectedCode, AccountPageMessages.MobileVerificationRequired, UserMessageLevel.Success, true);
            }
            else
            {
                result.AssertCodeAndMessage(expectedCode);
            }
        }
    }
}