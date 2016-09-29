using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Vacancy
{
    public static class VacancyRepositoryResources
    {
        public static readonly string[] VacancySummaryColumns = new string[] {
            "VacancyId", "VacancyOwnerRelationshipId", "VacancyReferenceNumber", "VacancyStatusId",
            "AddressLine1", "AddressLine2", "AddressLine3", "AddressLine4", "AddressLine5", "Town", "CountyId", "PostCode", "LocalAuthorityId", "Longitude", "Latitude",
            "ApprenticeshipFrameworkId", "Title", "ApprenticeshipType", "ShortDescription", "WeeklyWage", "WageType", "WageText", "NumberofPositions",
            "ApplicationClosingDate", "InterviewsFromDate", "ExpectedStartDate", "ExpectedDuration", "WorkingWeek", "EmployerAnonymousName",
            "ApplyOutsideNAVMS", "LockedForSupportUntil", "NoOfOfflineApplicants", "MasterVacancyId", "VacancyLocationTypeId", "VacancyManagerID", "DeliveryOrganisationID", "ContractOwnerID",
            "VacancyGuid", "SubmissionCount", "StartedToQADateTime", "StandardId", "HoursPerWeek", "WageUnitId", "DurationTypeId", "DurationValue", "QAUserName",
            "TrainingTypeId", "VacancyTypeId", "SectorId", "UpdatedDateTime"
        };
}
}
