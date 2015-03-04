﻿namespace SFA.Apprenticeships.Web.Candidate.ViewModels.VacancySearch
{
    using System;
    using System.Globalization;
    using Locations;

    public abstract class VacancySummaryViewModel
    {
        public int Id { get; set; }

        public SavedVacancyViewModelStatuses Status { get; set; }

        public string Title { get; set; }

        public string EmployerName { get; set; }

        public DateTime ClosingDate { get; set; }

        public string Description { get; set; }

        public GeoPointViewModel Location { get; set; }

        public double Distance { get; set; }

        public double Score { get; set; }

        public DateTime StartDate { get; set; }

        public string DistanceAsString
        {
            get
            {
                return Math.Round(Distance, 1, MidpointRounding.AwayFromZero).ToString(CultureInfo.InvariantCulture);
            }
        }
    }
}