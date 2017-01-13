namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.VacancyManagement
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Converters;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.Vacancies;
    using Vacancy;
    using Web.Common.ViewModels;

    public class EditWageViewModel : WageUpdate
    {
        public EditWageViewModel()
        {
            WageUnits = ApprenticeshipVacancyConverter.GetWageUnits();
        }

        public int VacancyReferenceNumber { get; set; }

        public VacancyApplicationsState VacancyApplicationsState { get; set; }

        public WageClassification Classification { get; set; }

        public CustomWageType CustomType { get; set; }

        public WageUnit RangeUnit { get; set; }

        public List<SelectListItem> WageUnits { get; set; }
    }
}