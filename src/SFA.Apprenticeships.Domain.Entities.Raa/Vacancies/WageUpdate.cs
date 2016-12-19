namespace SFA.Apprenticeships.Domain.Entities.Raa.Vacancies
{
    using System;
    using Entities.Vacancies;
    using FluentValidation.Attributes;
    using Newtonsoft.Json;
    using Validation;

    [Validator(typeof(WageUpdateValidator))]
    public class WageUpdate
    {
        public WageType? Type { get; set; }

        public decimal? Amount { get; set; }

        public decimal? AmountLowerBound { get; set; }

        public decimal? AmountUpperBound { get; set; }

        public WageUnit? Unit { get; set; }

        [JsonIgnore]
        public Wage ExistingWage { get; set; }

        [JsonIgnore]
        public DateTime? PossibleStartDate { get; set; }
    }
}