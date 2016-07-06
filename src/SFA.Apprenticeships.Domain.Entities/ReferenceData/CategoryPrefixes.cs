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
            if (string.IsNullOrEmpty(code)) return code;
            return code.StartsWith(SectorSubjectAreaTier1) ? code : $"{SectorSubjectAreaTier1}{code}";
        }

        public static string GetFrameworkCode(string code)
        {
            if (string.IsNullOrEmpty(code)) return code;
            return code.StartsWith(Framework) ? code : $"{Framework}{code}";
        }

        public static string GetStandardSectorCode(string code)
        {
            if (string.IsNullOrEmpty(code)) return code;
            return code.StartsWith(StandardSector) ? code : $"{StandardSector}{code}";
        }

        public static string GetStandardSectorCode(int standardSectorId)
        {
            return $"{StandardSector}{standardSectorId}";
        }

        public static string GetStandardCode(int standardId)
        {
            return $"{Standard}{standardId}";
        }

        public static string GetSectorCode(string code)
        {
            if (string.IsNullOrEmpty(code)) return code;
            return code.StartsWith(Sector) ? code : $"{Sector}{code}";
        }

        public static bool IsSectorSubjectAreaTier1Code(string code)
        {
            if (string.IsNullOrEmpty(code)) return false;
            return code.StartsWith(SectorSubjectAreaTier1);
        }

        public static bool IsFrameworkCode(string code)
        {
            if (string.IsNullOrEmpty(code)) return false;
            return code.StartsWith(Framework);
        }

        public static bool IsStandardSectorCode(string code)
        {
            if (string.IsNullOrEmpty(code)) return false;
            return code.StartsWith(StandardSector);
        }

        public static string GetOriginalSectorSubjectAreaTier1Code(string code)
        {
            return code?.Replace(SectorSubjectAreaTier1, "");
        }

        public static string GetOriginalFrameworkCode(string code)
        {
            return code?.Replace(Framework, "");
        }

        public static string GetOriginalStandardSectorCode(string code)
        {
            return code?.Replace(StandardSector, "");
        }

        public static int GetOriginalStandardSectorCode(int standardSectorId)
        {
            return standardSectorId;
        }

        public static int GetOriginalStandardCode(int standardId)
        {
            return standardId;
        }

        public static string GetOriginalSectorCode(string code)
        {
            return code?.Replace(Sector, "");
        }
    }
}