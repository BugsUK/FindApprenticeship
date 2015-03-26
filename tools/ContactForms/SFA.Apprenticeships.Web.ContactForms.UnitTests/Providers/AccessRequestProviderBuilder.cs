namespace SFA.Apprenticeships.Web.ContactForms.Tests.Providers
{
    using Application.Interfaces.Communications;
    using Mappers.Interfaces;
    using ContactForms.Providers;
    using ViewModels;
    using Domain.Entities;
    using Moq;

    public class AccessRequestProviderBuilder
    {
        Mock<ICommunciationService> _communciationServiceMock = new Mock<ICommunciationService>();
        private Mock<IViewModelToDomainMapper<AccessRequestViewModel, AccessRequest>> _viewModeltoDomainMapper = new Mock<IViewModelToDomainMapper<AccessRequestViewModel, AccessRequest>>();

        public AccessRequestProviderBuilder With(Mock<IViewModelToDomainMapper<AccessRequestViewModel, AccessRequest>> mock)
        {
            _viewModeltoDomainMapper = mock;
            return this;
        }

        public AccessRequestProviderBuilder With(Mock<ICommunciationService> mock)
        {
            _communciationServiceMock = mock;
            return this;
        }

        public AccessRequestProvider Build()
        {
            return new AccessRequestProvider(_communciationServiceMock.Object, 
                _viewModeltoDomainMapper.Object);
        }
    }
}