using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using Domain.Entities.Raa.Vacancies;
    using Domain.Interfaces.Repositories;
    using Domain.Raa.Interfaces.Queries;
    using Domain.Raa.Interfaces.Repositories;
    using ViewModels;

    public class InformationRadiatorController : Controller
    {
        private IVacancyReadRepository _vacancyRepository;

        public InformationRadiatorController(IVacancyReadRepository vacancyRepository)
        {
            _vacancyRepository = vacancyRepository;
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
                    viewModel.DatabaseError = false;
                    viewModel.DatabaseSuccess = true;
                }
            }
            catch (Exception ex)
            {
                viewModel.DatabaseError = true;
                viewModel.DatabaseException = ex;
            }

            return View(viewModel);
        }
    }
}