namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Vacancy.Entities
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("dbo.AdditionalQuestion")]
    public class AdditionalQuestion
    {
        public int AdditionalQuestionId { get; set; }

        public int VacancyId { get; set; }

        public short QuestionId { get; set; }

        public string Question { get; set; }
    }
}