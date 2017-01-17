namespace SFA.Apprenticeships.Domain.Entities.Raa.Vacancies
{
    using System;

    public struct WageRange
    {
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public decimal ApprenticeMinimumWage { get; set; }
        public decimal Under18NationalMinimumWage { get; set; }
        public decimal Between18And20NationalMinimumWage { get; set; }
        public decimal Over21NationalMinimumWage { get; set; }

        public string ValidationErrorMessage { get; set; }
    }
}