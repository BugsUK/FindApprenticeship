using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using Application.Interfaces;
    using Application.Vacancy;
    using Common.Attributes;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Raa.Interfaces.Queries;
    using Domain.Raa.Interfaces.Repositories;
    using Raa.Common.Configuration;
    using ViewModels;

    [ApplyAnalytics(typeof(RecruitWebConfiguration))]
    public class InformationRadiatorController : Controller
    {
        private IVacancyReadRepository _vacancyRepository;
        private ILogService _logService;
        private IVacancySummaryService _vacancySummaryService;

        public InformationRadiatorController(IVacancyReadRepository vacancyRepository,
            ILogService logService,
            IVacancySummaryService vacancySummaryService)
        {
            _vacancyRepository = vacancyRepository;
            _vacancySummaryService = vacancySummaryService;
            _logService = logService;
        }

        // GET: InformationRadiator
        public ActionResult Index()
        {
            var viewModel = new InformationRadiatorViewModel();

            int total = 0;

            try
            {
                var result = _vacancySummaryService.Find(new ApprenticeshipVacancyQuery()
                {
                    RequestedPage = 1,
                    PageSize = 1,
                    DesiredStatuses = new List<VacancyStatus>() { VacancyStatus.Live }
                }, out total);

                if (result != null && result.Count >= 1)
                {
                    viewModel.DatabaseStatus = InformationRadiatorStatus.Success;
                }
                else
                {
                    viewModel.DatabaseStatus = InformationRadiatorStatus.Warning;
                }
            }
            catch (Exception ex)
            {
                _logService.Error("Information radiator failure", ex);
                viewModel.DatabaseStatus = InformationRadiatorStatus.Exception;
            }

            return View(viewModel);
        }
    }
}