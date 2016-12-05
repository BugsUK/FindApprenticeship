namespace SFA.Apprenticeships.Infrastructure.Raa.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Domain.Entities.Raa.Parties;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.ReferenceData;
    using Domain.Entities.Vacancies;
    using ApprenticeshipLevel = Domain.Entities.Vacancies.ApprenticeshipLevel;
    using VacancySummary = Domain.Entities.Raa.Vacancies.VacancySummary;
    using VacancyType = Domain.Entities.Raa.Vacancies.VacancyType;
    using TrainingType = Domain.Entities.Raa.Vacancies.TrainingType;

    //TODO: This is only used by FAA for conversions - move to that project when found
    public static class VacancyExtensions
    {
        private const char CodeDivider = '|';

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

        public static string GetContactInformation(this Vacancy vacancy, ProviderSite providerSite)
        {
            var sb = new StringBuilder();

            if (!vacancy.EditedInRaa)
            {
                sb.Append(providerSite.ContactDetailsForCandidate);
            }
            else
            {
                if (!string.IsNullOrEmpty(vacancy.ContactName))
                {
                    sb.Append(vacancy.ContactName);
                }
                if (!string.IsNullOrEmpty(vacancy.ContactNumber))
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(" ");
                    }
                    sb.Append(vacancy.ContactNumber);
                }
                if (!string.IsNullOrEmpty(vacancy.ContactEmail))
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(" ");
                    }
                    sb.Append(vacancy.ContactEmail);
                }
            }

            return sb.ToString();
        }

        public static Domain.Entities.Vacancies.TrainingType GetTrainingType(this TrainingType trainingType)
        {
            switch (trainingType)
            {
                case TrainingType.Unknown:
                    return Domain.Entities.Vacancies.TrainingType.Unknown;
                case TrainingType.Frameworks:
                    return Domain.Entities.Vacancies.TrainingType.Frameworks;
                case TrainingType.Standards:
                    return Domain.Entities.Vacancies.TrainingType.Standards;
                case TrainingType.Sectors:
                    return Domain.Entities.Vacancies.TrainingType.Sectors;
                default:
                    throw new ArgumentException("Training Type: " + trainingType + " was not recognized");
            }
        }

        public static VacancyStatuses GetVacancyStatuses(this VacancyStatus vacancyStatuses)
        {
            switch (vacancyStatuses)
            {
                case VacancyStatus.Unknown:
                    return VacancyStatuses.Unknown;
                case VacancyStatus.Draft:
                    return VacancyStatuses.Unavailable;
                case VacancyStatus.Live:
                    return VacancyStatuses.Live;
                case VacancyStatus.Referred:
                    return VacancyStatuses.Unavailable;
                case VacancyStatus.Deleted:
                    return VacancyStatuses.Expired;
                case VacancyStatus.Submitted:
                    return VacancyStatuses.Unavailable;
                case VacancyStatus.Closed:
                    return VacancyStatuses.Expired;
                case VacancyStatus.Withdrawn:
                    return VacancyStatuses.Expired;
                case VacancyStatus.Completed:
                    return VacancyStatuses.Expired;
                case VacancyStatus.PostedInError:
                    return VacancyStatuses.Expired;
                case VacancyStatus.ReservedForQA:
                    return VacancyStatuses.Unavailable;
                default:
                    throw new ArgumentException("Provider Vacancy Status: " + vacancyStatuses + " was not recognized");
            }
        }

        public static Category GetCategory(this VacancySummary vacancy, IList<Category> categories)
        {
            switch (vacancy.TrainingType)
            {
                case TrainingType.Unknown: //Unknown assumes framework as this is a result of migrated data
                case TrainingType.Frameworks:
                    return GetFrameworkCategory(vacancy, categories);

                case TrainingType.Standards:
                    return GetStandardCategory(vacancy, categories);

                case TrainingType.Sectors:
                    return GetSectorCategory(vacancy, categories);

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static Category GetFrameworkCategory(VacancySummary vacancy, IList<Category> categories)
        {
            if (vacancy.VacancyType == VacancyType.Apprenticeship)
            {
                if (!string.IsNullOrWhiteSpace(vacancy.FrameworkCodeName))
                {
                    var frameworkCode = CategoryPrefixes.GetFrameworkCode(vacancy.FrameworkCodeName);

                    var subCategories = categories
                        .Where(c => c.SubCategories != null)
                        .SelectMany(c => c.SubCategories);

                    var framework = subCategories
                        .SingleOrDefault(c => c.CodeName.Split(CodeDivider).Contains(frameworkCode));

                    if (framework != null)
                    {
                        var category = categories.Single(sc => sc.CodeName == framework.ParentCategoryCodeName);
                        return new Category(category.Id, category.CodeName, category.FullName, category.CategoryType, category.Status);
                    }
                }

                return Category.UnknownSectorSubjectAreaTier1;
            }

            return Category.InvalidSectorSubjectAreaTier1;
        }

        private static Category GetStandardCategory(VacancySummary vacancy, IList<Category> categories)
        {
            if (vacancy.VacancyType == VacancyType.Apprenticeship)
            {
                if (vacancy.StandardId.HasValue)
                {
                    var standardCode = CategoryPrefixes.GetStandardCode(vacancy.StandardId.Value);

                    var subCategories = categories
                        .Where(c => c.SubCategories != null)
                        .SelectMany(c => c.SubCategories)
                        .ToList();

                    var standards = subCategories
                        .Where(c => c.SubCategories != null && c.SubCategories.Any())
                        .SelectMany(c => c.SubCategories);

                    var standard = standards
                        .SingleOrDefault(c => c.CodeName.Split(CodeDivider).Contains(standardCode));

                    if (standard != null)
                    {
                        var standardSector = subCategories.Single(sc => sc.CodeName == standard.ParentCategoryCodeName);
                        var category = categories.Single(sc => sc.CodeName == standardSector.ParentCategoryCodeName);
                        return new Category(category.Id, category.CodeName, category.FullName, category.CategoryType, category.Status);
                    }
                }

                return Category.UnknownSectorSubjectAreaTier1;
            }

            return Category.InvalidSectorSubjectAreaTier1;
        }

        private static Category GetSectorCategory(this VacancySummary vacancy, IList<Category> categories)
        {
            if (vacancy.VacancyType == VacancyType.Traineeship)
            {
                if (!string.IsNullOrWhiteSpace(vacancy.SectorCodeName))
                {
                    var code = CategoryPrefixes.GetSectorSubjectAreaTier1Code(vacancy.SectorCodeName);

                    var category = categories
                        .SingleOrDefault(c => c.CodeName.Split(CodeDivider).Contains(code));

                    if (category != null)
                    {
                        return new Category(category.Id, category.CodeName, category.FullName, category.CategoryType, category.Status);
                    }
                }

                return Category.UnknownSectorSubjectAreaTier1;
            }

            return Category.InvalidSectorSubjectAreaTier1;
        }

        public static Category GetSubCategory(this VacancySummary vacancy, IList<Category> categories)
        {
            switch (vacancy.TrainingType)
            {
                case TrainingType.Unknown: //Unknown assumes framework as this is a result of migrated data
                case TrainingType.Frameworks:
                    return GetFrameworkSubCategory(vacancy, categories);

                case TrainingType.Standards:
                    return GetStandardSubCategory(vacancy, categories);

                case TrainingType.Sectors:
                    return Category.InvalidSector;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static Category GetFrameworkSubCategory(VacancySummary vacancy, IList<Category> categories)
        {
            if (vacancy.VacancyType == VacancyType.Apprenticeship)
            {
                if (!string.IsNullOrWhiteSpace(vacancy.FrameworkCodeName))
                {
                    var frameworkCode = CategoryPrefixes.GetFrameworkCode(vacancy.FrameworkCodeName);

                    var subCategories = categories
                        .Where(c => c.SubCategories != null)
                        .SelectMany(c => c.SubCategories);

                    var framework = subCategories
                        .SingleOrDefault(c => c.CodeName.Split(CodeDivider).Contains(frameworkCode));

                    if (framework != null)
                    {
                        return new Category(framework.Id, framework.CodeName, framework.FullName, framework.CategoryType, framework.Status);
                    }
                }

                return Category.UnknownFramework;
            }

            return Category.InvalidFramework;
        }

        private static Category GetStandardSubCategory(VacancySummary vacancy, IList<Category> categories)
        {
            if (vacancy.VacancyType == VacancyType.Apprenticeship)
            {
                if (vacancy.StandardId.HasValue)
                {
                    var standardCode = CategoryPrefixes.GetStandardCode(vacancy.StandardId.Value);

                    var subCategories = categories
                        .Where(c => c.SubCategories != null)
                        .SelectMany(c => c.SubCategories)
                        .ToList();

                    var standards = subCategories
                        .Where(c => c.SubCategories != null && c.SubCategories.Any())
                        .SelectMany(c => c.SubCategories);

                    var standard = standards
                        .SingleOrDefault(c => c.CodeName.Split(CodeDivider).Contains(standardCode));
                    
                    if (standard != null)
                    {
                        var standardSector = subCategories.Single(sc => sc.CodeName == standard.ParentCategoryCodeName);
                        return new Category(standard.Id, standard.ParentCategoryCodeName, $"{standardSector.FullName} > {standard.FullName}", CategoryType.StandardSector, CategoryStatus.Active);
                    }
                }

                return Category.UnknownStandardSector;
            }

            return Category.InvalidStandardSector;
        }
    }
}
