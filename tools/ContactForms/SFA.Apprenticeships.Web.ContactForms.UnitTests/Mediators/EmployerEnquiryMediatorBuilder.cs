namespace SFA.Apprenticeships.Web.ContactForms.Tests.Mediators
{
    using ContactForms.Mediators.EmployerEnquiry;
    using ContactForms.Providers.Interfaces;
    using ContactForms.Validators;
    using Moq;

    internal class EmployerEnquiryMediatorBuilder
    {
        private Mock<IEmployerEnquiryProvider> _employerEnquiryProviderMock = new Mock<IEmployerEnquiryProvider>();
        private EmployerEnquiryViewModelServerValidators _enquiryViewModelServerValidators = new EmployerEnquiryViewModelServerValidators();
        public EmployerEnquiryMediatorBuilder With(Mock<IEmployerEnquiryProvider> mock)
        {
            _employerEnquiryProviderMock = mock;
            return this;
        }

        public EmployerEnquiryMediatorBuilder With(EmployerEnquiryViewModelServerValidators enquiryViewModelServerValidatorsMock)
        {
            _enquiryViewModelServerValidators = enquiryViewModelServerValidatorsMock;
            return this;
        }

        public EmployerEnquiryMediator Build()
        {
            return new EmployerEnquiryMediator(_employerEnquiryProviderMock.Object,
                _enquiryViewModelServerValidators);
        }
    }
}