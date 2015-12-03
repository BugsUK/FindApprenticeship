namespace SFA.Apprenticeships.Infrastructure.Raa
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Logging;
    using Application.ReferenceData;
    using Application.Vacancies;
    using Application.Vacancies.Entities;
    using Domain.Entities.Locations;
    using Domain.Entities.ReferenceData;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using Domain.Entities.Vacancies.Traineeships;
    using Domain.Interfaces.Repositories;
    using Presentation;

    /// <summary>
    /// TODO: This class will eventually use an RAA service for the data rather than referencing repositories directly.
    /// This service does not exist yet and so the simplest approach has been used for now
    /// </summary>
    public class VacancyIndexDataProvider : IVacancyIndexDataProvider
    {
        private readonly IApprenticeshipVacancyReadRepository _apprenticeshipVacancyReadRepository;
        private readonly IReferenceDataProvider _referenceDataProvider;
        private readonly ILogService _logService;

        public VacancyIndexDataProvider(IApprenticeshipVacancyReadRepository apprenticeshipVacancyReadRepository, IReferenceDataProvider referenceDataProvider, ILogService logService)
        {
            _apprenticeshipVacancyReadRepository = apprenticeshipVacancyReadRepository;
            _referenceDataProvider = referenceDataProvider;
            _logService = logService;
        }

        public int GetVacancyPageCount()
        {
            //We're returning all live vacancies as a single page for now to simplify things.
            //TODO: Implement robust paging as a stretch goal
            return 1;
        }

        public VacancySummaries GetVacancySummaries(int pageNumber)
        {
            var vacancies = _apprenticeshipVacancyReadRepository.GetWithStatus(ProviderVacancyStatuses.Live);
            var categories = _referenceDataProvider.GetCategories();
            var vacancySummaries = vacancies.Select(v => GetApprenticeshipSummary(v, categories));
            return new VacancySummaries(vacancySummaries, new List<TraineeshipSummary>());
        }

        private ApprenticeshipSummary GetApprenticeshipSummary(ApprenticeshipVacancy vacancy, IEnumerable<Category> categories)
        {
            //Manually mapping rather than using automapper as the two enties are significantly different
            //Note that 
            var summary = new ApprenticeshipSummary
            {
                Id = (int)vacancy.VacancyReferenceNumber,
                //TODO: Use common Vacancy Reference presenter
                VacancyReference = vacancy.VacancyReferenceNumber.ToString(),
                Title = vacancy.Title,
                PostedDate = vacancy.DateQAApproved ?? DateTime.MinValue,
                StartDate = vacancy.PossibleStartDate ?? DateTime.MinValue,
                ClosingDate = vacancy.ClosingDate ?? DateTime.MinValue,
                Description = vacancy.ShortDescription,
                //TODO: Map once Vicenc has finished with multi location work
                NumberOfPositions = 1,
                EmployerName = vacancy.ProviderSiteEmployerLink.Employer.Name,
                //TODO: Get provider
                //ProviderName = vacancy.ProviderSiteEmployerLink.,
                //TODO: Are we going to add this to RAA?
                //IsPositiveAboutDisability = vacancy.,
                //TODO: Store geopoints for employers
                //Location = vacancy.ProviderSiteEmployerLink.Employer.Address.GeoPoint,
                Location = new GeoPoint {Latitude = 52.4009991288043, Longitude = -1.50812239495425 }, //Coventry
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
                    _logService.Warn("Cannot find category containing a subcategory matching code {1} for the vacancy with Id {0}", summary.Id, summary.SubCategoryCode);
                }
                else
                {
                    summary.Category = category.FullName;
                    summary.CategoryCode = category.CodeName;

                    var subCategory = category.SubCategories.SingleOrDefault(sc => sc.CodeName == summary.SubCategoryCode);
                    if (subCategory == null)
                    {
                        _logService.Warn("Cannot find subcatagory matching code {1} in category {3} ({2}) for the vacancy with Id {0}", summary.Id, summary.SubCategoryCode, category.CodeName, category.FullName);
                    }
                    {
                        summary.CategoryCode = "Unknown";
                        summary.SubCategoryCode = "Unknown";
                    }
                }
            }

            return summary;
        }
    }
}