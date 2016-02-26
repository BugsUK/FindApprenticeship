namespace SFA.Apprenticeships.Web.Candidate.Factory
{
    using Common.ViewModels.MyApplications;
    using Constants;

    public class MyApplicationRoutesFactory
    {
        public static MyApplicationRoutes GetMyApplicationRoutes()
        {
            var myApplicationRoutes = new MyApplicationRoutes
            {
                ApprenticeshipSearch = CandidateRouteNames.ApprenticeshipSearch,
                ApprenticeshipView = CandidateRouteNames.ApprenticeshipView,
                ApprenticeshipArchive = CandidateRouteNames.ApprenticeshipArchive,
                NextSteps = CandidateRouteNames.NextSteps,
                ApprenticeshipApply = CandidateRouteNames.ApprenticeshipApply,
                ApprenticeshipDelete = CandidateRouteNames.ApprenticeshipDelete,
                TraineeshipView = CandidateRouteNames.TraineeshipView
            };

            return myApplicationRoutes;
        }
    }
}