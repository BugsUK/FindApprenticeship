﻿namespace SFA.Apprenticeships.Application.VacancyEtl.Entities
{
    using System;
    using Domain.Entities.Vacancies;

    public class VacancySummaryPage
    {
        public int PageNumber { get; set; }

        public int TotalPages { get; set; }

        public DateTime ScheduledRefreshDateTime { get; set; }
    }
}
