namespace SFA.Apprenticeships.Web.ContactForms.Mediators.EmployerEnquiry
{
    using System.Web.Mvc;
    using ContactForms.Constants;
    using ContactForms.Constants.Pages;
    using Domain.Enums;
    using Interfaces;
    using Providers.Interfaces;
    using Validators;
    using ViewModels;

    public class EmployerEnquiryMediator : MediatorBase, IEmployerEnquiryMediator
    {
        private readonly IEmployerEnquiryProvider _employerEnquiryProvider;
        private readonly EmployerEnquiryViewModelServerValidator _validator;
        private readonly IReferenceDataMediator _referenceDataMediator;
        public EmployerEnquiryMediator(IEmployerEnquiryProvider employerEnquiryProvider, 
                                        EmployerEnquiryViewModelServerValidator validator, 
                                    IReferenceDataMediator referenceDataMediator)
        {
            _employerEnquiryProvider = employerEnquiryProvider;
            _validator = validator;
            _referenceDataMediator = referenceDataMediator;
        }
        
        public MediatorResponse<EmployerEnquiryViewModel> SubmitEnquiry()
        {
            var model = new EmployerEnquiryViewModel
            {
                //Get The various reference data list
                EmployeesCountList = GetEmployeeCountTypes(),
                WorkSectorList = GetWorkSectorTypes(),
                PreviousExperienceTypeList = GetPreviousExperienceTypes(),
                TitleList = GetTitleTypes(),
                EnquirySourceList = GetEnquirySourceTypes(),
                EnquiryRelatesToList = GetEnquiryRelatesTo()
            };
            return GetMediatorResponse(EmployerEnquiryMediatorCodes.SubmitEnquiry.Success, model);
        }

        public MediatorResponse<EmployerEnquiryViewModel> SubmitEnquiry(EmployerEnquiryViewModel viewModel)
        {
            var validationResult = _validator.Validate(viewModel);

            if (!validationResult.IsValid)
            {
                viewModel = PopulateStaticData(viewModel, false);
                return GetMediatorResponse(EmployerEnquiryMediatorCodes.SubmitEnquiry.ValidationError, viewModel, validationResult);
            }

            //todo: add other cases..
            SubmitQueryStatus resultStatus = _employerEnquiryProvider.SubmitEnquiry(viewModel);
            //populate reference data
            viewModel = PopulateStaticData(viewModel, false);

            switch (resultStatus)
            {
                case SubmitQueryStatus.Success:
                    return GetMediatorResponse(EmployerEnquiryMediatorCodes.SubmitEnquiry.Success, viewModel, EmployerEnquiryPageMessages.QueryHasBeenSubmittedSuccessfully, UserMessageLevel.Success);
                default:
                    return GetMediatorResponse(EmployerEnquiryMediatorCodes.SubmitEnquiry.Error, viewModel, EmployerEnquiryPageMessages.ErrorWhileQuerySubmission, UserMessageLevel.Error);
            }
        }

        public MediatorResponse<EmployerEnquiryViewModel> SubmitGlaEnquiry()
        {
            var model = new EmployerEnquiryViewModel
            {
                //Get The various reference data list
                EmployeesCountList = GetEmployeeCountTypes(),
                WorkSectorList = GetWorkSectorTypes(),
                PreviousExperienceTypeList = GetPreviousExperienceTypes(),
                TitleList = GetTitleTypes(),
                EnquirySourceList = GetGlaEnquirySourceTypes(),
                EnquiryRelatesToList = GetEnquiryRelatesTo()
            };
            return GetMediatorResponse(EmployerEnquiryMediatorCodes.SubmitEnquiry.Success, model);
        }

