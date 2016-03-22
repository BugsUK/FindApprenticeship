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
    using Domain.Entities.Vacancies.Apprenticeships;
    using Extensions;
    using Presentation;

    public class ApprenticeshipSummaryMapper
    {
        public static ApprenticeshipSummary GetApprenticeshipSummary(VacancySummary vacancy, Employer employer, Provider provider, IEnumerable<Category> categories, ILogService logService)
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

            var wage = new Wage(vacancy.WageType, vacancy.Wage, vacancy.WageText, vacancy.WageUnit);

                var summary = new ApprenticeshipSummary
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
                    VacancyLocationType = vacancy.VacancyLocationType == VacancyLocationType.Nationwide ? ApprenticeshipLocationType.National : ApprenticeshipLocationType.NonNational,
                    ApprenticeshipLevel = vacancy.ApprenticeshipLevel.GetApprenticeshipLevel(),
                    Wage = wage.GetDisplayText(vacancy.HoursPerWeek),
                    WageUnit = wage.GetWageUnit(),
                    WorkingWeek = vacancy.WorkingWeek,
                    SubCategoryCode = vacancy.FrameworkCodeName
                };

                if (!string.IsNullOrEmpty(summary.SubCategoryCode))
                {
                    var category = categories.SingleOrDefault(c => c.SubCategories.Any(sc => sc.CodeName == summary.SubCategoryCode));
                    if (category == null)
                    {
                        summary.CategoryCode = "Unknown";
                        summary.SubCategoryCode = "Unknown";
                        logService.Warn("Cannot find category containing a subcategory matching code {1} for the vacancy with Id {0}", summary.Id, summary.SubCategoryCode);
                    }
                    else
                    {
                        summary.Category = category.FullName;
                        summary.CategoryCode = category.CodeName;

                        var subCategory = category.SubCategories.SingleOrDefault(sc => sc.CodeName == summary.SubCategoryCode);
                        if (subCategory == null)
                        {
                            summary.SubCategory = "Unknown";
                            summary.SubCategoryCode = "Unknown";
                            logService.Warn("Cannot find subcatagory matching code {1} in category {3} ({2}) for the vacancy with Id {0}", summary.Id, summary.SubCategoryCode, category.CodeName, category.FullName);
                        }
                        else
                        {
                            summary.SubCategory = subCategory.FullName;
                            summary.SubCategoryCode = subCategory.CodeName;
                        }
                    }
                }

                return summary;
            }
            catch (Exception ex)
            {
                logService.Error($"Failed to map apprenticeship with Id: {vacancy.VacancyId}", ex);
                return null;
            }
        }
    }
}