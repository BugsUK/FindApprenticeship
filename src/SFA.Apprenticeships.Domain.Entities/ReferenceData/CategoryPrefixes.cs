namespace SFA.Apprenticeships.Domain.Entities.ReferenceData
{
    public class CategoryPrefixes
    {
        public const string SectorSubjectAreaTier1 = "SSAT1.";
        public const string Framework = "FW.";
        public const string StandardSector = "STDSEC.";
        public const string Standard = "STD.";
        public const string Sector = "SEC.";

        public static string GetSectorSubjectAreaTier1Code(string code)
        {
            return code.StartsWith(SectorSubjectAreaTier1) ? code : $"{SectorSubjectAreaTier1}{code}";
        }

        public static string GetFrameworkCode(string code)
        {
            return code.StartsWith(Framework) ? code : $"{Framework}{code}";
        }

        public static string GetStandardSectorCode(string code)
        {
            return code.StartsWith(StandardSector) ? code : $"{StandardSector}{code}";
        }

        public static string GetStandardCode(int standardId)
        {
            return $"{Standard}{standardId}";
        }

        public static string GetSectorCode(string code)
        {
            return code.StartsWith(Sector) ? code : $"{Sector}{code}";
        }
    }
}