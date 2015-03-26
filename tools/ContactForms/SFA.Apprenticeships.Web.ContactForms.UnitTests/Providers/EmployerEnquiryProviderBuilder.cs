namespace SFA.Apprenticeships.Web.ContactForms.Tests.Providers
{
    using Application.Interfaces.Communications;
    using Mappers.Interfaces;
    using ContactForms.Providers;
    using ViewModels;
    using Domain.Entities;
    using Moq;

    public class EmployerEnquiryProviderBuilder
    {
        Mock<ICommunciationService> _communciationServiceMock = new Mock<ICommunciationService>();
        private Mock<IViewModelToDomainMapper<EmployerEnquiryViewModel, EmployerEnquiry>> _employerEnquiryVtoDMapper= new Mock<IViewModelToDomainMapper<EmployerEnquiryViewModel, EmployerEnquiry>>();
        
        public EmployerEnquiryProviderBuilder With(Mock<IViewModelToDomainMapper<EmployerEnquiryViewModel, EmployerEnquiry>> mock)
        {
            _employerEnquiryVtoDMapper = mock;
            return this;
        }

        public EmployerEnquiryProviderBuilder With(Mock<ICommunciationService>  mock)
        {
            _communciationServiceMock = mock;
            return this;
        }

        public EmployerEnquiryProvider Build()
        {
            return new EmployerEnquiryProvider(_communciationServiceMock.Object, 
                _employerEnquiryVtoDMapper.Object);
        }
    }
}