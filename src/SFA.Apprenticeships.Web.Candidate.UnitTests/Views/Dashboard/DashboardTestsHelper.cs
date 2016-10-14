namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Views.Dashboard
{
    using Candidate.ViewModels.MyApplications;
    using Domain.Entities.Applications;
    using System;
    using System.Collections.Generic;

    public static class DashboardTestsHelper
    {
        public static List<MyApprenticeshipApplicationViewModel> GetApprenticeships(int count,
    ApplicationStatuses applicationStatus = ApplicationStatuses.Draft, string unsuccessfulReason = null)
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

                if (unsuccessfulReason != null)
                {
                    myApprenticeshipApplicationViewModel.UnsuccessfulReason = unsuccessfulReason;
                    myApprenticeshipApplicationViewModel.UnsuccessfulDateTime = DateTime.Now;
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