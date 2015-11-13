﻿namespace SFA.Apprenticeships.Domain.Interfaces.Queries
{
    using System;

    public class ApprenticeshipVacancyQuery
    {
        public string FrameworkCodeName { get; set; }

        public DateTime? LiveDate { get; set; }

        public int CurrentPage { get; set; }

        public int PageSize { get; set; }
    }
}