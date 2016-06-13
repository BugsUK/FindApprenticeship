namespace SFA.Apprenticeships.Infrastructure.Raa.IoC
{
    using Application.Applications;
    using Application.ReferenceData;
    using Application.Reporting;
    using Application.Vacancies;
    using Application.Vacancy;
    using Common.Configuration;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Entities.Vacancies.Traineeships;
    using StructureMap.Configuration.DSL;

    public class RaaRegistry : Registry
    {
        public RaaRegistry(ServicesConfiguration servicesConfiguration)
        {
            if (servicesConfiguration.ServiceImplementation == ServicesConfiguration.Raa)
            {
                //For<IVacancyIndexDataProvider>().Use<VacancyIndexDataProvider>();

                //For<IVacancyDataProvider<ApprenticeshipVacancyDetail>>()
                //    .Use<ApprenticeshipVacancyDataProvider>();

                //For<IVacancyDataProvider<TraineeshipVacancyDetail>>()
                //    .Use<TraineeshipVacancyDataProvider>();

                //For<ILegacyApplicationStatusesProvider>()
                //    .Use<NullApplicationStatusesProvider>();

                For<IReferenceDataProvider>()
                    .Use<ReferenceDataProvider>();

                For<IReportingProvider>()
                    .Use<ReportingProvider>();
            }
        }
    }
}