namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Vacancy.Entities
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("dbo.VacancyTextField")]
    public class VacancyTextField
    {
        public int VacancyTextFieldId { get; set; }

        public int VacancyId { get; set; }

        public int Field { get; set; }

        public string Value { get; set; }
    }
}
