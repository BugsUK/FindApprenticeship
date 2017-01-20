namespace SFA.Apprenticeships.Web.Candidate.ViewModels.VacancySearch
{
    using System;
    using System.Globalization;
    using Common.ViewModels.Locations;

    public abstract class VacancySummaryViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string EmployerName { get; set; }

        public bool IsPositiveAboutDisability { get; set; }

        public bool IsEmployerAnonymous { get; set; }

        public DateTime ClosingDate { get; set; }

        public string Description { get; set; }

        public int NumberOfPositions { get; set; }

        public GeoPointViewModel Location { get; set; }

        public double Distance { get; set; }

        public double Score { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime PostedDate { get; set; }

        public string SubCategory { get; set; }

        public string DistanceAsString
        {
            get
            {
                return Math.Round(Distance, 1, MidpointRounding.AwayFromZero).ToString(CultureInfo.InvariantCulture);
            }
        }

        public string GoogleStaticMapsUrl { get; set; }
    }
}