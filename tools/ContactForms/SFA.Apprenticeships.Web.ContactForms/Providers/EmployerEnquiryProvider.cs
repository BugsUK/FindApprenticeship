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
            IViewModelToDomainMapper<EmployerEnquiryViewModel, EmployerEnquiry> employerEnquiryVtoDMapper)
        {            
            _communciationService = communciationService;
            _employerEnquiryViewModelToDomainMapper = employerEnquiryVtoDMapper;
        }
        
        public SubmitQueryStatus SubmitEnquiry(EmployerEnquiryViewModel employerEnquiryData)
        {
            try
            {
                var employerEnquiry = _employerEnquiryViewModelToDomainMapper.ConvertToDomain(employerEnquiryData);

                //todo: fix email via tokens
                //_communciationService.SendMessageToHelpdesk(MessageTypes.SendEmployerEnquiry,  new[]
                //{
                //    //common tokens 
                //    new CommunicationToken(CommunicationTokens.Address1, employerEnquiry.Address.AddressLine1),
                //    new CommunicationToken(CommunicationTokens.Address2, employerEnquiry.Address.AddressLine2),
                //    new CommunicationToken(CommunicationTokens.Address3, employerEnquiry.Address.AddressLine3),
                //    new CommunicationToken(CommunicationTokens.City, employerEnquiry.Address.City),
                //    new CommunicationToken(CommunicationTokens.Postcode, employerEnquiry.Address.Postcode),
                //    new CommunicationToken(CommunicationTokens.Title, employerEnquiry.Title),
                //    new CommunicationToken(CommunicationTokens.Firstname, employerEnquiry.Firstname),
                //    new CommunicationToken(CommunicationTokens.Lastname, employerEnquiry.Lastname),
                //    new CommunicationToken(CommunicationTokens.WorkPhoneNumber, employerEnquiry.WorkPhoneNumber),
                //    new CommunicationToken(CommunicationTokens.MobileNumber, employerEnquiry.MobileNumber),
                //    new CommunicationToken(CommunicationTokens.Companyname, employerEnquiry.Companyname),
                //    //Employer enquiry specific tokens 
                //    new CommunicationToken(CommunicationTokens.EmployeesCount, employerEnquiry.EmployeesCount),
                //    new CommunicationToken(CommunicationTokens.EnquiryDescription, employerEnquiry.EnquiryDescription),
                //    new CommunicationToken(CommunicationTokens.EnquirySource, employerEnquiry.EnquirySource),
                //    new CommunicationToken(CommunicationTokens.Position, employerEnquiry.Position),
                //    new CommunicationToken(CommunicationTokens.PreviousExperienceType, employerEnquiry.PreviousExperienceType),
                //    new CommunicationToken(CommunicationTokens.WorkSector, employerEnquiry.WorkSector),
                //});
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