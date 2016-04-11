namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Mediators.Home
{
    using System;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Recruit.Mediators.Home;
    using Application.Interfaces.Users;
    using Domain.Entities.Raa.Users;
    using Common.Mediators;
    using ViewModels.Home;
    using SFA.Infrastructure.Interfaces;

    [TestFixture]
    public class HomeMediatorTests
    {        
        private Mock<IUserProfileService> _userProfileService;
        private Mock<ILogService> _logServiceMock;            
        private HomeMediator _homeMediator;

        [SetUp]
        public void SetUp()
        {
            _userProfileService=new Mock<IUserProfileService>();
            _logServiceMock=new Mock<ILogService>();            
            _homeMediator=new HomeMediator(_logServiceMock.Object,
                _userProfileService.Object);
        }               

        [Test]
        public void GetContactMessageViewModelTest_WithUserName()
        {
            //// Arrange.
            const string username = "a.user";            

            var providerUser = new ProviderUser
            {                             
                Fullname = "fullname",
                Email = "someone@sfa.com"
            };
                       
            _userProfileService.Setup(pup => pup.GetProviderUser(username))
                .Returns(providerUser);            

            MediatorResponse<ContactMessageViewModel> response = _homeMediator.GetContactMessageViewModel(username);
            response.ViewModel.Name.Should().NotBeNullOrWhiteSpace();
            response.ViewModel.Email.Should().NotBeNullOrWhiteSpace();
        }

        [Test]
        public void SendContactMessageTest_WithUserName()
        {
            throw new NotImplementedException();
        }
    }
}
