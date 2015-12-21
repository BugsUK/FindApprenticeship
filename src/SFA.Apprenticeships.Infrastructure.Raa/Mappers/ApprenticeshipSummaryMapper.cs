namespace SFA.Apprenticeships.Infrastructure.Raa.Mappers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Logging;
    using Domain.Entities.Locations;
    using Domain.Entities.ReferenceData;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using Presentation;

    public class ApprenticeshipSummaryMapper
    {
        public static ApprenticeshipSummary GetApprenticeshipSummary(ApprenticeshipVacancy vacancy, IEnumerable<Category> categories, ILogService logService)
        {
            //Manually mapping rather than using automapper as the two enties are significantly different
            
            //TODO: Store geopoints for employers
            var location = vacancy.ProviderSiteEmployerLink.Employer.Address.GeoPoint ?? new GeoPoint();
            if (location.Latitude == 0 || location.Longitude == 0)
            {
                //Coventry
                location.Latitude = 52.4009991288043;
                location.Longitude = -1.50812239495425;
            }

            var summary = new ApprenticeshipSummary
            {
                Id = (int)vacancy.VacancyReferenceNumber,
                //Goes into elastic unformatted for searching
                VacancyReference = vacancy.VacancyReferenceNumber.ToString(),
                Title = vacancy.Title,
                PostedDate = vacancy.DateQAApproved ?? DateTime.MinValue,
                StartDate = vacancy.PossibleStartDate ?? DateTime.MinValue,
                ClosingDate = vacancy.ClosingDate ?? DateTime.MinValue,
                Description = vacancy.ShortDescription,
                NumberOfPositions = vacancy.NumberOfPositions,
                EmployerName = vacancy.ProviderSiteEmployerLink.Employer.Name,
                //TODO: Get provider
                //ProviderName = vacancy.ProviderSiteEmployerLink.,
                //TODO: Are we going to add this to RAA?
                //IsPositiveAboutDisability = vacancy.,
                Location = location,
                //TODO: How do we determine this in RAA?
                VacancyLocationType = ApprenticeshipLocationType.NonNational,
                ApprenticeshipLevel = vacancy.ApprenticeshipLevel.GetApprenticeshipLevel(),
                Wage = new Wage(vacancy.WageType, vacancy.Wage, vacancy.WageUnit).GetDisplayText(vacancy.HoursPerWeek),
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
    }
}