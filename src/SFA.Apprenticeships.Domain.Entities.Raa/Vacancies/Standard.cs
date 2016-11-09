namespace SFA.Apprenticeships.Domain.Entities.Raa.Vacancies
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Reference.Standard")]
    public class Standard
    {
        public int StandardId { get; set; }

        public int ApprenticeshipSectorId { get; set; }

        public string Name { get; set; }

        public ApprenticeshipLevel ApprenticeshipLevel { get; set; }
        public int LarsCode { get; set; }
    }
}