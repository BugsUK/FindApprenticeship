namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy
{
    using Domain.Entities.ReferenceData;

    public class EditCategoryViewModel
    {
        public int Id { get; set; }
        public string SsatName { get; set; }
        public string FullName { get; set; }
        public CategoryStatus Status { get; set; }
    }
}