
namespace SFA.Apprenticeships.Web.ContactForms.Tests.Mediators
{
    using ContactForms.Mediators.AccessRequest;
    using ContactForms.Mediators.Interfaces;
    using ContactForms.Providers.Interfaces;
    using Validators;
    using Moq;

    internal class AccessRequestMediatorBuilder
    {
        private Mock<IAccessRequestProvider> _providerMock = new Mock<IAccessRequestProvider>();
        private Mock<IReferenceDataMediator> _referenceDataMediator = new Mock<IReferenceDataMediator>();
        private AccessRequestViewModelServerValidator _viewModelServerValidator = new AccessRequestViewModelServerValidator();

        public AccessRequestMediatorBuilder With(Mock<IAccessRequestProvider> mock)
        {
            _providerMock = mock;
            return this;
        }

        public AccessRequestMediatorBuilder With(Mock<IReferenceDataMediator> mock)
        {
            _referenceDataMediator = mock;
            return this;
        }

        public AccessRequestMediatorBuilder With(AccessRequestViewModelServerValidator viewModelServerValidatorMock)
        {
            _viewModelServerValidator = viewModelServerValidatorMock;
            return this;
        }

        public AccessRequestMediator Build()
        {
            return new AccessRequestMediator(_providerMock.Object, _viewModelServerValidator, _referenceDataMediator.Object);
        }
    }
}