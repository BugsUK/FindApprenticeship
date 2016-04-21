using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SFA.Apprenticeships.Web.Manage.ViewModels
{
    using Domain.Entities.Raa.Vacancies;

    public class ReportVacancyExtensionsParameters
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public VacancyStatus  Status { get; set; }

        public int? UKPRN { get; set; }
    }
}