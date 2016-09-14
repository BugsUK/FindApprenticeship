namespace SFA.Apprenticeships.Domain.Entities.ReferenceData
{
    public class ReleaseNote
    {
        public DasApplication Application { get; set; }

        public int Version { get; set; }

        public string Note { get; set; }
    }
}