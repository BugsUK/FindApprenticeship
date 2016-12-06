namespace SFA.DAS.RAA.Api.Models
{
    using FluentValidation.Attributes;
    using Validators;

    [Validator(typeof(VacancyValidator))]
    public class Vacancy
    {
        public int VacancyReferenceNumber { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
    }
}