        public MediatorResponse<EmployerEnquiryViewModel> SubmitGlaEnquiry(EmployerEnquiryViewModel viewModel)
        {
            var validationResult = _validator.Validate(viewModel);

            if (!validationResult.IsValid)
            {
                viewModel = PopulateStaticData(viewModel, true);
                return GetMediatorResponse(EmployerEnquiryMediatorCodes.SubmitEnquiry.ValidationError, viewModel, validationResult);
            }

            //todo: add other cases..
            SubmitQueryStatus resultStatus = _employerEnquiryProvider.SubmitGlaEnquiry(viewModel);
            //populate reference data
            viewModel = PopulateStaticData(viewModel, true);

            switch (resultStatus)
            {
                case SubmitQueryStatus.Success:
                    return GetMediatorResponse(EmployerEnquiryMediatorCodes.SubmitEnquiry.Success, viewModel, EmployerEnquiryPageMessages.QueryHasBeenSubmittedSuccessfully, UserMessageLevel.Success);
                default:
                    return GetMediatorResponse(EmployerEnquiryMediatorCodes.SubmitEnquiry.Error, viewModel, EmployerEnquiryPageMessages.ErrorWhileQuerySubmission, UserMessageLevel.Error);
            }
        }

        private EmployerEnquiryViewModel PopulateStaticData(EmployerEnquiryViewModel viewModel, bool isGla)
        {
            viewModel.EmployeesCountList = GetEmployeeCountTypes();
            viewModel.WorkSectorList = GetWorkSectorTypes();
            viewModel.PreviousExperienceTypeList = GetPreviousExperienceTypes();
            viewModel.EnquirySourceList = isGla ? GetGlaEnquirySourceTypes() : GetEnquirySourceTypes();
            viewModel.EmployeesCountList = GetEmployeeCountTypes();
            viewModel.TitleList = GetTitleTypes();
            viewModel.EnquiryRelatesToList = GetEnquiryRelatesTo();
            return viewModel;
        }

        #region Helper Methods

        private SelectList GetTitleTypes()
        {
            var titleTypesKeyValuePair = _referenceDataMediator.GetReferenceData(ReferenceDataTypes.Titles).ViewModel.ReferenceData;
            return new SelectList(titleTypesKeyValuePair, CommonConstants.Id, CommonConstants.Description, string.Empty);
        }

        private SelectList GetEmployeeCountTypes()
        {
            var employeeCountKeyValuePair = _referenceDataMediator.GetReferenceData(ReferenceDataTypes.EmployeeCountTypes).ViewModel.ReferenceData;
            return new SelectList(employeeCountKeyValuePair, CommonConstants.Id, CommonConstants.Description, string.Empty);
        }

        private SelectList GetWorkSectorTypes()
        {
            var workSectorTypesKeyValuePair = _referenceDataMediator.GetReferenceData(ReferenceDataTypes.WorkSectorTypes).ViewModel.ReferenceData;
            return new SelectList(workSectorTypesKeyValuePair, CommonConstants.Id, CommonConstants.Description, string.Empty);
        }

        private SelectList GetPreviousExperienceTypes()
        {
            var previousExperienceKeyValuePair = _referenceDataMediator.GetReferenceData(ReferenceDataTypes.PreviousExperienceTypes).ViewModel.ReferenceData;
            return new SelectList(previousExperienceKeyValuePair, CommonConstants.Id, CommonConstants.Description, string.Empty);
        }

        private SelectList GetEnquirySourceTypes()
        {
            var enquirySourceKeyValuePair = _referenceDataMediator.GetReferenceData(ReferenceDataTypes.EnquirySourceTypes).ViewModel.ReferenceData;
            return new SelectList(enquirySourceKeyValuePair, CommonConstants.Id, CommonConstants.Description, string.Empty);
        }

        private SelectList GetGlaEnquirySourceTypes()
        {
            var enquirySourceKeyValuePair = _referenceDataMediator.GetReferenceData(ReferenceDataTypes.GlaEnquirySourceTypes).ViewModel.ReferenceData;
            return new SelectList(enquirySourceKeyValuePair, CommonConstants.Id, CommonConstants.Description, string.Empty);
        }

        private SelectList GetEnquiryRelatesTo()
        {
            var enquiryRelatesToKeyValuePair = _referenceDataMediator.GetReferenceData(ReferenceDataTypes.EnquiryRelatesTo).ViewModel.ReferenceData;
            return new SelectList(enquiryRelatesToKeyValuePair, CommonConstants.Id, CommonConstants.Description, string.Empty);
        } 
        #endregion
    }
}