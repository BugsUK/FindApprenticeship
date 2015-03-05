namespace SFA.Apprenticeships.Web.Employer.Tests.Mediators
{
    using Employer.Mediators.EmployerEnquiry;
    using Employer.Providers.Interfaces;
    using Moq;
    using Validators;

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