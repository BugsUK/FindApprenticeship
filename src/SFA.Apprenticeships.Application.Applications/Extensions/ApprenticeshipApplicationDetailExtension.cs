namespace SFA.Apprenticeships.Application.Applications.Extensions
{
    using Domain.Entities.Applications;
    using Domain.Interfaces.Repositories;
    using Entities;

    public static class ApprenticeshipApplicationDetailExtension
    {
        public static bool UpdateApprenticeshipApplicationDetail(
            this ApprenticeshipApplicationDetail apprenticeshipApplication,
            ApplicationStatusSummary applicationStatusSummary,
            IApprenticeshipApplicationReadRepository apprenticeshipApplicationReadRepository,
            IApprenticeshipApplicationWriteRepository apprenticeshipApplicationWriteRepository)
        {
            var updated = false;

            if (applicationStatusSummary.IsLegacySystemUpdate())
            {
                // Only update application status etc. if update originated from Legacy system.
                if (apprenticeshipApplication.Status != applicationStatusSummary.ApplicationStatus)
                {
                    var ignoreOwnershipCheck = applicationStatusSummary.UpdateSource == ApplicationStatusSummary.Source.Raa;
                    updated = apprenticeshipApplicationWriteRepository.UpdateApplicationStatus(apprenticeshipApplication, applicationStatusSummary.ApplicationStatus, ignoreOwnershipCheck);

                    if (updated)
                    {

                        //Ensure passed in entity is up to date with any changes
                        var updatedApplication = apprenticeshipApplicationReadRepository.Get(apprenticeshipApplication.EntityId);
                        apprenticeshipApplication.Status = updatedApplication.Status;
                        apprenticeshipApplication.IsArchived = updatedApplication.IsArchived;
                        apprenticeshipApplication.DateUpdated = updatedApplication.DateUpdated;
                        apprenticeshipApplication.SuccessfulDateTime = updatedApplication.SuccessfulDateTime;
                        apprenticeshipApplication.UnsuccessfulDateTime = updatedApplication.UnsuccessfulDateTime;
                    }
                }

                if (apprenticeshipApplication.LegacyApplicationId != applicationStatusSummary.LegacyApplicationId)
                {
                    // Ensure the application is linked to the legacy application.
                    apprenticeshipApplication.LegacyApplicationId = applicationStatusSummary.LegacyApplicationId;
                    updated = true;
                }

                if (apprenticeshipApplication.UnsuccessfulReason != applicationStatusSummary.UnsuccessfulReason && apprenticeshipApplication.Status == ApplicationStatuses.Unsuccessful)
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