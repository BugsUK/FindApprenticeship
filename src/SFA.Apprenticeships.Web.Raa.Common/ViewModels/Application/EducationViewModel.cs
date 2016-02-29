namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Application
{
    public class EducationViewModel
    {
        public const string PartialView = "Application/Education";

        public string Institution { get; set; }
        public int FromYear { get; set; }
        public int ToYear { get; set; }
    }
}