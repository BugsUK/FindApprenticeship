namespace SFA.Apprenticeships.Domain.Entities.ReferenceData
{
    public class ReleaseNote
    {
        public ReleaseNote(DasApplication dasApplication, int version, string note)
        {
            Application = dasApplication;
            Version = version;
            Note = note;
        }

        public DasApplication Application { get; private set; }

        public int Version { get; private set; }

        public string Note { get; private set; }
    }
}