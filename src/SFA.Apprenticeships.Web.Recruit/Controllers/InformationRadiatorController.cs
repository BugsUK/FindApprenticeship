using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using Application.Interfaces;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Interfaces.Repositories;
    using Domain.Raa.Interfaces.Queries;
    using Domain.Raa.Interfaces.Repositories;
    using ViewModels;

    public class InformationRadiatorController : Controller
    {
        private IVacancyReadRepository _vacancyRepository;
        private ILogService _logService;

        public InformationRadiatorController(IVacancyReadRepository vacancyRepository,
            ILogService logService)
        {
            _vacancyRepository = vacancyRepository;
            _logService = logService;
        }

        // GET: InformationRadiator
        public ActionResult Index()
        {
            var viewModel = new InformationRadiatorViewModel();

            int total = 0;

            try
            {
                var result = _vacancyRepository.Find(new ApprenticeshipVacancyQuery()
                {
                    CurrentPage = 1,
                    PageSize = 1,
                    DesiredStatuses = new List<VacancyStatus>() {VacancyStatus.Live}
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