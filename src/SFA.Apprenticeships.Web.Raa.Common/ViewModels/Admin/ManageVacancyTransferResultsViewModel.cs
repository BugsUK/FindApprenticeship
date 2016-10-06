namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Admin
{
    using Domain.Entities.Raa.Parties;
    using Domain.Entities.Raa.Vacancies;
    using System.Collections.Generic;

    public class ManageVacancyTransferResultsViewModel
    {
        public IList<IDictionary<Vacancy, VacancyParty>> Vacancies { get; set; }
    }
}