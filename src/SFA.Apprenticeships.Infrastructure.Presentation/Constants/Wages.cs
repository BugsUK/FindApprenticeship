namespace SFA.Apprenticeships.Infrastructure.Presentation.Constants
{
    public static class Wages
    {
        public const decimal ApprenticeMinimumWage = 3.30m;
        public const decimal Under18NationalMinimumWage = 3.87m;
        public const decimal Between18And20NationalMinimumWage = 5.30m;
        public const decimal Over21NationalMinimumWage = 6.70m;
    }

    //TODO: This is the simplest possible approach based on the fact that Ovais is refactoring much of this code
    public static class WagesAfter01102016
    {
        public const decimal ApprenticeMinimumWage = 3.40m;
        public const decimal Under18NationalMinimumWage = 4.00m;
        public const decimal Between18And20NationalMinimumWage = 5.55m;
        public const decimal Over21NationalMinimumWage = 6.95m;
    }
}