﻿namespace SFA.Apprenticeships.Domain.Entities.Vacancies
{
    using System;
    using Locations;

    public abstract class VacancySummary
    {
        protected VacancySummary()
        {
            
        }

        protected VacancySummary(VacancySummary vacancySummary)
        {
            Id = vacancySummary.Id;
            VacancyReference = vacancySummary.VacancyReference;
            Title = vacancySummary.Title;
            //todo: 1.9: PostedDate = vacancySummary.PostedDate;
            StartDate = vacancySummary.StartDate;
            ClosingDate = vacancySummary.ClosingDate;
            Description = vacancySummary.Description;
            EmployerName = vacancySummary.EmployerName;
            Location = vacancySummary.Location;
            Sector = vacancySummary.Sector;
            SectorCode = vacancySummary.SectorCode;
            Framework = vacancySummary.Framework;
            FrameworkCode = vacancySummary.FrameworkCode;
        }

        public int Id { get; set; }

        public string VacancyReference { get; set; }

        public string Title { get; set; }

        //todo: 1.9: public DateTime PostedDate { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime ClosingDate { get; set; }

        public string Description { get; set; }

        public string EmployerName { get; set; }

        public GeoPoint Location { get; set; }

        public string Sector { get; set; }

        public string SectorCode { get; set; }

        public string Framework { get; set; }

        public string FrameworkCode { get; set; }
    }
}
