namespace SFA.Apprenticeships.Data.Migrate.Faa.Entities.Sql
{
    public class Person
    {
        public int PersonId { get; set; }
	    public int Title { get; set; }
        public string OtherTitle { get; set; }
        public string FirstName { get; set; }
        public string MiddleNames { get; set; }
        public string Surname { get; set; }
        public string LandlineNumber { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public int PersonTypeId { get; set; }
    }
}