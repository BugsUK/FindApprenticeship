namespace SFA.DAS.RAA.Api.Models
{
    public class EmployerIdentifier
    {
        public EmployerIdentifier(int? id, int? edsUrn)
        {
            Id = id;
            EdsUrn = edsUrn;
        }

        public int? Id { get; set; }
        public int? EdsUrn { get; set; }
    }
}