﻿namespace SFA.Apprenticeships.Domain.Entities.Vacancies
{
    public class ApprenticeshipSearchResponse : ApprenticeshipSummary
    {
        public double Distance { get; set; }

        public double Score { get; set; }
    }
}
