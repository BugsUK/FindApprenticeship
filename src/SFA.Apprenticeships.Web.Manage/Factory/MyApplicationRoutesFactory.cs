namespace SFA.Apprenticeships.Web.Manage.Factory
{
    using Common.ViewModels.MyApplications;
    using Constants;

    public class MyApplicationRoutesFactory
    {
        public static MyApplicationRoutes GetMyApplicationRoutes()
        {
            var myApplicationRoutes = new MyApplicationRoutes
            {
                //TODO fix nulls
                ApprenticeshipSearch = ManagementRouteNames.ViewCandidateApprenticeshipApplication,
                ApprenticeshipView = ManagementRouteNames.ViewCandidateApprenticeshipApplication,
                ApprenticeshipArchive = ManagementRouteNames.ViewCandidateApprenticeshipApplication,
                NextSteps = ManagementRouteNames.ViewCandidateApprenticeshipApplication,
                ApprenticeshipApply = ManagementRouteNames.ViewCandidateApprenticeshipApplication,
                ApprenticeshipDelete = ManagementRouteNames.ViewCandidateApprenticeshipApplication,
                TraineeshipView = ManagementRouteNames.ViewCandidateTraineeshipApplication
            };

            return myApplicationRoutes;
        }
    }
}