namespace SFA.Apprenticeships.Web.Recruit.Mediators.Admin
{
    using Apprenticeships.Application.Interfaces;
    using Apprenticeships.Application.Interfaces.Providers;
    using Apprenticeships.Application.Interfaces.VacancyPosting;
    using Common.Constants;
    using Common.Mediators;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.Vacancies;
    using System;
    using System.Collections.Generic;
    using Raa.Common.Mediators.Admin;
    using Raa.Common.ViewModels.Admin;

    public class AdminMediator : MediatorBase, IAdminMediator
    {
        private readonly IVacancyPostingService _vacancyPostingService;
        private readonly IProviderService _providerService;

        public AdminMediator(IVacancyPostingService vacancyPostingService, IProviderService providerService)
        {
            _vacancyPostingService = vacancyPostingService;
            _providerService = providerService;
        }

        public MediatorResponse<TransferVacanciesResultsViewModel> GetVacancyDetails(TransferVacanciesViewModel viewModel)
        {
            try
            {
                IList<TransferVacancyViewModel> vacanciesToBeTransferred = new List<TransferVacancyViewModel>();
                TransferVacanciesResultsViewModel transferVacanciesResultsViewModel = new TransferVacanciesResultsViewModel()
                {
                    TransferVacanciesViewModel = viewModel
                };

                foreach (var vacancy in viewModel.VacancyReferenceNumbers.Split(','))
                {
                    string vacancyReference;
                    if (VacancyHelper.TryGetVacancyReference(vacancy, out vacancyReference))
                    {
                        Vacancy vacancyDetails = _vacancyPostingService.GetVacancyByReferenceNumber(Convert.ToInt32(vacancyReference));
                        if (vacancyDetails != null)
                        {
                            TransferVacancyViewModel vacancyView = new TransferVacancyViewModel
                            {
                                ContractOwnerId = vacancyDetails.ProviderId,
                                VacancyManagerId = vacancyDetails.VacancyManagerId,
                                VacancyReferenceNumber = vacancyDetails.VacancyReferenceNumber,
                                DeliveryOrganisationId = vacancyDetails.DeliveryOrganisationId,
                                VacancyOwnerRelationShipId = vacancyDetails.VacancyOwnerRelationshipId,
                                ProviderName = _providerService.GetProvider(vacancyDetails.ProviderId).TradingName,
                            };
                            if (vacancyDetails.VacancyManagerId.HasValue)
                            {
                                vacancyView.ProviderSiteName =
                                    _providerService.GetProviderSite(vacancyDetails.VacancyManagerId.Value).TradingName;
                            }
                            vacanciesToBeTransferred.Add(vacancyView);
                        }
                    }
                }
                transferVacanciesResultsViewModel.VacanciesToBeTransferredVm = vacanciesToBeTransferred;
                return GetMediatorResponse(AdminMediatorCodes.GetVacancyDetails.Ok, transferVacanciesResultsViewModel);
            }
            catch (CustomException exception) when (exception.Code == ErrorCodes.ProviderVacancyAuthorisation.Failed)
            {
                return GetMediatorResponse(AdminMediatorCodes.GetVacancyDetails.FailedAuthorisation, new TransferVacanciesResultsViewModel(), TransferVacanciesMessages.UnAuthorisedAccess, UserMessageLevel.Warning);
            }
        }
    }
}