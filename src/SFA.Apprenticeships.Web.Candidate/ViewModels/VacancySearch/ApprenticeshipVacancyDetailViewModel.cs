﻿namespace SFA.Apprenticeships.Web.Candidate.ViewModels.VacancySearch
{
    using System;

    [Serializable]
    public class ApprenticeshipVacancyDetailViewModel : VacancyDetailViewModel
    {
        public ApprenticeshipVacancyDetailViewModel()
        {
        }

        public ApprenticeshipVacancyDetailViewModel(string message) : base(message)
        {
        }
    }
}