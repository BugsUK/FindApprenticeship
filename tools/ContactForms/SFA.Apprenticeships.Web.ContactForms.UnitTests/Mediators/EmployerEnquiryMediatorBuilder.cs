namespace SFA.Apprenticeships.Web.ContactForms.Tests.Mediators
{
    using ContactForms.Mediators.EmployerEnquiry;
    using ContactForms.Mediators.Interfaces;
    using ContactForms.Providers.Interfaces;
    using Validators;
    using Moq;

    internal class EmployerEnquiryMediatorBuilder
    {
        private Mock<IEmployerEnquiryProvider> _employerEnquiryProviderMock = new Mock<IEmployerEnquiryProvider>();
        readonly Mock<IReferenceDataMediator> _referenceDataMediator = new Mock<IReferenceDataMediator>();
        private EmployerEnquiryViewModelServerValidator _enquiryViewModelServerValidator = new EmployerEnquiryViewModelServerValidator();

        public EmployerEnquiryMediatorBuilder With(Mock<IEmployerEnquiryProvider> mock)
        {
            _employerEnquiryProviderMock = mock;
            return this;
        }

        public EmployerEnquiryMediatorBuilder With(EmployerEnquiryViewModelServerValidator enquiryViewModelServerValidatorMock)
        {
            _enquiryViewModelServerValidator = enquiryViewModelServerValidatorMock;
            return this;
        }

        public EmployerEnquiryMediator Build()
        {
            return new EmployerEnquiryMediator(_employerEnquiryProviderMock.Object,
                _enquiryViewModelServerValidator, _referenceDataMediator.Object);
        }
    }
}