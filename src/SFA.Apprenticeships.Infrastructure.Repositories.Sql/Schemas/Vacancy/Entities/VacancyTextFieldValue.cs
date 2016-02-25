namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Vacancy.Entities
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("dbo.VacancyTextFieldValue")]
    public class VacancyTextFieldValue
    {
        public int VacancyTextFieldValueId { get; set; }

        public string CodeName { get; set; }

        public string ShortName { get; set; }

        public string FullName { get; set; }
    }
}