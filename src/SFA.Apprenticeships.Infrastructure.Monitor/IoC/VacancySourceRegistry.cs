namespace SFA.Apprenticeships.Infrastructure.Monitor.IoC
{
    using Application.Application;
    using Application.Candidate.Strategies;
    using Application.Candidate.Strategies.Apprenticeships;
    using Application.Vacancies;
    using Application.Vacancy;
    using Domain.Entities.Vacancies;
    using Domain.Entities.Vacancies.Traineeships;
    using Raa;
    using StructureMap.Configuration.DSL;

    public class VacancySourceRegistry : Registry
    {
        public VacancySourceRegistry()
        {
            // Strategies
            For<IGetCandidateVacancyDetailStrategy<ApprenticeshipVacancyDetail>>()
                    .Use<GetCandidateVacancyDetailStrategy<ApprenticeshipVacancyDetail>>();
            For<IGetCandidateVacancyDetailStrategy<TraineeshipVacancyDetail>>()
                .Use<GetCandidateVacancyDetailStrategy<TraineeshipVacancyDetail>>();

            // Application status provider -> it's not exactly related with vacancy sources...
            For<IGetCandidateApprenticeshipApplicationsStrategy>()
                    .Use<GetCandidateApprenticeshipApplicationsStrategy>();

            // Services --
            For<IVacancyIndexDataProvider>().Use<VacancyIndexDataProvider>();

            For<IVacancyDataProvider<ApprenticeshipVacancyDetail>>()
                .Use<ApprenticeshipVacancyDataProvider>();

            For<IVacancyDataProvider<TraineeshipVacancyDetail>>()
                .Use<TraineeshipVacancyDataProvider>();

            //--
        }
    }
}