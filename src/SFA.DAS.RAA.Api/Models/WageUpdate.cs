namespace SFA.DAS.RAA.Api.Models
{
    using Apprenticeships.Domain.Entities.Vacancies;
    using FluentValidation.Attributes;
    using Newtonsoft.Json;
    using Validators;

    [Validator(typeof(WageUpdateValidator))]
    public class WageUpdate
    {
        public WageType Type { get; set; }

        public decimal? Amount { get; set; }

        public decimal? AmountLowerBound { get; set; }

        public decimal? AmountUpperBound { get; set; }

        public WageUnit Unit { get; set; }

        [JsonIgnore]
        public decimal? ExistingAmount { get; set; }
    }
}