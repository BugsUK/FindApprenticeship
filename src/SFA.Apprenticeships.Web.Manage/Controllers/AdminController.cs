namespace SFA.Apprenticeships.Web.Manage.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Application.Interfaces;
    using Attributes;
    using Common.Attributes;
    using Common.Mediators;
    using Constants;
    using Domain.Entities.Raa;
    using Domain.Entities.Raa.Vacancies;
    using FluentValidation.Mvc;
    using Raa.Common.Mediators.Admin;
    using Raa.Common.ViewModels.Api;
    using Raa.Common.ViewModels.Provider;

    [AuthorizeUser(Roles = Roles.Raa)]
    [AuthorizeUser(Roles = Roles.Admin)]
    public class AdminController : ManagementControllerBase
    {
        private readonly IAdminMediator _adminMediator;

        public AdminController(IAdminMediator adminMediator, IConfigurationService configurationService, ILogService loggingService) : base(configurationService, loggingService)
        {
            _adminMediator = adminMediator;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Providers(ProviderSearchViewModel viewModel)
        {
            var response = _adminMediator.SearchProviders(viewModel);

            ModelState.Clear();

            switch (response.Code)
            {
                case AdminMediatorCodes.SearchProviders.FailedValidation:
                    response.ValidationResult.AddToModelState(ModelState, "SearchViewModel");
                    return View(response.ViewModel);

                case AdminMediatorCodes.SearchProviders.Ok:
                    return View(response.ViewModel);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpPost]
        [MultipleFormActionsButton(SubmitButtonActionName = "SearchProvidersAction")]
        public ActionResult SearchProviders(ProviderSearchResultsViewModel viewModel)
        {
            viewModel.SearchViewModel.PerformSearch = true;
            return RedirectToRoute(ManagementRouteNames.AdminProviders, viewModel.SearchViewModel);
        }

        [HttpGet]
        public ActionResult Provider(int providerId)
        {
            var response = _adminMediator.GetProvider(providerId);

            return View(response.ViewModel);
        }

        [HttpGet]
        public ActionResult CreateProvider()
        {
            return View(new ProviderViewModel());
        }

        [HttpPost]
        [MultipleFormActionsButton(SubmitButtonActionName = "CreateProviderAction")]
        public ActionResult CreateProvider(ProviderViewModel viewModel)
        {
            var response = _adminMediator.CreateProvider(viewModel);

            ModelState.Clear();

            SetUserMessage(response.Message);

            switch (response.Code)
            {
                case AdminMediatorCodes.CreateProvider.FailedValidation:
                    response.ValidationResult.AddToModelState(ModelState, "SearchViewModel");
                    return View(response.ViewModel);

                case AdminMediatorCodes.CreateProvider.UkprnAlreadyExists:
                    return View(response.ViewModel);

                case AdminMediatorCodes.CreateProvider.Ok:
                    return View(response.ViewModel);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpPost]
        [MultipleFormActionsButton(SubmitButtonActionName = "SaveProviderAction")]
        public ActionResult SaveProvider(ProviderViewModel viewModel)
        {
            var response = _adminMediator.SaveProvider(viewModel);

            ModelState.Clear();

            SetUserMessage(response.Message);

            switch (response.Code)
            {
                case AdminMediatorCodes.SaveProvider.FailedValidation:
                    response.ValidationResult.AddToModelState(ModelState, "SearchViewModel");
                    return View("Provider", response.ViewModel);

                case AdminMediatorCodes.SaveProvider.Error:
                    return RedirectToRoute(ManagementRouteNames.AdminViewProvider, new { viewModel.ProviderId });

                case AdminMediatorCodes.SaveProvider.Ok:
                    return RedirectToRoute(ManagementRouteNames.AdminViewProvider, new { viewModel.ProviderId });

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpGet]
        public ActionResult ProviderSites(ProviderSiteSearchViewModel viewModel)
        {
            var response = _adminMediator.SearchProviderSites(viewModel);

            ModelState.Clear();

            switch (response.Code)
            {
                case AdminMediatorCodes.SearchProviderSites.FailedValidation:
                    response.ValidationResult.AddToModelState(ModelState, "SearchViewModel");
                    return View(response.ViewModel);

                case AdminMediatorCodes.SearchProviderSites.Ok:
                    return View(response.ViewModel);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpPost]
        [MultipleFormActionsButton(SubmitButtonActionName = "SearchProviderSitesAction")]
        public ActionResult SearchProviderSites(ProviderSiteSearchResultsViewModel viewModel)
        {
            viewModel.SearchViewModel.PerformSearch = true;
            return RedirectToRoute(ManagementRouteNames.AdminProviderSites, viewModel.SearchViewModel);
        }

        [HttpGet]
        public ActionResult ProviderSite(int providerSiteId)
        {
            var response = _adminMediator.GetProviderSite(providerSiteId);

            return View(response.ViewModel);
        }

        [HttpGet]
        public ActionResult CreateProviderSite(int providerId)
        {
            return View(new ProviderSiteViewModel { ProviderId = providerId });
        }

        [HttpPost]
        [MultipleFormActionsButton(SubmitButtonActionName = "CreateProviderSiteAction")]
        public ActionResult CreateProviderSite(ProviderSiteViewModel viewModel)
        {
            var response = _adminMediator.CreateProviderSite(viewModel);

            ModelState.Clear();

            SetUserMessage(response.Message);

            switch (response.Code)
            {
                case AdminMediatorCodes.CreateProviderSite.FailedValidation:
                    response.ValidationResult.AddToModelState(ModelState, "SearchViewModel");
                    return View(response.ViewModel);

                case AdminMediatorCodes.CreateProviderSite.EdsUrnAlreadyExists:
                    return View(response.ViewModel);

                case AdminMediatorCodes.CreateProviderSite.Ok:
                    return RedirectToRoute(ManagementRouteNames.AdminViewProvider, new {viewModel.ProviderId});

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpPost]
        [MultipleFormActionsButton(SubmitButtonActionName = "SaveProviderSiteAction")]
        public ActionResult SaveProviderSite(ProviderSiteViewModel viewModel)
        {
            var response = _adminMediator.SaveProviderSite(viewModel);

            ModelState.Clear();

            SetUserMessage(response.Message);

            switch (response.Code)
            {
                case AdminMediatorCodes.SaveProviderSite.FailedValidation:
                    response.ValidationResult.AddToModelState(ModelState, "SearchViewModel");
                    return View("ProviderSite", response.ViewModel);

                case AdminMediatorCodes.SaveProviderSite.Error:
                    return RedirectToRoute(ManagementRouteNames.AdminViewProviderSite, new { viewModel.ProviderSiteId });

                case AdminMediatorCodes.SaveProviderSite.Ok:
                    return RedirectToRoute(ManagementRouteNames.AdminViewProviderSite, new { viewModel.ProviderSiteId });

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpPost]
        [MultipleFormActionsButton(SubmitButtonActionName = "CreateProviderSiteRelationshipAction")]
        public ActionResult CreateProviderSiteRelationship(ProviderSiteViewModel viewModel)
        {
            var response = _adminMediator.CreateProviderSiteRelationship(viewModel);

            ModelState.Clear();

            SetUserMessage(response.Message);

            switch (response.Code)
            {
                case AdminMediatorCodes.CreateProviderSiteRelationship.FailedValidation:
                    response.ValidationResult.AddToModelState(ModelState, "SearchViewModel");
                    return View("ProviderSite", response.ViewModel);

                case AdminMediatorCodes.CreateProviderSiteRelationship.InvalidUkprn:
                    return RedirectToRoute(ManagementRouteNames.AdminViewProviderSite, new { viewModel.ProviderSiteId });

                case AdminMediatorCodes.CreateProviderSiteRelationship.Error:
                    return RedirectToRoute(ManagementRouteNames.AdminViewProviderSite, new { viewModel.ProviderSiteId });

                case AdminMediatorCodes.CreateProviderSiteRelationship.Ok:
                    return RedirectToRoute(ManagementRouteNames.AdminViewProviderSite, new { viewModel.ProviderSiteId });

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpGet]
        public ActionResult ApiUsers(ApiUserSearchViewModel viewModel)
        {
            var response = _adminMediator.SearchApiUsers(viewModel);

            ModelState.Clear();

            switch (response.Code)
            {
                case AdminMediatorCodes.SearchApiUsers.FailedValidation:
                    response.ValidationResult.AddToModelState(ModelState, "SearchViewModel");
                    return View(response.ViewModel);

                case AdminMediatorCodes.SearchApiUsers.Ok:
                    return View(response.ViewModel);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpPost]
        [MultipleFormActionsButton(SubmitButtonActionName = "SearchApiUsersAction")]
        public ActionResult SearchApiUsers(ApiUserSearchResultsViewModel viewModel)
        {
            viewModel.SearchViewModel.PerformSearch = true;
            return RedirectToRoute(ManagementRouteNames.AdminApiUsers, viewModel.SearchViewModel);
        }

        [HttpGet]
        public ActionResult ApiUser(Guid externalSystemId)
        {
            var response = _adminMediator.GetApiUser(externalSystemId);

            return View(response.ViewModel);
        }

        [HttpGet]
        public ActionResult CreateApiUser()
        {
            return View(new ApiUserViewModel());
        }

        [HttpPost]
        [MultipleFormActionsButton(SubmitButtonActionName = "CreateApiUserAction")]
        public ActionResult CreateApiUser(ApiUserViewModel viewModel)
        {
            var response = _adminMediator.CreateApiUser(viewModel);

            ModelState.Clear();

            SetUserMessage(response.Message);

            switch (response.Code)
            {
                case AdminMediatorCodes.CreateApiUser.FailedValidation:
                    response.ValidationResult.AddToModelState(ModelState, "SearchViewModel");
                    return View(response.ViewModel);

                case AdminMediatorCodes.CreateApiUser.CompanyIdAlreadyExists:
                    return View(response.ViewModel);

                case AdminMediatorCodes.CreateApiUser.UnknownCompanyId:
                    return View(response.ViewModel);

                case AdminMediatorCodes.CreateApiUser.Error:
                    return View(response.ViewModel);

                case AdminMediatorCodes.CreateApiUser.Ok:
                    return View("ApiUser", response.ViewModel);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpPost]
        [MultipleFormActionsButton(SubmitButtonActionName = "SaveApiUserAction")]
        public ActionResult SaveApiUser(ApiUserViewModel viewModel)
        {
            var response = _adminMediator.SaveApiUser(viewModel);

            ModelState.Clear();

            SetUserMessage(response.Message);

            switch (response.Code)
            {
                case AdminMediatorCodes.SaveApiUser.FailedValidation:
                    response.ValidationResult.AddToModelState(ModelState, "SearchViewModel");
                    return View("ApiUser", response.ViewModel);

                case AdminMediatorCodes.SaveApiUser.Error:
                    return RedirectToRoute(ManagementRouteNames.AdminViewApiUser, new { viewModel.ExternalSystemId });

                case AdminMediatorCodes.SaveApiUser.Ok:
                    return RedirectToRoute(ManagementRouteNames.AdminViewApiUser, new { viewModel.ExternalSystemId });

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpGet]
        public ActionResult ResetApiUserPassword(Guid externalSystemId)
        {
            var response = _adminMediator.GetApiUser(externalSystemId);

            return View(response.ViewModel);
        }

        [HttpPost]
        [MultipleFormActionsButton(SubmitButtonActionName = "ResetApiUserPasswordAction")]
        public ActionResult ResetApiUserPassword(ApiUserViewModel viewModel)
        {
            var response = _adminMediator.ResetApiUserPassword(viewModel);

            ModelState.Clear();

            SetUserMessage(response.Message);

            switch (response.Code)
            {
                case AdminMediatorCodes.ResetApiUserPassword.FailedValidation:
                    response.ValidationResult.AddToModelState(ModelState, "SearchViewModel");
                    return View("ResetApiUserPassword", response.ViewModel);

                case AdminMediatorCodes.ResetApiUserPassword.Error:
                    return RedirectToRoute(ManagementRouteNames.AdminResetApiUserPassword, new { viewModel.ExternalSystemId });

                case AdminMediatorCodes.ResetApiUserPassword.Ok:
                    return View("ApiUser", response.ViewModel);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpGet]
        public ActionResult ExportApiUsers()
        {
            var response = _adminMediator.GetApiUsersBytes();
            return File(response.ViewModel, "text/csv", "ApiUsers.csv");
        }

        [HttpGet]
        public ActionResult Standards()
        {
            var response = _adminMediator.GetStandard();
            return View(response.ViewModel);
        }

        [HttpGet]
        public ActionResult Frameworks()
        {
            var response = _adminMediator.GetFrameworks();
            return View(response.ViewModel);
        }

        [HttpGet]
        public ActionResult DownloadFrameworksCsv()
        {
            var response = _adminMediator.GetFrameworksBytes();
            return File(response.ViewModel, "text/csv", "FrameworkList.csv");
        }

        [HttpGet]
        public ActionResult DownloadStandardsCsv()
        {
            var response = _adminMediator.GetStandardsBytes();
            return File(response.ViewModel, "text/csv", "StandardsList.csv");
        }
    }
}