namespace SFA.Apprenticeships.Web.Manage.ViewModels.Admin
{
    using Domain.Entities.ReferenceData;

    public class FrameworkViewModel
    {
        public string SSAT1Name { get; set; }

        public int FrameworkId { get; set; }

        public string FrameworkFullName { get; set; }

        public CategoryStatus FrameworkStatus { get; set; }
    }
}