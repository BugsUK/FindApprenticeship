namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Admin
{
    using Domain.Entities.Raa.Vacancies;
    using System.Collections.Generic;

    public class ManageVacancyTransferResultsViewModel
    {
        public IList<Vacancy> Vacancies { get; set; }
    }
}