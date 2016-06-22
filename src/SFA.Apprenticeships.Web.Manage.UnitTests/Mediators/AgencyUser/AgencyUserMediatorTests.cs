namespace SFA.Apprenticeships.Web.Manage.UnitTests.Mediators.AgencyUser
{
    using System.Collections.Generic;
    using Common.Constants;
    using Common.Providers;
    using Common.UnitTests.Builders;
    using Common.UnitTests.Mediators;
    using Constants.Messages;
    using Domain.Entities.Raa;
    using FluentAssertions;
    using Manage.Mediators.AgencyUser;
    using Manage.Providers;
    using Moq;
    using NUnit.Framework;
    using Raa.Common.Configuration;
    using Raa.Common.Providers;
    using Raa.Common.ViewModels.Vacancy;

    using SFA.Apprenticeships.Application.Interfaces;
    using SFA.Infrastructure.Interfaces;
    using ViewModels;

    [TestFixture]
    public class AgencyUserMediatorTests
    {
        private const string ValidAuthorisationGroupClaim = "validGroupClaim";
        private const string InValidAuthorisationGroupClaim = "invalidGroupClaim";

        [Test]
        public void Authorize_EmptyUsername()
        {
            var principal = new ClaimsPrincipalBuilder().Build();

            var configurationService = new Mock<IConfigurationService>();
            configurationService.Setup(c => c.Get<ManageWebConfiguration>())
                .Returns(new ManageWebConfiguration { AuthorisationGroupClaim = ValidAuthorisationGroupClaim });

            var mediator = new AgencyUserMediatorBuilder().With(configurationService).Build();

            var response = mediator.Authorize(principal);
            response.AssertMessage(AgencyUserMediatorCodes.Authorize.EmptyUsername, AuthorizeMessages.EmptyUsername, UserMessageLevel.Error);
        }

        [Test]
        public void Authenticated_MissingServicePermission()
        {
            var principal = new ClaimsPrincipalBuilder().WithName("User001").WithGroup(InValidAuthorisationGroupClaim).Build();

            var configurationService = new Mock<IConfigurationService>();
            configurationService.Setup(c => c.Get<ManageWebConfiguration>())
                .Returns(new ManageWebConfiguration { AuthorisationGroupClaim = ValidAuthorisationGroupClaim });

            var mediator = new AgencyUserMediatorBuilder().With(configurationService).Build();

            var response = mediator.Authorize(principal);
            response.AssertMessage(AgencyUserMediatorCodes.Authorize.MissingServicePermission, AuthorizeMessages.MissingServicePermission, UserMessageLevel.Error);
        }

        [Test]
        public void Authenticated_SessionReturnUrl()
        {
            const string returnUrl = "/localallowedurl";
            var userDataProvider = new Mock<IUserDataProvider>();
            userDataProvider.Setup(p => p.Pop(UserDataItemNames.ReturnUrl)).Returns(returnUrl);

            var configurationService = new Mock<IConfigurationService>();
            configurationService.Setup(c => c.Get<ManageWebConfiguration>())
                .Returns(new ManageWebConfiguration { AuthorisationGroupClaim = ValidAuthorisationGroupClaim });

            var mediator = new AgencyUserMediatorBuilder().With(userDataProvider).With(configurationService).Build();

            var principal = new ClaimsPrincipalBuilder().WithName("User001").WithGroup(ValidAuthorisationGroupClaim).Build();

            var response = mediator.Authorize(principal);
            response.AssertCode(AgencyUserMediatorCodes.Authorize.ReturnUrl, false, true);
            response.Parameters.Should().Be(returnUrl);
        }

        [Test]
        public void Authenticated_SessionReturnUrlNotAllowed()
        {
            const string returnUrl = "http://notallowedurl.com/";
            var userDataProvider = new Mock<IUserDataProvider>();
            userDataProvider.Setup(p => p.Pop(UserDataItemNames.ReturnUrl)).Returns(returnUrl);

            var configurationService = new Mock<IConfigurationService>();
            configurationService.Setup(c => c.Get<ManageWebConfiguration>())
                .Returns(new ManageWebConfiguration { AuthorisationGroupClaim = ValidAuthorisationGroupClaim });

            var mediator = new AgencyUserMediatorBuilder().With(userDataProvider).With(configurationService).Build();

            var principal = new ClaimsPrincipalBuilder().WithName("User001").WithGroup(ValidAuthorisationGroupClaim).Build();

            var response = mediator.Authorize(principal);
            response.AssertCode(AgencyUserMediatorCodes.Authorize.Ok);
        }

        [Test]
        public void Authenticated_Ok()
        {
            var principal = new ClaimsPrincipalBuilder().WithName("User001").WithGroup(ValidAuthorisationGroupClaim).Build();

            var configurationService = new Mock<IConfigurationService>();
            configurationService.Setup(c => c.Get<ManageWebConfiguration>())
                .Returns(new ManageWebConfiguration { AuthorisationGroupClaim = ValidAuthorisationGroupClaim });

            var mediator = new AgencyUserMediatorBuilder().With(configurationService).Build();

            var response = mediator.Authorize(principal);
            response.AssertCode(AgencyUserMediatorCodes.Authorize.Ok);
        }

        [Test] //TODO: remove roleListPart
        public void GetHomeViewModelShouldGetUserViewModel()
        {
            const string userName = "User001";
            const string roleList = "Agency";
            var principal = new ClaimsPrincipalBuilder().WithName(userName).WithRole(Roles.Raa).WithRoleList(roleList).Build();

            var userProvider = new Mock<IAgencyUserProvider>();
            userProvider.Setup(up => up.GetAgencyUser(userName)).Returns(new AgencyUserViewModel
            {
                RoleId = roleList
            });

            var configurationService = new Mock<IConfigurationService>();
            configurationService.Setup(c => c.Get<ManageWebConfiguration>())
                .Returns(new ManageWebConfiguration { AuthorisationGroupClaim = ValidAuthorisationGroupClaim });
            var mediator = new AgencyUserMediatorBuilder().With(userProvider).With(configurationService).Build();

            var response = mediator.GetHomeViewModel(principal, new DashboardVacancySummariesSearchViewModel());

            response.AssertCode(AgencyUserMediatorCodes.GetHomeViewModel.OK);
            response.ViewModel.AgencyUser.RoleId.Should().Be(roleList);
            userProvider.Verify(up => up.GetAgencyUser(userName), Times.Once);
        }

        [Test]
        public void GetHomeViewModelShouldGetVacancies()
        {
            var searchViewModel = new DashboardVacancySummariesSearchViewModel();
            var vacancies = new List<DashboardVacancySummaryViewModel>
            {
                new DashboardVacancySummaryViewModel
                {
                    Title = "Vacancy 1"
                }
            };
            var vacancyProvider = new Mock<IVacancyQAProvider>();
            vacancyProvider.Setup(vp => vp.GetPendingQAVacanciesOverview(searchViewModel)).Returns(new DashboardVacancySummariesViewModel {Vacancies = vacancies});
            var configurationService = new Mock<IConfigurationService>();
            configurationService.Setup(c => c.Get<ManageWebConfiguration>())
                .Returns(new ManageWebConfiguration {AuthorisationGroupClaim = ValidAuthorisationGroupClaim});
            var mediator = new AgencyUserMediatorBuilder().With(vacancyProvider).With(configurationService).Build();
            var principal = new ClaimsPrincipalBuilder().WithName("User001").WithGroup(ValidAuthorisationGroupClaim).Build();

            var response = mediator.GetHomeViewModel(principal, searchViewModel);

            response.AssertCode(AgencyUserMediatorCodes.GetHomeViewModel.OK);
            vacancyProvider.Verify(vp => vp.GetPendingQAVacanciesOverview(searchViewModel), Times.Once);
            response.ViewModel.VacancySummaries.Vacancies.ShouldBeEquivalentTo(vacancies);
        }
    }
}