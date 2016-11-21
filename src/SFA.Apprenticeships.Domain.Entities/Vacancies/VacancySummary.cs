namespace SFA.Apprenticeships.Domain.Entities.Vacancies
{
    using Locations;
    using System;

    public abstract class VacancySummary
    {
        protected VacancySummary()
        {

        }

        protected VacancySummary(VacancySummary vacancySummary)
        {
            Id = vacancySummary.Id;
            VacancyReference = vacancySummary.VacancyReference;
            Title = vacancySummary.Title;
            PostedDate = vacancySummary.PostedDate;
            StartDate = vacancySummary.StartDate;
            ClosingDate = vacancySummary.ClosingDate;
            Description = vacancySummary.Description;
            NumberOfPositions = vacancySummary.NumberOfPositions;
            EmployerName = vacancySummary.EmployerName;
            ProviderName = vacancySummary.ProviderName;
            IsPositiveAboutDisability = vacancySummary.IsPositiveAboutDisability;
            IsEmployerAnonymous = vacancySummary.IsEmployerAnonymous;
            Location = vacancySummary.Location;
            Category = vacancySummary.Category;
            CategoryCode = vacancySummary.CategoryCode;
            SubCategory = vacancySummary.SubCategory;
            SubCategoryCode = vacancySummary.SubCategoryCode;
        }

        public int Id { get; set; }

        public string VacancyReference { get; set; }

        public string Title { get; set; }

        public DateTime PostedDate { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime ClosingDate { get; set; }

        public string Description { get; set; }

        public int? NumberOfPositions { get; set; }

        public string EmployerName { get; set; }

        public string ProviderName { get; set; }

        public bool IsPositiveAboutDisability { get; set; }

        public bool IsEmployerAnonymous { get; set; }

        public GeoPoint Location { get; set; }

        public string Category { get; set; }

        public string CategoryCode { get; set; }

        public string SubCategory { get; set; }

        public string SubCategoryCode { get; set; }

    }
}
