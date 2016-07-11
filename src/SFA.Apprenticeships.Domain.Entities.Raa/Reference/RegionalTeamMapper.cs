namespace SFA.Apprenticeships.Domain.Entities.Raa.Reference
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public class RegionalTeamMapper
    {
        private static readonly Regex PostcodeAreaRegex = new Regex(@"^([A-Za-z]{1,2})\d+");

        private static readonly IDictionary<string, RegionalTeam> Map = new Dictionary<string, RegionalTeam>
        {
            {"DH", RegionalTeam.North},
            {"DL", RegionalTeam.North},
            {"HG", RegionalTeam.North},
            {"NE", RegionalTeam.North},
            {"SR", RegionalTeam.North},
            {"TS", RegionalTeam.North},
            {"YO", RegionalTeam.North},

            {"BB", RegionalTeam.NorthWest},
            {"BL", RegionalTeam.NorthWest},
            {"CA", RegionalTeam.NorthWest},
            {"CW", RegionalTeam.NorthWest},
            {"FY", RegionalTeam.NorthWest},
            {"L", RegionalTeam.NorthWest},
            {"LA", RegionalTeam.NorthWest},
            {"M", RegionalTeam.NorthWest},
            {"OL", RegionalTeam.NorthWest},
            {"PR", RegionalTeam.NorthWest},
            {"SK", RegionalTeam.NorthWest},
            {"WA", RegionalTeam.NorthWest},
            {"WN", RegionalTeam.NorthWest},
            {"CH", RegionalTeam.NorthWest},

            {"BD", RegionalTeam.YorkshireAndHumberside},
            {"DN", RegionalTeam.YorkshireAndHumberside},
            {"HD", RegionalTeam.YorkshireAndHumberside},
            {"HU", RegionalTeam.YorkshireAndHumberside},
            {"HX", RegionalTeam.YorkshireAndHumberside},
            {"LN", RegionalTeam.YorkshireAndHumberside},
            {"LS", RegionalTeam.YorkshireAndHumberside},
            {"S", RegionalTeam.YorkshireAndHumberside},
            {"WF", RegionalTeam.YorkshireAndHumberside},

            {"DE", RegionalTeam.EastMidlands},
            {"LE", RegionalTeam.EastMidlands},
            {"NG", RegionalTeam.EastMidlands},
            {"NN", RegionalTeam.EastMidlands},

            {"B", RegionalTeam.WestMidlands},
            {"CV", RegionalTeam.WestMidlands},
            {"DY", RegionalTeam.WestMidlands},
            {"HR", RegionalTeam.WestMidlands},
            {"ST", RegionalTeam.WestMidlands},
            {"SY", RegionalTeam.WestMidlands},
            {"TF", RegionalTeam.WestMidlands},
            {"WR", RegionalTeam.WestMidlands},
            {"WS", RegionalTeam.WestMidlands},
            {"WV", RegionalTeam.WestMidlands},

            {"AL", RegionalTeam.EastAnglia},
            {"CB", RegionalTeam.EastAnglia},
            {"CM", RegionalTeam.EastAnglia},
            {"CO", RegionalTeam.EastAnglia},
            {"IG", RegionalTeam.EastAnglia},
            {"IP", RegionalTeam.EastAnglia},
            {"LU", RegionalTeam.EastAnglia},
            {"NR", RegionalTeam.EastAnglia},
            {"PE", RegionalTeam.EastAnglia},
            {"RM", RegionalTeam.EastAnglia},
            {"SG", RegionalTeam.EastAnglia},
            {"SS", RegionalTeam.EastAnglia},
            {"WD", RegionalTeam.EastAnglia},

            {"BN", RegionalTeam.SouthEast},
            {"BR", RegionalTeam.SouthEast},
            {"CR", RegionalTeam.SouthEast},
            {"CT", RegionalTeam.SouthEast},
            {"DA", RegionalTeam.SouthEast},
            {"GU", RegionalTeam.SouthEast},
            {"EN", RegionalTeam.SouthEast},
            {"HA", RegionalTeam.SouthEast},
            {"HP", RegionalTeam.SouthEast},
            {"KT", RegionalTeam.SouthEast},
            {"ME", RegionalTeam.SouthEast},
            {"MK", RegionalTeam.SouthEast},
            {"OX", RegionalTeam.SouthEast},
            {"RG", RegionalTeam.SouthEast},
            {"RH", RegionalTeam.SouthEast},
            {"SL", RegionalTeam.SouthEast},
            {"SM", RegionalTeam.SouthEast},
            {"SO", RegionalTeam.SouthEast},
            {"TN", RegionalTeam.SouthEast},
            {"TW", RegionalTeam.SouthEast},
            {"UB", RegionalTeam.SouthEast},
            {"E", RegionalTeam.SouthEast},
            {"EC", RegionalTeam.SouthEast},
            {"N", RegionalTeam.SouthEast},
            {"NW", RegionalTeam.SouthEast},
            {"SE", RegionalTeam.SouthEast},
            {"SW", RegionalTeam.SouthEast},
            {"W", RegionalTeam.SouthEast},
            {"WC", RegionalTeam.SouthEast},

            {"BA", RegionalTeam.SouthWest},
            {"BH", RegionalTeam.SouthWest},
            {"BS", RegionalTeam.SouthWest},
            {"DT", RegionalTeam.SouthWest},
            {"EX", RegionalTeam.SouthWest},
            {"GL", RegionalTeam.SouthWest},
            {"PL", RegionalTeam.SouthWest},
            {"PO", RegionalTeam.SouthWest},
            {"SN", RegionalTeam.SouthWest},
            {"SP", RegionalTeam.SouthWest},
            {"TA", RegionalTeam.SouthWest},
            {"TQ", RegionalTeam.SouthWest},
            {"TR", RegionalTeam.SouthWest}
        }; 

        public static RegionalTeam GetRegionalTeam(string postcode)
        {
            if (string.IsNullOrWhiteSpace(postcode))
            {
                return RegionalTeam.Other;
            }

            postcode = postcode.Replace(" ", "");

            var match = PostcodeAreaRegex.Match(postcode);
            if (match.Success)
            {
                var postcodeArea = match.Groups[1].Value.ToUpper();

                if (Map.ContainsKey(postcodeArea))
                {
                    return Map[postcodeArea];
                }
            }

            return RegionalTeam.Other;
        }
    }
}