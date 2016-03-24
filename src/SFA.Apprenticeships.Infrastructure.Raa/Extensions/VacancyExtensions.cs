namespace SFA.Apprenticeships.Infrastructure.Raa.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.ReferenceData;
    using Domain.Entities.Vacancies;
    using ApprenticeshipLevel = Domain.Entities.Vacancies.Apprenticeships.ApprenticeshipLevel;
    using VacancySummary = Domain.Entities.Raa.Vacancies.VacancySummary;
    using VacancyType = Domain.Entities.Raa.Vacancies.VacancyType;

    //TODO: This is only used by FAA for conversions - move to that project when found
    public static class VacancyExtensions
    {
        public static ApprenticeshipLevel GetApprenticeshipLevel(
            this Domain.Entities.Raa.Vacancies.ApprenticeshipLevel apprenticeshipLevel)
        {
            switch (apprenticeshipLevel)
            {
                case Domain.Entities.Raa.Vacancies.ApprenticeshipLevel.Unknown:
                    return ApprenticeshipLevel.Unknown;
                case Domain.Entities.Raa.Vacancies.ApprenticeshipLevel.Intermediate:
                    return ApprenticeshipLevel.Intermediate;
                case Domain.Entities.Raa.Vacancies.ApprenticeshipLevel.Advanced:
                    return ApprenticeshipLevel.Advanced;
                case Domain.Entities.Raa.Vacancies.ApprenticeshipLevel.Higher:
                    return ApprenticeshipLevel.Higher;
                case Domain.Entities.Raa.Vacancies.ApprenticeshipLevel.FoundationDegree:
                case Domain.Entities.Raa.Vacancies.ApprenticeshipLevel.Degree:
                case Domain.Entities.Raa.Vacancies.ApprenticeshipLevel.Masters:
                    return ApprenticeshipLevel.Degree;
                default:
                    throw new ArgumentException("Apprenticeship Level: " + apprenticeshipLevel + " was not recognized");
            }
        }

        public static LegacyWageType GetLegacyWageType(this WageType wageType)
        {
            return LegacyWageType.LegacyText;
        }

        public static VacancyStatuses GetVacancyStatuses(this VacancyStatus vacancyStatuses)
        {
            switch (vacancyStatuses)
            {
                case VacancyStatus.Unknown:
                    return VacancyStatuses.Unknown;
                case VacancyStatus.Draft:
                    return VacancyStatuses.Unavailable;
                case VacancyStatus.Submitted:
                    return VacancyStatuses.Unavailable;
                case VacancyStatus.Live:
                    return VacancyStatuses.Live;
                case VacancyStatus.ReservedForQA:
                    return VacancyStatuses.Unavailable;
                case VacancyStatus.Referred:
                    return VacancyStatuses.Unavailable;
                case VacancyStatus.Closed:
                    return VacancyStatuses.Expired;
                default:
                    throw new ArgumentException("Provider Vacancy Status: " + vacancyStatuses + " was not recognized");
            }
        }

        public static Category GetSubCategory(this VacancySummary vacancy, IEnumerable<Category> categories)
        {
            switch (vacancy.TrainingType)
            {
                case TrainingType.Frameworks:
                    return GetFrameworkSubCategory(vacancy, categories);

                case TrainingType.Standards:
                    return GetStandardSubCategory(vacancy, categories);

                case TrainingType.Sectors:
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            return null;
        }

        public static Category GetCategory(this VacancySummary vacancy, IEnumerable<Category> categories)
        {
            if (vacancy.VacancyType == VacancyType.Traineeship)
            {
                if (!string.IsNullOrWhiteSpace(vacancy.SectorCodeName))
                {
                    var code = $"SSAT1.{vacancy.SectorCodeName}";

                    var category = categories
                        .SingleOrDefault(c => c.CodeName == code);

                    if (category != null)
                    {
                        return new Category
                        {
                            CodeName = category.CodeName,
                            FullName = category.FullName
                        };
                    }
                }

                return Category.UnknownSectorSubjectAreaTier1;
            }

            return Category.InvalidSectorSubjectAreaTier1;
        }

        private static Category GetFrameworkSubCategory(VacancySummary vacancy, IEnumerable<Category> categories)
        {
            if (vacancy.VacancyType == VacancyType.Apprenticeship)
            {
                if (!string.IsNullOrWhiteSpace(vacancy.FrameworkCodeName))
                {
                    var frameworkCode = $"FW.{vacancy.FrameworkCodeName}";

                    var subCategories = categories
                        .Where(c => c.SubCategories != null)
                        .SelectMany(c => c.SubCategories);

                    var framework = subCategories
                        .SingleOrDefault(c => c.CodeName == frameworkCode);

                    if (framework != null)
                    {
                        return new Category
                        {
                            CodeName = framework.CodeName,
                            FullName = framework.FullName
                        };
                    }
                }

                return Category.UnknownFramework;
            }

            return Category.InvalidFramework;
        }

        private static Category GetStandardSubCategory(VacancySummary vacancy, IEnumerable<Category> categories)
        {
            if (vacancy.VacancyType == VacancyType.Apprenticeship)
            {
                if (vacancy.StandardId.HasValue)
                {
                    var standardCode = $"STD.{vacancy.StandardId}";

                    var subCategories = categories
                        .Where(c => c.SubCategories != null)
                        .SelectMany(c => c.SubCategories);

                    var standards = subCategories
                        .Where(c => c.SubCategories != null && c.SubCategories.Any())
                        .SelectMany(c => c.SubCategories);

                    var standard = standards
                        .SingleOrDefault(c => c.CodeName == standardCode);

                    if (standard != null)
                    {
                        return new Category
                        {
                            CodeName = standard.ParentCategoryCodeName
                        };
                    }
                }

                return Category.UnknownStandardSector;
            }

            return Category.InvalidStandardSector;
        }
    }
}
