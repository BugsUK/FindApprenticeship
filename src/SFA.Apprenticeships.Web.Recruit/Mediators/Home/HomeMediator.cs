namespace SFA.Apprenticeships.Web.Recruit.Mediators.Home
{
    using System;

    using FluentValidation.Results;

    using SFA.Apprenticeships.Web.Common.Constants;
    using SFA.Apprenticeships.Web.Common.Mediators;
    using SFA.Apprenticeships.Web.Raa.Common.Providers;
    using SFA.Apprenticeships.Web.Recruit.Constants.Messages;
    using SFA.Apprenticeships.Web.Recruit.Mediators.ProviderUser;
    using SFA.Apprenticeships.Web.Recruit.Validators;
    using SFA.Apprenticeships.Web.Recruit.ViewModels.Home;
    using SFA.Infrastructure.Interfaces;

    public class HomeMediator : MediatorBase, IHomeMediator
    {        
        private readonly IProviderUserProvider _providerUserProvider;               
        private readonly ILogService _logService;
        private readonly ContactMessageServerViewModelValidator _contactMessageServerViewModelValidator;                
        private readonly IProviderUserMediator _providerUserMediator;

        public HomeMediator(
            ILogService logService,
            IProviderUserProvider providerUserProvider,
            ContactMessageServerViewModelValidator contactMessageServerViewModelValidator,
            IProviderUserMediator providerUserMediator)
        {
            _logService = logService;
            _providerUserProvider = providerUserProvider;
            _contactMessageServerViewModelValidator = contactMessageServerViewModelValidator;
            _providerUserMediator = providerUserMediator;
        }

        public MediatorResponse<ContactMessageViewModel> SendContactMessage(ContactMessageViewModel contactMessageViewModel)
        {
            ValidationResult validationResult = _contactMessageServerViewModelValidator.Validate(contactMessageViewModel);

            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(HomeMediatorCodes.SendContactMessage.ValidationError, contactMessageViewModel, validationResult);
            }

           if (this._providerUserMediator.SendContactMessage(contactMessageViewModel))            
            {                
                return GetMediatorResponse(HomeMediatorCodes.SendContactMessage.SuccessfullySent,
                    contactMessageViewModel, ApplicationPageMessages.SendContactMessageSucceeded, UserMessageLevel.Success);
            }

            return GetMediatorResponse(HomeMediatorCodes.SendContactMessage.Error, contactMessageViewModel,
                ApplicationPageMessages.SendContactMessageFailed, UserMessageLevel.Warning);            
        }        

        public MediatorResponse<ContactMessageViewModel> GetContactMessageViewModel(string username)
        {
            var viewModel = InternalGetContactMessageViewModel(username);
            
            return GetMediatorResponse(HomeMediatorCodes.GetContactMessageViewModel.Successful, viewModel);            
        }                

        #region Helpers

        private ContactMessageViewModel InternalGetContactMessageViewModel(string username)
        {
            var viewModel = new ContactMessageViewModel();
            if (!string.IsNullOrWhiteSpace(username))
            {
                try
                {                    
                    var provider = _providerUserProvider.GetProviderUser(username);
                    viewModel.Email = provider.Email;
                    viewModel.Name = provider.Username;
                }
                catch (Exception ex)
                {
                    _logService.Error("Failed to created view model (ignoring)", ex);
                }
            }
            return viewModel;            
        }               

        #endregion
    }
}