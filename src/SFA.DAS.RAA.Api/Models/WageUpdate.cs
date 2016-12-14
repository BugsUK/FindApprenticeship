namespace SFA.DAS.RAA.Api.Models
{
    using System;
    using Apprenticeships.Domain.Entities.Vacancies;
    public class WageUpdate
    {
        public WageType Type { get; set; }

        public decimal? Amount { get; set; }

        public decimal? AmountLowerBound { get; set; }

        public decimal? AmountUpperBound { get; set; }

        public WageUnit Unit { get; set; }
    }
}