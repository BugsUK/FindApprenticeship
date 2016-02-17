namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Views.Dashboard
{
    using System;
    using System.Collections.Generic;
    using Common.ViewModels.MyApplications;
    using Domain.Entities.Applications;

    public static class DashboardTestsHelper
    {
        public static List<MyApprenticeshipApplicationViewModel> GetApprenticeships(int count,
    ApplicationStatuses applicationStatus = ApplicationStatuses.Draft)
        {
            var apprenticeships = new List<MyApprenticeshipApplicationViewModel>();

            for (var i = 0; i < count; i++)
            {
                var myApprenticeshipApplicationViewModel = new MyApprenticeshipApplicationViewModel
                {
                    ApplicationStatus = applicationStatus
                };

                if (applicationStatus == ApplicationStatuses.Submitted)
                {
                    myApprenticeshipApplicationViewModel.DateApplied = new DateTime(2015, 01, 01);
                }

                apprenticeships.Add(myApprenticeshipApplicationViewModel);
            }

            return apprenticeships;
        }

        public static List<MyTraineeshipApplicationViewModel> GetTraineeships(int count)
        {
            var traineeships = new List<MyTraineeshipApplicationViewModel>();

            for (var i = 0; i < count; i++)
            {
                traineeships.Add(new MyTraineeshipApplicationViewModel());
            }

            return traineeships;
        } 
    }
}