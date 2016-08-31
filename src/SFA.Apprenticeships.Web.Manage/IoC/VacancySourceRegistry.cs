namespace SFA.Apprenticeships.Web.Manage.IoC
{
    using Application.Application;
    using Application.Candidate.Strategies.Apprenticeships;
    using Application.Candidate.Strategies.Traineeships;
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
            // In Web.Manage we only use Raa implementation (no need to read or write anything from AVMS).
            // Therefor we can just inject Raa dependencies
            // TODO: same contents than the same file in Web.Recruit (unless the first two lines). Consider using a common class? 

            For<IGetCandidateApprenticeshipApplicationsStrategy>().Use<GetCandidateApprenticeshipApplicationsStrategy>();
            For<IGetCandidateTraineeshipApplicationsStrategy>().Use<GetCandidateTraineeshipApplicationsStrategy>();

            For<IVacancyIndexDataProvider>().Use<VacancyIndexDataProvider>();

            For<IVacancyDataProvider<ApprenticeshipVacancyDetail>>()
                .Use<ApprenticeshipVacancyDataProvider>();

            For<IVacancyDataProvider<TraineeshipVacancyDetail>>()
                .Use<TraineeshipVacancyDataProvider>();

            For<ILegacyApplicationStatusesProvider>()
                .Use<NullApplicationStatusesProvider>();

            For<IReferenceDataProvider>()
                .Use<ReferenceDataProvider>();
        }
    }
}