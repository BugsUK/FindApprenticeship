namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Application
{
    public class QualificationViewModel
    {
        public const string PartialView = "Application/Qualification";

        public string QualificationType { get; set; }
        public string Subject { get; set; }
        public string Grade { get; set; }
        public bool IsPredicted { get; set; }
        public int Year { get; set; }
    }
}