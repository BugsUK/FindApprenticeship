namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Views.Shared
{
    public class ValidationSummaryViewModel
    {
        public int ErrorIfZero { get; set; }

        public string ErrorIfNull { get; set; }

        public int WarningIfZero { get; set; }
         
        public string WarningIfNull { get; set; } 
    }
}