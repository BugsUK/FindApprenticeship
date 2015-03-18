namespace SFA.Apprenticeships.Web.ContactForms.Providers
{
    using System;
    using Application.Interfaces.Communications;
    using Domain.Entities;
    using Infrastructure.Logging;
    using Interfaces;
    using Mappers.Interfaces;
    using ViewModels;

    public class AccessRequestProvider : IAccessRequestProvider
    {
        private readonly ICommunciationService _communciationService;
        private readonly IViewModelToDomainMapper<AccessRequestViewModel, AccessRequest> _accessRequestModelToDomainMapper;        

        public AccessRequestProvider(ICommunciationService communciationService, 
            IViewModelToDomainMapper<AccessRequestViewModel, AccessRequest> accessRequestModelToDomainMapper)
        {
            _communciationService = communciationService;
            _accessRequestModelToDomainMapper = accessRequestModelToDomainMapper;            
        }

        public AccessRequestSubmitStatus SubmitAccessRequest(AccessRequestViewModel viewModel)
        {
            try
            {
                var result = _accessRequestModelToDomainMapper.ConvertToDomain(viewModel);
                //todo: fix email via tokens
                //_communciationService.SendMessageToHelpdesk(MessageTypes.SendWebAccessRequest, new[]
                //{
                //    //common tokens 
                //    //new CommunicationToken(CommunicationTokens.Address1, result.Address.AddressLine1),
                //    //new CommunicationToken(CommunicationTokens.Address2, result.Address.AddressLine2),
                //    //new CommunicationToken(CommunicationTokens.Address3, result.Address.AddressLine3),
                //    //new CommunicationToken(CommunicationTokens.City, result.Address.City),
                //    //new CommunicationToken(CommunicationTokens.Postcode, result.Address.Postcode),
                //    new CommunicationToken(CommunicationTokens.Title, result.Title),
                //    new CommunicationToken(CommunicationTokens.Firstname, result.Firstname),
                //    new CommunicationToken(CommunicationTokens.Lastname, result.Lastname),
                //    new CommunicationToken(CommunicationTokens.WorkPhoneNumber, result.WorkPhoneNumber),
                //    new CommunicationToken(CommunicationTokens.MobileNumber, result.MobileNumber),
                //    //new CommunicationToken(CommunicationTokens.Companyname, result.Companyname),
                //    //access request specific tokens 
                //    //new CommunicationToken(CommunicationTokens.UserType, result.UserType),
                //    //new CommunicationToken(CommunicationTokens.SystemName, result.Systemname),
                //    //new CommunicationToken(CommunicationTokens.ContactEmail, result.AdditionalEmail),
                //    //new CommunicationToken(CommunicationTokens.ContactName, result.Contactname),
                //    //new CommunicationToken(CommunicationTokens.ContactPhoneNumber, result.AdditionalPhoneNumber),
                //    //new CommunicationToken(CommunicationTokens.HasApprenticeshipVacancies, result.HasApprenticeshipVacancies.ToString()),
                //    //new CommunicationToken(CommunicationTokens.Services, String.Join(",", result.ServiceTypeIds))
                //});
                return AccessRequestSubmitStatus.Success;
            }
            catch (System.Exception exception)
            {
                //todo: log error using preferred logging mechanism
                return AccessRequestSubmitStatus.Error;
            }
        }
    }
}