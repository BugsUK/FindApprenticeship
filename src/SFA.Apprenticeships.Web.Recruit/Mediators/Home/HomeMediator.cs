namespace SFA.Apprenticeships.Web.Recruit.Mediators.Home
{
    using System;

    using SFA.Apprenticeships.Application.Interfaces.Users;
    using SFA.Apprenticeships.Web.Common.Mediators;
    using SFA.Apprenticeships.Web.Recruit.ViewModels.Home;
    using SFA.Infrastructure.Interfaces;

    public class HomeMediator : MediatorBase, IHomeMediator
    {
        private readonly IUserProfileService _userProfileService;               
        private readonly ILogService _logService;

        public HomeMediator(
            ILogService logService,
            IUserProfileService userProfileService)
        {
            _logService = logService;
            _userProfileService = userProfileService;                      
        }

        public MediatorResponse<ContactMessageViewModel> SendContactMessage(string username, ContactMessageViewModel contactMessageViewModel)
        {
            //var validationResult = _contactMessageServerViewModelValidator.Validate(contactMessageViewModel);

            //if (!validationResult.IsValid)
            //{                
            //    return GetMediatorResponse(HomeMediatorCodes.SendContactMessage.ValidationError, contactMessageViewModel, validationResult);
            //}

            //if (this._userProfileService.SendContactMessage(candidateId, contactMessageViewModel))
            //{
            //    var viewModel = this.InternalGetContactMessageViewModel(candidateId);                
            //    return GetMediatorResponse(HomeMediatorCodes.SendContactMessage.SuccessfullySent,
            //        viewModel, ApplicationPageMessages.SendContactMessageSucceeded, UserMessageLevel.Success);
            //}

            //PopulateContactMessageViewModelEnquiries(contactMessageViewModel);

            //return GetMediatorResponse(HomeMediatorCodes.SendContactMessage.Error, contactMessageViewModel,
            //    ApplicationPageMessages.SendContactMessageFailed, UserMessageLevel.Warning);
            return null;
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
                    var provider = _userProfileService.GetProviderUser(username);
                    viewModel.Email = provider.Email;
                    viewModel.Name = provider.Fullname;
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