namespace SFA.Apprenticeships.Domain.Entities.Users
{
    public class Name
    {
        public Name(string firstName, string middleNames, string lastName)
        {
            FirstName = firstName;
            MiddleNames = middleNames;
            LastName = lastName;
        }

        public string FirstName { get; private set; }
        public string MiddleNames { get; private set; }
        public string LastName { get; private set; }
    }
}