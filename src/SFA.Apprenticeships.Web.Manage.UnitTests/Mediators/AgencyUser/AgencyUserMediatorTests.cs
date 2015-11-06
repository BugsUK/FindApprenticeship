﻿namespace SFA.Apprenticeships.Web.Manage.UnitTests.Mediators.AgencyUser
{
    using System.Collections.Generic;
    using Common.Constants;
    using Common.Providers;
    using Common.UnitTests.Builders;
    using Common.UnitTests.Mediators;
    using Constants.Messages;
    using FluentAssertions;
    using Manage.Mediators.AgencyUser;
    using Manage.Providers;
    using Moq;
    using NUnit.Framework;
    using ViewModels;

    [TestFixture]
    public class AgencyUserMediatorTests
    {
        [Test]
        public void Authorize_EmptyUsername()
        {
            var mediator = new AgencyUserMediatorBuilder().Build();

            var principal = new ClaimsPrincipalBuilder().Build();

            var response = mediator.Authorize(principal);
            response.AssertMessage(AgencyUserMediatorCodes.Authorize.EmptyUsername, AuthorizeMessages.EmptyUsername, UserMessageLevel.Error);
        }

        [Test]
        public void Authenticated_MissingServicePermission()
        {
            var mediator = new AgencyUserMediatorBuilder().Build();

            var principal = new ClaimsPrincipalBuilder().WithName("User001").Build();

            var response = mediator.Authorize(principal);
            response.AssertMessage(AgencyUserMediatorCodes.Authorize.MissingServicePermission, AuthorizeMessages.MissingServicePermission, UserMessageLevel.Error);
        }

        [Test]
        public void Authenticated_MissingRoleListClaim()
        {
            var mediator = new AgencyUserMediatorBuilder().Build();

            var principal = new ClaimsPrincipalBuilder().WithName("User001").WithRole(Constants.Roles.Raa).Build();

            var response = mediator.Authorize(principal);
            response.AssertMessage(AgencyUserMediatorCodes.Authorize.MissingRoleListClaim, AuthorizeMessages.MissingRoleListClaim, UserMessageLevel.Error);
        }

        [Test]
        public void Authenticated_SessionReturnUrl()
        {
            const string returnUrl = "/localallowedurl";
            var userDataProvider = new Mock<IUserDataProvider>();
            userDataProvider.Setup(p => p.Pop(UserDataItemNames.ReturnUrl)).Returns(returnUrl);

            var mediator = new AgencyUserMediatorBuilder().With(userDataProvider).Build();

            var principal = new ClaimsPrincipalBuilder().WithName("User001").WithRole(Constants.Roles.Raa).WithRoleList("Agency").Build();

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

            var mediator = new AgencyUserMediatorBuilder().With(userDataProvider).Build();

            var principal = new ClaimsPrincipalBuilder().WithName("User001").WithRole(Constants.Roles.Raa).WithRoleList("Agency").Build();

            var response = mediator.Authorize(principal);
            response.AssertCode(AgencyUserMediatorCodes.Authorize.Ok);
        }

        [Test]
        public void Authenticated_Ok()
        {
            var mediator = new AgencyUserMediatorBuilder().Build();

            var principal = new ClaimsPrincipalBuilder().WithName("User001").WithRole(Constants.Roles.Raa).WithRoleList("Agency").Build();

            var response = mediator.Authorize(principal);
            response.AssertCode(AgencyUserMediatorCodes.Authorize.Ok);
        }

        [Test]
        public void GetHomeViewModelShouldGetUserViewModel()
        {
            const string userName = "User001";
            const string roleList = "Agency";
            var principal = new ClaimsPrincipalBuilder().WithName(userName).WithRole(Constants.Roles.Raa).WithRoleList(roleList).Build();

            var userProvider = new Mock<IAgencyUserProvider>();
            userProvider.Setup(up => up.GetAgencyUser(userName, roleList)).Returns(new AgencyUserViewModel
            {
                RoleId = roleList
            });

            var mediator = new AgencyUserMediatorBuilder().With(userProvider).Build();

            var response = mediator.GetHomeViewModel(principal);

            response.AssertCode(AgencyUserMediatorCodes.GetHomeViewModel.OK);
            response.ViewModel.AgencyUser.RoleId.Should().Be(roleList);
            userProvider.Verify(up => up.GetAgencyUser(userName, roleList), Times.Once);
        }

        [Test]
        public void GetHomeViewModelShouldGetVacancies()
        {
            var vacancies = new List<DashboardVacancySummaryViewModel>
            {
                new DashboardVacancySummaryViewModel
                {
                    Title = "Vacancy 1"
                }
            };
            var vacancyProvider = new Mock<IVacancyProvider>();
            vacancyProvider.Setup(vp => vp.GetPendingQAVacancies()).Returns(vacancies);
            var mediator = new AgencyUserMediatorBuilder().With(vacancyProvider).Build();
            var principal = new ClaimsPrincipalBuilder().WithName("User001").WithRole(Constants.Roles.Raa).WithRoleList("Agency").Build();

            var response = mediator.GetHomeViewModel(principal);

            response.AssertCode(AgencyUserMediatorCodes.GetHomeViewModel.OK);
            vacancyProvider.Verify(vp => vp.GetPendingQAVacancies(), Times.Once);
            response.ViewModel.Vacancies.ShouldBeEquivalentTo(vacancies);
        }
    }
}