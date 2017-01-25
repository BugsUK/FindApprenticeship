namespace SFA.Apprenticeships.Web.Manage.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces;
    using Attributes;
    using Common.Attributes;
    using Common.Mediators;
    using Constants;
    using Domain.Entities.Raa;
    using FluentValidation.Mvc;
    using Raa.Common.Mediators.Admin;
    using Raa.Common.ViewModels.Api;
    using Raa.Common.ViewModels.Employer;
    using Raa.Common.ViewModels.Provider;
    using System.Web.Mvc;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.ReferenceData;
    using Raa.Common.ViewModels.Vacancy;

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
                    return RedirectToRoute(ManagementRouteNames.AdminViewProvider, new { viewModel.ProviderId });

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
        public ActionResult DeleteProviderSiteRelationship(int providerSiteRelationshipId)
        {
            var response = _adminMediator.GetProviderSiteRelationship(providerSiteRelationshipId);

            return View(response.ViewModel);
        }

        [HttpPost]
        [MultipleFormActionsButton(SubmitButtonActionName = "ConfirmDeleteProviderSiteRelationshipAction")]
        public ActionResult ConfirmDeleteProviderSiteRelationship(int providerSiteRelationshipId)
        {
            var response = _adminMediator.DeleteProviderSiteRelationship(providerSiteRelationshipId);

            ModelState.Clear();

            SetUserMessage(response.Message);

            switch (response.Code)
            {
                case AdminMediatorCodes.DeleteProviderSiteRelationship.Error:
                    return RedirectToRoute(ManagementRouteNames.AdminDeleteProviderSiteRelationship, new { response.ViewModel.ProviderSiteRelationshipId });

                case AdminMediatorCodes.DeleteProviderSiteRelationship.Ok:
                    return RedirectToRoute(ManagementRouteNames.AdminViewProviderSite, new { response.ViewModel.ProviderSiteId });

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
        public ActionResult Employers(EmployerSearchViewModel viewModel)
        {
            var response = _adminMediator.SearchEmployers(viewModel);

            ModelState.Clear();

            switch (response.Code)
            {
                case AdminMediatorCodes.SearchEmployers.FailedValidation:
                    response.ValidationResult.AddToModelState(ModelState, "SearchViewModel");
                    return View(response.ViewModel);

                case AdminMediatorCodes.SearchEmployers.Ok:
                    return View(response.ViewModel);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpPost]
        [MultipleFormActionsButton(SubmitButtonActionName = "SearchEmployersAction")]
        public ActionResult SearchEmployers(EmployerSearchViewModel viewModel)
        {
            viewModel.PerformSearch = true;
            return RedirectToRoute(ManagementRouteNames.AdminEmployers, viewModel.RouteValues);
        }

        [HttpGet]
        public ActionResult Employer(int employerId)
        {
            var response = _adminMediator.GetEmployer(employerId);

            return View(response.ViewModel);
        }

        [HttpPost]
        [MultipleFormActionsButton(SubmitButtonActionName = "SaveEmployerAction")]
        public ActionResult SaveEmployer(EmployerViewModel viewModel)
        {
            var response = _adminMediator.SaveEmployer(viewModel);

            ModelState.Clear();

            SetUserMessage(response.Message);

            switch (response.Code)
            {
                case AdminMediatorCodes.SaveEmployer.FailedValidation:
                    response.ValidationResult.AddToModelState(ModelState, "SearchViewModel");
                    return View("Employer", response.ViewModel);

                case AdminMediatorCodes.SaveEmployer.Error:
                    return RedirectToRoute(ManagementRouteNames.AdminViewEmployer, new { viewModel.EmployerId });

                case AdminMediatorCodes.SaveEmployer.Ok:
                    return RedirectToRoute(ManagementRouteNames.AdminViewEmployer, new { viewModel.EmployerId });

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpGet]
        public ActionResult Standards()
        {
            var response = _adminMediator.GetStandard();

            var standards = new List<EditStandardViewModel>();

            foreach(var standardSector in response.ViewModel.SelectMany(s => s.Sectors).OrderBy(s => s.Name))
            {
                var ssat1 = response.ViewModel.Single(s =>  s.Id == standardSector.ApprenticeshipOccupationId);

                standards.AddRange(standardSector.Standards.OrderBy(s => s.Name).Select(s => new EditStandardViewModel()
                {
                    Id = s.Id,
                    Name = ssat1.Name,
                    StandardSectorName = s.Name,
                    StandardName = standardSector.Name,
                    Status = s.Status
                }));
            }

            return View(standards);
        }

        [HttpPost]
        public JsonResult UpdateStandard(EditStandardViewModel standard)
        {
            var entity = new Standard() { Id = standard.Id, Status = standard.Status };
            
            var response = _adminMediator.UpdateStandard(entity);
            
            // always return success here as an exception will return of it's own accord
            return Json(new {status = "Ok"});
        }

        [HttpGet]
        public ActionResult Frameworks()
        {
            var response = _adminMediator.GetFrameworks();
            var categories = new List<EditCategoryViewModel>();
            var occupations = response.ViewModel.Select(s => new OccupationViewModel() {Id = s.Id, FullName = s.FullName, CodeName = s.CodeName.Replace("SSAT1.","") }).ToList();

            foreach (var category in response.ViewModel)
            {
                categories.AddRange(
                    category.SubCategories.Select(s => new EditCategoryViewModel()
                    {
                        Id = s.Id,
                        Code = CategoryPrefixes.GetOriginalFrameworkCode(s.CodeName),
                        SsatName = category.FullName,
                        FullName = s.FullName,
                        Status = s.Status
                    }));
            }

            var viewModel = new EditFrameworksViewModel() { Categories = categories, Occupations = occupations};

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult UpdateFramework(EditCategoryViewModel category)
        {
            category.SsatCode = CategoryPrefixes.GetOriginalSectorSubjectAreaTier1Code(category.SsatCode);

            var response = _adminMediator.UpdateFramework(category);

            return Json(new {Status = "Ok"});
        }

        [HttpPost]
        public ActionResult CreateFramework(EditCategoryViewModel category)
        {
            category.SsatCode = CategoryPrefixes.GetOriginalSectorSubjectAreaTier1Code(category.SsatCode);

            var response = _adminMediator.InsertFramework(category);

            return Json(response.ViewModel);
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