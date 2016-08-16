using SFA.Apprenticeships.Web.Common.UnitTests.Mediators;

namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.Account
{
    using System;
    using System.Linq;
    using Builders;
    using Domain.Entities.Candidates;
    using Candidate.Mediators.Account;
    using Candidate.Providers;
    using Candidate.ViewModels;
    using Candidate.ViewModels.Account;
    using Common.Constants;
    using Common.ViewModels.Locations;
    using Constants.Pages;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    [TestFixture]
    [Parallelizable]
    public class SaveSettingsTests
    {
        [Test]
        public void SaveValidationErrorTest()
        {
            var settingsViewModel = new SettingsViewModel();
            var accountProvider = new Mock<IAccountProvider>();
            accountProvider.Setup(x => x.GetSettingsViewModel(It.IsAny<Guid>())).Returns(new SettingsViewModel());
            var accountMediator = new AccountMediatorBuilder().With(accountProvider.Object).Build();

            var response = accountMediator.SaveSettings(Guid.NewGuid(), settingsViewModel);
            response.Code.Should().Be(AccountMediatorCodes.Settings.ValidationError);
            response.ViewModel.Should().Be(settingsViewModel);
            response.ValidationResult.Should().NotBeNull();
        }

        [Test]
        public void SaveSuccessTest()
        {
            var settingsViewModel = new SettingsViewModel
            {
                Address = new AddressViewModel
                {
                    AddressLine1 = "Add1",
                    AddressLine2 = "Add2",
                    AddressLine3 = "Add3",
                    AddressLine4 = "Add4",
                    Postcode = "N7 8LS"
                },
                DateOfBirth = new DateViewModel { Day = DateTime.UtcNow.Day, Month = DateTime.UtcNow.Month, Year = DateTime.UtcNow.Year },
                PhoneNumber = "079824524523",
                Firstname = "FN",
                Lastname = "LN",
                EnableApplicationStatusChangeAlertsViaEmail = true
            };

            // ReSharper disable once RedundantAssignment
            var candidate = new Candidate();
            candidate.DisableAllOptionalCommunications();
            var accountProviderMock = new Mock<IAccountProvider>();

            accountProviderMock.Setup(x => x.TrySaveSettings(It.IsAny<Guid>(), It.IsAny<SettingsViewModel>(), out candidate)).Returns(true);

            var accountMediator = new AccountMediatorBuilder().With(accountProviderMock.Object).Build();
            var response = accountMediator.SaveSettings(Guid.NewGuid(), settingsViewModel);

            response.Code.Should().Be(AccountMediatorCodes.Settings.Success);
            response.ViewModel.Should().Be(settingsViewModel);
        }

        [Test]
        public void SaveNotificationsSettingsSuccessWithWarning()
        {
            var settingsViewModel = new SettingsViewModelBuilder().Build();

            settingsViewModel.Mode = SettingsViewModel.SettingsMode.YourAccount;

            var candidateServiceProviderMock = new Mock<ICandidateServiceProvider>();

            // ReSharper disable once RedundantAssignment
            var candidate = new Candidate();
            candidate.DisableAllOptionalCommunications();
            var accountProviderMock = new Mock<IAccountProvider>();
            
            accountProviderMock.Setup(x => x.TrySaveSettings(It.IsAny<Guid>(), It.IsAny<SettingsViewModel>(), out candidate)).Returns(true);
            
            var accountMediator = new AccountMediatorBuilder().With(candidateServiceProviderMock).With(accountProviderMock.Object).Build();
            var response = accountMediator.SaveSettings(Guid.NewGuid(), settingsViewModel);

            response.AssertMessage(AccountMediatorCodes.Settings.SuccessWithWarning, AccountPageMessages.SettingsUpdatedNotificationsAlertWarning, UserMessageLevel.Info, true);
        }

        [Test]
        public void SaveSearchesSettingsSuccessWithWarning()
        {
            var savedSearchViewModels = new Fixture().Build<SavedSearchViewModel>().With(s => s.AlertsEnabled, false).CreateMany(1).ToList();
            var settingsViewModel = new SettingsViewModelBuilder().EnableSavedSearchAlertsViaEmail(true).EnableSavedSearchAlertsViaText(true).WithSavedSearchViewModels(savedSearchViewModels).Build();
            settingsViewModel.Mode = SettingsViewModel.SettingsMode.SavedSearches;

            // ReSharper disable once RedundantAssignment
            var candidate = new Candidate();
            candidate.DisableAllOptionalCommunications();
            var accountProviderMock = new Mock<IAccountProvider>();

            accountProviderMock.Setup(x => x.TrySaveSettings(It.IsAny<Guid>(), It.IsAny<SettingsViewModel>(), out candidate)).Returns(true);

            var accountMediator = new AccountMediatorBuilder().With(accountProviderMock.Object).Build();
            var response = accountMediator.SaveSettings(Guid.NewGuid(), settingsViewModel);

            response.AssertMessage(AccountMediatorCodes.Settings.SuccessWithWarning, AccountPageMessages.SettingsUpdatedSavedSearchesAlertWarning, UserMessageLevel.Info, true);
        }

        [Test]
        public void SaveErrorTest()
        {
            var settingsViewModel = new SettingsViewModel
            {
                Address = new AddressViewModel
                {
                    AddressLine1 = "Add1",
                    AddressLine2 = "Add2",
                    AddressLine3 = "Add3",
                    AddressLine4 = "Add4",
                    Postcode = "N7 8LS"
                },
                DateOfBirth = new DateViewModel { Day = DateTime.UtcNow.Day, Month = DateTime.UtcNow.Month, Year = DateTime.UtcNow.Year },
                PhoneNumber = "079824524523",
                Firstname = "FN",
                Lastname = "LN"
            };

            // ReSharper disable once RedundantAssignment
            var candidate = new Candidate();
            var accountProviderMock = new Mock<IAccountProvider>();

            accountProviderMock.Setup(x => x.TrySaveSettings(It.IsAny<Guid>(), It.IsAny<SettingsViewModel>(), out candidate)).Returns(false);

            var accountMediator = new AccountMediatorBuilder().With(accountProviderMock.Object).Build();
            var response = accountMediator.SaveSettings(Guid.NewGuid(), settingsViewModel);

            response.Code.Should().Be(AccountMediatorCodes.Settings.SaveError);
            response.ViewModel.Should().Be(settingsViewModel);
            response.Message.Text.Should().Be(AccountPageMessages.SettingsUpdateFailed);
            response.Message.Level.Should().Be(UserMessageLevel.Warning);
        }
    }
}