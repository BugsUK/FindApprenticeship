using SFA.Apprenticeships.Web.Common.Mediators;

namespace SFA.Apprenticeships.Web.Candidate.Mediators.Home
{
    using System;
    using System.Web.Mvc;
    using SFA.Infrastructure.Interfaces;
    using Common.Constants;
    using Constants.Pages;
    using Providers;
    using Validators;
    using ViewModels.Home;

    public class HomeMediator : MediatorBase, IHomeMediator
    {
        private readonly ICandidateServiceProvider _candidateServiceProvider;
        private readonly ContactMessageServerViewModelValidator _contactMessageServerViewModelValidator;
        private readonly FeedbackServerViewModelValidator _feedbackServerViewModelValidator;
        private readonly ILogService _logService;

        public HomeMediator(
            ILogService logService,
            ICandidateServiceProvider candidateServiceProvider,
            ContactMessageServerViewModelValidator contactMessageServerViewModelValidator,
            FeedbackServerViewModelValidator feedbackServerViewModelValidator)
        {
            _logService = logService;
            _candidateServiceProvider = candidateServiceProvider;
            _contactMessageServerViewModelValidator = contactMessageServerViewModelValidator;
            _feedbackServerViewModelValidator = feedbackServerViewModelValidator;
        }

        public MediatorResponse<ContactMessageViewModel> SendContactMessage(Guid? candidateId, ContactMessageViewModel contactMessageViewModel)
        {
            var validationResult = _contactMessageServerViewModelValidator.Validate(contactMessageViewModel);

            if (!validationResult.IsValid)
            {
                PopulateContactMessageViewModelEnquiries(contactMessageViewModel);
                return GetMediatorResponse(HomeMediatorCodes.SendContactMessage.ValidationError, contactMessageViewModel, validationResult);
            }

            if (_candidateServiceProvider.SendContactMessage(candidateId, contactMessageViewModel))
            {
                var viewModel = InternalGetContactMessageViewModel(candidateId);
                PopulateContactMessageViewModelEnquiries(viewModel);
                return GetMediatorResponse(HomeMediatorCodes.SendContactMessage.SuccessfullySent,
                    viewModel, ApplicationPageMessages.SendContactMessageSucceeded, UserMessageLevel.Success);
            }

            PopulateContactMessageViewModelEnquiries(contactMessageViewModel);

            return GetMediatorResponse(HomeMediatorCodes.SendContactMessage.Error, contactMessageViewModel,
                ApplicationPageMessages.SendContactMessageFailed, UserMessageLevel.Warning);
        }

        public MediatorResponse<FeedbackViewModel> SendFeedback(Guid? candidateId, FeedbackViewModel feedbackViewModel)
        {
            var validationResult = _feedbackServerViewModelValidator.Validate(feedbackViewModel);

            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(HomeMediatorCodes.SendFeedback.ValidationError, feedbackViewModel, validationResult);
            }

            if (_candidateServiceProvider.SendFeedback(candidateId, feedbackViewModel))
            {
                var viewModel = InternalGetFeedbackViewModel(candidateId);

                return GetMediatorResponse(HomeMediatorCodes.SendFeedback.SuccessfullySent,
                    viewModel, ApplicationPageMessages.SendFeedbackSucceeded, UserMessageLevel.Success);
            }

            return GetMediatorResponse(HomeMediatorCodes.SendFeedback.Error, feedbackViewModel,
                ApplicationPageMessages.SendFeedbackFailed, UserMessageLevel.Warning);
        }

        public MediatorResponse<ContactMessageViewModel> GetContactMessageViewModel(Guid? candidateId)
        {
            var viewModel = InternalGetContactMessageViewModel(candidateId);
            
            return GetMediatorResponse(HomeMediatorCodes.GetContactMessageViewModel.Successful, viewModel);
        }

        public MediatorResponse<FeedbackViewModel> GetFeedbackViewModel(Guid? candidateId)
        {
            var viewModel = InternalGetFeedbackViewModel(candidateId);

            return GetMediatorResponse(HomeMediatorCodes.GetFeedbackViewModel.Successful, viewModel);
        }

        protected static SelectList GetEnquiries()
        {
            var enquiries = new SelectList(
                new[]
                {
                    new {Id="noSelect", Enquiry = "-- Or choose from one of these questions --" },
                    new {Id="changeEmailAddress", Enquiry = "I need to change my email address" },
                    new {Id="forgottenEmailAddress", Enquiry = "I've forgotten my email address"},
                    new {Id="cantSignIn", Enquiry = "I can't sign in to my account"},
                    new {Id="cantActivate", Enquiry = "I can't activate my account"},
                    new {Id="knowMore", Enquiry = "I'd like to know more about eligibility and funding"},
                    new {Id="problemsSubmitting", Enquiry = "I'm having problems submitting my application"},
                    new {Id="noApplication", Enquiry = "I can't see my application"},
                    new {Id="withdrawApplication", Enquiry = "I want to withdraw an application"},
                    new {Id="deleteAccount", Enquiry = "I want to delete my account" }
                },
                "Id",
                "Enquiry"
                );

            return enquiries;
        }

        #region Helpers

        private ContactMessageViewModel InternalGetContactMessageViewModel(Guid? candidateId)
        {
            var viewModel = new ContactMessageViewModel();
            PopulateContactMessageViewModelEnquiries(viewModel);

            if (candidateId.HasValue)
            {
                try
                {
                    var candidate = _candidateServiceProvider.GetCandidate(candidateId.Value);

                    viewModel.Email = candidate.RegistrationDetails.EmailAddress;
                    viewModel.Name = string.Format("{0} {1}",
                        candidate.RegistrationDetails.FirstName, candidate.RegistrationDetails.LastName);
                }
                catch(Exception ex)
                {
                    _logService.Error("Failed to created view model (ignoring)", ex);
                }
            }

            return viewModel;
        }

        private FeedbackViewModel InternalGetFeedbackViewModel(Guid? candidateId)
        {
            var viewModel = new FeedbackViewModel();

            if (candidateId.HasValue)
            {
                try
                {
                    var candidate = _candidateServiceProvider.GetCandidate(candidateId.Value);

                    viewModel.Email = candidate.RegistrationDetails.EmailAddress;
                    viewModel.Name = string.Format("{0} {1}",
                        candidate.RegistrationDetails.FirstName, candidate.RegistrationDetails.LastName);
                }
                catch (Exception ex)
                {
                    _logService.Error("Failed to created view model (ignoring)", ex);
                }
            }

            return viewModel;
        }

        private static void PopulateContactMessageViewModelEnquiries(ContactMessageViewModel contactMessageViewModel)
        {
            contactMessageViewModel.Enquiries = GetEnquiries();
            contactMessageViewModel.SelectedEnquiry = "noSelect";
        }

        #endregion
    }
}