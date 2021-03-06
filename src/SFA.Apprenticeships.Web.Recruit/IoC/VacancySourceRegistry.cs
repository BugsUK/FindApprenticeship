﻿namespace SFA.Apprenticeships.Web.Recruit.IoC
{
    using Application.Application;
    using Application.ReferenceData;
    using Application.Vacancies;
    using Application.Vacancy;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Entities.Vacancies.Traineeships;
    using Infrastructure.Raa;
    using StructureMap.Configuration.DSL;

    public class VacancySourceRegistry : Registry
    {
        public VacancySourceRegistry()
        {
            // In Web.Recruit we only use Raa implementation (no need to read or write anything from AVMS).
            // Therefor we can just inject Raa dependencies

            For<IVacancyIndexDataProvider>().Use<VacancyIndexDataProvider>();

            For<IVacancyDataProvider<ApprenticeshipVacancyDetail>>()
                .Use<ApprenticeshipVacancyDataProvider>();

            For<IVacancyDataProvider<TraineeshipVacancyDetail>>()
                .Use<TraineeshipVacancyDataProvider>();

            For<IReferenceDataProvider>()
                .Use<ReferenceDataProvider>();
        }
    }
}