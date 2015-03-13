namespace SFA.Apprenticeships.Web.ContactForms.Providers
{
    using Application.Interfaces.Communications;
    using Domain.Entities;
    using Interfaces;
    using Mappers.Interfaces;
    using ViewModels;

    public class AccessRequestProvider : IAccessRequestProvider
    {
        private readonly ICommunciationService _communciationService;
        private readonly IViewModelToDomainMapper<AccessRequestViewModel, AccessRequest> _accessRequestModelToDomainMapper;

        public AccessRequestProvider(ICommunciationService communciationService, IViewModelToDomainMapper<AccessRequestViewModel, AccessRequest> accessRequestModelToDomainMapper)
        {
            _communciationService = communciationService;
            _accessRequestModelToDomainMapper = accessRequestModelToDomainMapper;
        }

        public AccessRequestSubmitStatus SubmitAccessRequest(AccessRequestViewModel viewModel)
        {
            try
            {
                //var result = _accessRequestModelToDomainMapper.ConvertToDomain(viewModel);
                //_communciationService.SendMessageToHelpdesk(result); //todo: implement token based message sending 
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