﻿namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy
{
    using Domain.Entities.Raa.Vacancies;

    public class VacancyLinksViewModel
    {
        public const string PartialView = "DisplayTemplates/_VacancyLinks";

        public VacancyLinksViewModel(long vacancyReferenceNumber, VacancyStatus status)
        {
            VacancyReferenceNumber = vacancyReferenceNumber;
            Status = status;
        }

        public long VacancyReferenceNumber { get; private set; }
        public VacancyStatus Status { get; private set; }
    }
}