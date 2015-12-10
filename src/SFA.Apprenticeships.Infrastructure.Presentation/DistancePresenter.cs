namespace SFA.Apprenticeships.Infrastructure.Presentation
{
    using System;
    using System.Globalization;

    public static class DistancePresenter
    {
        public static string ToDistanceInMilesDisplayText(this double distance)
        {
            var roundedDistance = Math.Round(distance, 1, MidpointRounding.AwayFromZero).ToString(CultureInfo.InvariantCulture);
            return $"{roundedDistance} miles";
        }
    }
}