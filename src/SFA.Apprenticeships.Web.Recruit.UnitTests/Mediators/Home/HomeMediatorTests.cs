namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Mediators.Home
{
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Recruit.Mediators.Home;
    using Domain.Entities.Raa.Users;
    using Common.Mediators;
    using FluentValidation.Results;
    using Common.Constants;
    using Raa.Common.Providers;
    using Recruit.Mediators.ProviderUser;
    using Recruit.Validators;

    using SFA.Apprenticeships.Application.Interfaces;

    using ViewModels.Home;
    using SFA.Infrastructure.Interfaces;

    [TestFixture]
    [Parallelizable]
    public class HomeMediatorTests
    {        
        private Mock<IProviderUserProvider> _userProfileService;
        private Mock<ContactMessageServerViewModelValidator> _contactMessageServerViewModelValidator;
        private Mock<ILogService> _logServiceMock;            
        private HomeMediator _homeMediator;
        private Mock<IProviderUserMediator> _providerUserMediator;

        [SetUp]
        public void SetUp()
        {
            _userProfileService=new Mock<IProviderUserProvider>();
            _logServiceMock=new Mock<ILogService>();
            _providerUserMediator=new Mock<IProviderUserMediator>();
            _contactMessageServerViewModelValidator=new Mock<ContactMessageServerViewModelValidator>();
            _homeMediator =new HomeMediator(_logServiceMock.Object,
                _userProfileService.Object, _contactMessageServerViewModelValidator.Object, _providerUserMediator.Object);
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
                    
            //Act   
            _userProfileService.Setup(pup => pup.GetProviderUser(username))
                .Returns(providerUser);            


            MediatorResponse<ContactMessageViewModel> response = _homeMediator.GetContactMessageViewModel(username);

            //Assert
            response.ViewModel.Name.Should().NotBeNullOrWhiteSpace();
            response.ViewModel.Email.Should().NotBeNullOrWhiteSpace();
        }

        [Test]
        public void SendContactMessageTest_WithUserName()
        {
            //Arrange            
            const string username = "a.user";
            var contactMessageViewModel=new ContactMessageViewModel
                                            {
                                                Details = "Can you please guide me to the right job?",
                                                Email = "jane.doe@example.com",
                                                Enquiry = "How to apply for a apprenticeship",
                                                Name = "Jane Doe"
            };            

            //Act       
            _contactMessageServerViewModelValidator.Setup(validator => validator.Validate(It.IsAny<ContactMessageViewModel>()))
                .Returns(new ValidationResult());

            _providerUserMediator.Setup(mediator => mediator.SendContactMessage(contactMessageViewModel)).Returns(true);

            MediatorResponse<ContactMessageViewModel> response = _homeMediator.SendContactMessage(username,
                contactMessageViewModel);

            //Assert
            response.Code.Should().NotBeNullOrWhiteSpace();
            response.Message.Should().NotBeNull();   
            response.Message.Level.ShouldBeEquivalentTo(UserMessageLevel.Success);
            response.Message.Text.Should().Be("Your question has been successfully sent. Thank you.");
        }
    }
}
