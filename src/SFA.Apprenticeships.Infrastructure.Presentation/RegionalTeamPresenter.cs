namespace SFA.Apprenticeships.Infrastructure.Presentation
{
    using Domain.Entities.Raa.Reference;

    public static class RegionalTeamPresenter
    {
        public static string GetTitle(this RegionalTeam regionalTeam)
        {
            switch (regionalTeam)
            {
                case RegionalTeam.NorthWest:
                    return "North West";
                case RegionalTeam.YorkshireAndHumberside:
                    return "Yorkshire and Humberside";
                case RegionalTeam.EastMidlands:
                    return "East Midlands";
                case RegionalTeam.WestMidlands:
                    return "West Midlands";
                case RegionalTeam.EastAnglia:
                    return "East Anglia";
                case RegionalTeam.SouthEast:
                    return "South East";
                case RegionalTeam.SouthWest:
                    return "South West";
                default:
                    return regionalTeam.ToString();
            }
        }
    }
}