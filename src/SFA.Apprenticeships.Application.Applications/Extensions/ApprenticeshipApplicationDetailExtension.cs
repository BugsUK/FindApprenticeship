namespace SFA.Apprenticeships.Application.Applications.Extensions
{
    using System;
    using Domain.Entities.Applications;
    using Domain.Interfaces.Repositories;
    using Entities;

    internal static class ApprenticeshipApplicationDetailExtension
    {
        internal static bool UpdateApprenticeshipApplicationDetail(this ApprenticeshipApplicationDetail apprenticeshipApplication, ApplicationStatusSummary applicationStatusSummary, IApprenticeshipApplicationWriteRepository apprenticeshipApplicationWriteRepository)
        {
            var updated = false;

            if (applicationStatusSummary.IsLegacySystemUpdate())
            {
                // Only update application status etc. if update originated from Legacy system.
                if (apprenticeshipApplication.Status != applicationStatusSummary.ApplicationStatus)
                {
                    var originalApplicationStatus = apprenticeshipApplication.Status;

                    apprenticeshipApplication.Status = applicationStatusSummary.ApplicationStatus;

                    var ignoreOwnershipCheck = applicationStatusSummary.UpdateSource == ApplicationStatusSummary.Source.Raa;
                    apprenticeshipApplication = apprenticeshipApplicationWriteRepository.UpdateApplicationStatus(apprenticeshipApplication, ignoreOwnershipCheck);

                    if (originalApplicationStatus != apprenticeshipApplication.Status)
                    {
                        // Application status has changed, ensure it appears on the candidate's dashboard.
                        apprenticeshipApplication.IsArchived = false;
                        updated = true;
                    }
                }

                if (apprenticeshipApplication.LegacyApplicationId != applicationStatusSummary.LegacyApplicationId)
                {
                    // Ensure the application is linked to the legacy application.
                    apprenticeshipApplication.LegacyApplicationId = applicationStatusSummary.LegacyApplicationId;
                    updated = true;
                }

                if (apprenticeshipApplication.UnsuccessfulReason != applicationStatusSummary.UnsuccessfulReason)
                {
                    apprenticeshipApplication.UnsuccessfulReason = applicationStatusSummary.UnsuccessfulReason;
                    updated = true;
                }
            }

            if (apprenticeshipApplication.VacancyStatus != applicationStatusSummary.VacancyStatus)
            {
                apprenticeshipApplication.VacancyStatus = applicationStatusSummary.VacancyStatus;
                updated = true;
            }

            if (apprenticeshipApplication.Vacancy.ClosingDate != applicationStatusSummary.ClosingDate)
            {
                apprenticeshipApplication.Vacancy.ClosingDate = applicationStatusSummary.ClosingDate;
                updated = true;
            }

            return updated;
        }
    }
}