namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.Register
{
    using Candidate.Mediators.Register;
    using Candidate.Providers;
    using Candidate.Validators;
    using Common.Validators;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public abstract class RegisterBaseTests
    {
        protected RegisterMediator _registerMediator;
        protected Mock<ICandidateServiceProvider> _candidateServiceProvider;
        protected ActivationViewModelServerValidator _activationViewModelServerValidator;
        protected RegisterViewModelServerValidator _registerViewModelServerValidator;
        protected MonitoringInformationViewModelValidator _MonitoringInformationViewModelValidator;

        [SetUp]
        public void SetUp()
        {
            _candidateServiceProvider = new Mock<ICandidateServiceProvider>();
            _activationViewModelServerValidator = new ActivationViewModelServerValidator();
            _registerViewModelServerValidator = new RegisterViewModelServerValidator();
            _MonitoringInformationViewModelValidator = new MonitoringInformationViewModelValidator();

            _registerMediator = new RegisterMediator(_candidateServiceProvider.Object, _registerViewModelServerValidator, _activationViewModelServerValidator, _MonitoringInformationViewModelValidator);
        }
    }
}
