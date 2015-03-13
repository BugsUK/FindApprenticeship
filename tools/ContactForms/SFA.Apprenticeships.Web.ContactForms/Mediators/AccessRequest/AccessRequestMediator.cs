namespace SFA.Apprenticeships.Web.ContactForms.Mediators.AccessRequest
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Constants;
    using Constants.Pages;
    using Domain.Enums;
    using Interfaces;
    using Providers.Interfaces;
    using Validators;
    using ViewModels;

    public class AccessRequestMediator : MediatorBase, IAccessRequestMediator
    {
        private readonly IAccessRequestProvider _accessRequestProvider;
        private readonly AccessRequestViewModelServerValidator _validator;
        private readonly IReferenceDataMediator _referenceDataMediator;

        public AccessRequestMediator(IAccessRequestProvider accessRequestProvider, AccessRequestViewModelServerValidator validator, IReferenceDataMediator referenceDataMediator)
        {
            _accessRequestProvider = accessRequestProvider;
            _validator = validator;
            _referenceDataMediator = referenceDataMediator;
        }

        public MediatorResponse<AccessRequestViewModel> SubmitAccessRequest()
        {
            var viewModel = new AccessRequestViewModel();
            viewModel = PopulateStaticData(viewModel);
            return GetMediatorResponse(AccessRequestMediatorCodes.SubmitAccessRequest.Success, viewModel);
        }

        public MediatorResponse<AccessRequestViewModel> SubmitAccessRequest(AccessRequestViewModel viewModel)
        {
            var validationResult = _validator.Validate(viewModel);

            if (!validationResult.IsValid)
            {
                viewModel = PopulateStaticData(viewModel);
                return GetMediatorResponse(AccessRequestMediatorCodes.SubmitAccessRequest.ValidationError, viewModel, validationResult);
            }
            AccessRequestSubmitStatus resultStatus = _accessRequestProvider.SubmitAccessRequest(viewModel);
            //populate reference data
            viewModel = PopulateStaticData(viewModel);

            switch (resultStatus)
            {
                case AccessRequestSubmitStatus.Success:
                    return GetMediatorResponse(AccessRequestMediatorCodes.SubmitAccessRequest.Success, viewModel, AccessRequestPageMessages.RequestHasBeenSubmittedSuccessfully, UserMessageLevel.Success);
                default:
                    return GetMediatorResponse(AccessRequestMediatorCodes.SubmitAccessRequest.Error, viewModel, AccessRequestPageMessages.ErrorWhileRequestSubmission, UserMessageLevel.Error);
            }
        }

        private AccessRequestViewModel PopulateStaticData(AccessRequestViewModel viewModel)
        {
            viewModel.UserTypeList = GetAccessRequestAreaTypes();
            viewModel.TitleList = GetTitleTypes();
            viewModel.ServiceTypes = GetAccessRequestServiceTypes(viewModel);
            return viewModel;
        }

        #region Helper Methods
        private SelectList GetTitleTypes()
        {
            var referenceData = _referenceDataMediator.GetReferenceData(ReferenceDataTypes.Titles).ViewModel.ReferenceData;
            return new SelectList(referenceData, CommonConstants.Id, CommonConstants.Description, string.Empty);
        }

        private AccessRequestServicesViewModel GetAccessRequestServiceTypes(AccessRequestViewModel viewModel)
        {
            var referenceData = _referenceDataMediator.GetReferenceData(ReferenceDataTypes.AccessRequestServiceTypes).ViewModel.ReferenceData;
            var servicesViewModel = new AccessRequestServicesViewModel();

            var availableServices = referenceData.Select(data => new AccessRequestServiceTypes()
            {
                Id = data.Id,
                Description = data.Description
            }).ToList();
            //Set availableServices  
            servicesViewModel.AvailableServices = availableServices;

            if (viewModel.ServiceTypes != null)
            {
                var selectedServices =
                    viewModel.ServiceTypes.PostedServiceIds.Select(data => new AccessRequestServiceTypes()
                    {
                        Id = data, IsSelected = true
                    }).ToList();
                servicesViewModel.SelectedServices = selectedServices;
                servicesViewModel.PostedServiceIds = viewModel.ServiceTypes.PostedServiceIds;
            }

            return servicesViewModel;
        }

        private SelectList GetAccessRequestAreaTypes()
        {
            var referenceData = _referenceDataMediator.GetReferenceData(ReferenceDataTypes.AccessRequestAreaTypes).ViewModel.ReferenceData;
            return new SelectList(referenceData, CommonConstants.Id, CommonConstants.Description, string.Empty);
        }
        #endregion
    }
}