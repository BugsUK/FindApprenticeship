namespace SFA.Apprenticeships.Infrastructure.Raa.Mappers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SFA.Infrastructure.Interfaces;
    using Domain.Entities.Locations;
    using Domain.Entities.Raa.Parties;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.ReferenceData;
    using Domain.Entities.Vacancies.Traineeships;

    public class TraineeshipSummaryMapper
    {
        public static TraineeshipSummary GetTraineeshipSummary(VacancySummary vacancy, Employer employer, Provider provider, IEnumerable<Category> categories, ILogService logService)
        {
            try
            {
                //Manually mapping rather than using automapper as the two enties are significantly different

                //TODO: Geocode new vacancies
                var location = new GeoPoint();
                if (vacancy.Address.GeoPoint != null && vacancy.Address.GeoPoint.Latitude != 0 &&
                    vacancy.Address.GeoPoint.Longitude != 0)
                {
                    location.Latitude = vacancy.Address.GeoPoint.Latitude;
                    location.Longitude = vacancy.Address.GeoPoint.Longitude;
                }
                else
                {
                    //Coventry
                    location.Latitude = 52.4009991288043;
                    location.Longitude = -1.50812239495425;
                }

                var summary = new TraineeshipSummary
                {
                    Id = (int)vacancy.VacancyReferenceNumber,
                    //Goes into elastic unformatted for searching
                    VacancyReference = vacancy.VacancyReferenceNumber.ToString(),
                    Title = vacancy.Title,
                    // ReSharper disable PossibleInvalidOperationException
                    PostedDate = vacancy.DateQAApproved.Value,
                    StartDate = vacancy.PossibleStartDate.Value,
                    ClosingDate = vacancy.ClosingDate.Value,
                    // ReSharper restore PossibleInvalidOperationException
                    Description = vacancy.ShortDescription,
                    NumberOfPositions = vacancy.NumberOfPositions,
                    EmployerName = employer.Name,
                    ProviderName = provider.Name,
                    //TODO: Are we going to add this to RAA?
                    //IsPositiveAboutDisability = vacancy.,
                    Location = location,
                    CategoryCode = vacancy.SectorCodeName,
                    SubCategoryCode = vacancy.FrameworkCodeName
                };

                if (!string.IsNullOrEmpty(summary.CategoryCode))
                {
                    var category = categories.SingleOrDefault(c => c.CodeName == summary.CategoryCode);
                    if (category == null)
                    {
                        summary.CategoryCode = "Unknown";
                        logService.Warn("Cannot find category matching code {1} for the vacancy with Id {0}", summary.Id, summary.CategoryCode);
                    }
                    else
                    {
                        summary.Category = category.FullName;
                        summary.CategoryCode = category.CodeName;
                    }
                }

                return summary;
            }
            catch (Exception ex)
            {
                logService.Error($"Failed to map traineeship with Id: {vacancy.VacancyId}", ex);
                return null;
            }
        }
    }
}