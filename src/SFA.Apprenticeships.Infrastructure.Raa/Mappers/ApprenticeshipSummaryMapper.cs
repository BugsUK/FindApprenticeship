namespace SFA.Apprenticeships.Infrastructure.Raa.Mappers
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Extensions;
    using SFA.Infrastructure.Interfaces;
    using Domain.Entities.Locations;
    using Domain.Entities.Raa.Parties;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.ReferenceData;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Extensions;
    using Presentation;

    using SFA.Apprenticeships.Application.Interfaces;

    public class ApprenticeshipSummaryMapper
    {
        public static ApprenticeshipSummary GetApprenticeshipSummary(VacancySummary vacancy, Employer employer, Provider provider, IList<Category> categories, ILogService logService)
        {
            try
            {
                //Manually mapping rather than using automapper as the two enties are significantly different

                var location = GetGeoPoint(vacancy);

                var wage = new Wage(vacancy.WageType, vacancy.Wage, vacancy.WageText, vacancy.WageUnit);

                var category = vacancy.GetCategory(categories);
                var subcategory = vacancy.GetSubCategory(categories);
                LogCategoryAndSubCategory(vacancy, logService, category, subcategory);

                var summary = new ApprenticeshipSummary
                {
                    Id = vacancy.VacancyId,
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
                    EmployerName = string.IsNullOrEmpty(vacancy.EmployerAnonymousName) ? employer.Name : string.Empty,
                    ProviderName = provider.Name,
                    IsPositiveAboutDisability = employer.IsPositiveAboutDisability,
                    Location = location,
                    VacancyLocationType = vacancy.VacancyLocationType == VacancyLocationType.Nationwide ? ApprenticeshipLocationType.National : ApprenticeshipLocationType.NonNational,
                    ApprenticeshipLevel = vacancy.ApprenticeshipLevel.GetApprenticeshipLevel(),
                    Wage = wage.GetDisplayText(vacancy.HoursPerWeek),
                    WageUnit = wage.GetWageUnit(),
                    WorkingWeek = vacancy.WorkingWeek,
                    CategoryCode = category.CodeName,
                    Category = category.FullName,
                    SubCategoryCode = subcategory.CodeName,
                    SubCategory = subcategory.FullName
                };

                return summary;
            }
            catch (Exception ex)
            {
                logService.Error($"Failed to map apprenticeship with Id: {vacancy?.VacancyId ?? 0}", ex);
                return null;
            }
        }

        private static GeoPoint GetGeoPoint(VacancySummary vacancy)
        {
            return new GeoPoint
            {
                Latitude = vacancy.Address.GeoPoint.Latitude,
                Longitude = vacancy.Address.GeoPoint.Longitude
            };
        }

        private static void LogCategoryAndSubCategory(VacancySummary vacancy, ILogService logService, Category category, Category subcategory)
        {
            if (!category.IsValid())
            {
                logService.Warn("Cannot find a category for the apprenticeship with Id {0}", vacancy.VacancyId);
            }
            if (!subcategory.IsValid())
            {
                logService.Warn("Cannot find a category for the apprenticeship with Id {0}", vacancy.VacancyId);
            }
        }
    }
}