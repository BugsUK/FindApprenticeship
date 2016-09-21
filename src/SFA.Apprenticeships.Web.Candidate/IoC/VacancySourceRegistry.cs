namespace SFA.Apprenticeships.Web.Candidate.IoC
{
    using Application.Application;
    using Application.Candidate.Strategies;
    using Application.Candidate.Strategies.Apprenticeships;
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
            // Strategies
            For<ILegacyGetCandidateVacancyDetailStrategy<ApprenticeshipVacancyDetail>>().Use<GetCandidateVacancyDetailStrategy<ApprenticeshipVacancyDetail>>();
            For<ILegacyGetCandidateVacancyDetailStrategy<TraineeshipVacancyDetail>>().Use<GetCandidateVacancyDetailStrategy<TraineeshipVacancyDetail>>();

            // Application status provider -> it's not exactly related with vacancy sources...
            For<IGetCandidateApprenticeshipApplicationsStrategy>().Use<GetCandidateApprenticeshipApplicationsStrategy>();

            For<ILegacyApplicationStatusesProvider>()
                .Use<NullApplicationStatusesProvider>();

            // Services --
            For<IVacancyIndexDataProvider>().Use<VacancyIndexDataProvider>();

            For<IVacancyDataProvider<ApprenticeshipVacancyDetail>>()
                .Use<ApprenticeshipVacancyDataProvider>()
                .Name = "ApprenticeshipVacancyDataProvider";

            For<IVacancyDataProvider<TraineeshipVacancyDetail>>()
                .Use<TraineeshipVacancyDataProvider>()
                .Name = "TraineeshipVacancyDataProvider";
            //--
        }
    }
}