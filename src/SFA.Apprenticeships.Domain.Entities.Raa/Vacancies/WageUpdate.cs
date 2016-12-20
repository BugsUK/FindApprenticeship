namespace SFA.Apprenticeships.Domain.Entities.Raa.Vacancies
{
    using System;
    using Entities.Vacancies;
    using FluentValidation.Attributes;
    using Newtonsoft.Json;
    using Validation;

    /// <summary>
    /// Defines the changes to be made to a vacancies wage
    /// </summary>
    [Validator(typeof(WageUpdateValidator))]
    public class WageUpdate
    {
        /// <summary>
        /// The new type of the wage
        /// </summary>
        public WageType? Type { get; set; }

        /// <summary>
        /// The new amount of the wage. Only required for a Custom wage tpe and must be higher than the existing amount
        /// </summary>
        public decimal? Amount { get; set; }

        /// <summary>
        /// The new lower amount of the wage range. Only required for a CustomRange wage tpe and must be higher than the existing lower amount
        /// </summary>
        public decimal? AmountLowerBound { get; set; }

        /// <summary>
        /// The new upper amount of the wage range. Only required for a CustomRange wage tpe and must be higher than the lower amount
        /// </summary>
        public decimal? AmountUpperBound { get; set; }

        /// <summary>
        /// The freqency the wage is quoted in
        /// </summary>
        public WageUnit? Unit { get; set; }

        [JsonIgnore]
        public Wage ExistingWage { get; set; }

        [JsonIgnore]
        public DateTime? PossibleStartDate { get; set; }
    }
}