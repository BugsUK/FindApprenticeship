namespace SFA.Apprenticeships.Web.ContactForms.Providers
{
    using Application.Interfaces.Communications;
    using Domain.Entities;
    using Interfaces;
    using Mappers.Interfaces;
    using ViewModels;

    public class EmployerEnquiryProvider : IEmployerEnquiryProvider
    {
        private readonly ICommunciationService _communciationService;
        private readonly IViewModelToDomainMapper<EmployerEnquiryViewModel, EmployerEnquiry> _employerEnquiryViewModelToDomainMapper;

        public EmployerEnquiryProvider(ICommunciationService communciationService, 
            IViewModelToDomainMapper<EmployerEnquiryViewModel, 
            EmployerEnquiry> employerEnquiryVtoDMapper)
        {            
            _communciationService = communciationService;
            _employerEnquiryViewModelToDomainMapper = employerEnquiryVtoDMapper;
        }

        
        public SubmitQueryStatus SubmitEnquiry(EmployerEnquiryViewModel employerEnquiryData)
        {
            try
            {
                var employerEnquiry = _employerEnquiryViewModelToDomainMapper.ConvertToDomain(employerEnquiryData);
                _communciationService.SendMessageToHelpdesk(employerEnquiry);
                return SubmitQueryStatus.Success;
            }
            catch (System.Exception exception)
            {
                //todo: log error using preferred logging mechanism
                return SubmitQueryStatus.Error;
            }
        }
    }
}