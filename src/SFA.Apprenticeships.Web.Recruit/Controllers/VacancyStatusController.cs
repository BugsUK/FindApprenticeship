namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using Application.Interfaces;
    using Attributes;
    using Common.Constants;
    using Constants;
    using Domain.Entities.Raa;
    using Mediators.VacancyPosting;
    using Mediators.VacancyStatus;
    using Raa.Common.ViewModels.VacancyStatus;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    [AuthorizeUser(Roles = Roles.Faa)]
    [AuthorizeUser(Roles = Roles.VerifiedEmail)]
    public class VacancyStatusController : RecruitmentControllerBase
    {
        private readonly IVacancyStatusMediator _vacancyStatusMediator;


        public VacancyStatusController(IVacancyStatusMediator vacancyStatusMediator,
            IConfigurationService configurationService,
            ILogService logService) : base(configurationService, logService)
        {
            _vacancyStatusMediator = vacancyStatusMediator;
        }

        [HttpGet]
        public ActionResult Archive(int vacancyReferenceNumber)
        {
            var response = _vacancyStatusMediator.GetArchiveVacancyViewModelByVacancyReferenceNumber(vacancyReferenceNumber);
            return View(response.ViewModel);
        }

        [HttpGet]
        public ActionResult BulkDeclineCandidates(int vacancyReferenceNumber)
        {
            var response = _vacancyStatusMediator.GetBulkDeclineCandidatesViewModelByVacancyReferenceNumber(vacancyReferenceNumber);
            return View(response.ViewModel);
        }

        [HttpPost]
        public ActionResult ConfirmBulkDeclineCandidates(IList<Guid> applicationIds, int vacancyReferencenumber)
        {
            if (applicationIds != null && applicationIds.Any())
                return RedirectToAction("ConfirmBulkUnsuccessfulDecision", "ApprenticeshipApplication",
                    routeValues: new { applicationIds = String.Join(",", applicationIds), vacancyReferencenumber = vacancyReferencenumber });
            SetUserMessage("You must select a candidate", UserMessageLevel.Error);
            return RedirectToAction("BulkDeclineCandidates",
                    routeValues: new { vacancyReferencenumber = vacancyReferencenumber });
        }

        [HttpPost]
        public ActionResult ConfirmArchive(ArchiveVacancyViewModel viewModel)
        {
            var response = _vacancyStatusMediator.ArchiveVacancy(viewModel);

            switch (response.Code)
            {
                case VacancyStatusMediatorCodes.ArchiveVacancy.Ok:
                    return RedirectToRoute(RecruitmentRouteNames.PreviewVacancy, new { vacancyReferenceNumber = viewModel.VacancyReferenceNumber });
                case VacancyStatusMediatorCodes.ArchiveVacancy.OutstandingActions:
                    return RedirectToRoute(RecruitmentRouteNames.ArchiveVacancy, new { vacancyReferenceNumber = viewModel.VacancyReferenceNumber });
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}