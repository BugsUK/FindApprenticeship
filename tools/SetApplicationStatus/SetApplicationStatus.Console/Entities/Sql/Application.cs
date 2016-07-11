namespace SetApplicationStatus.Console.Entities.Sql
{
    public class Application
    {
        public int ApplicationId { get; set; }

        public ApplicationStatus Status { get; set; }

        public int VacancyId { get; set; }
    }
}
