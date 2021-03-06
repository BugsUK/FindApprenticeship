﻿namespace SFA.Apprenticeships.Web.Common.ViewModels
{
    using System;

    public class DateViewModel
    {
        public DateViewModel()
        {
            
        }

        public DateViewModel(DateTime? date)
        {
            if (date == null) return;
            Day = date.Value.Day;
            Month = date.Value.Month;
            Year = date.Value.Year;
        }

        public int? Day { get; set; }

        public int? Month { get; set; }

        public int? Year { get; set; }

        public bool HasValue
        {
            get
            {
                return Day.HasValue && Month.HasValue && Year.HasValue;
            }
        }

        public DateTime Date
        {
            get
            {
                // ReSharper disable once NotResolvedInText
                if (!Day.HasValue) throw new ArgumentOutOfRangeException("Day");
                // ReSharper disable once NotResolvedInText
                if (!Month.HasValue) throw new ArgumentOutOfRangeException("Month");
                // ReSharper disable once NotResolvedInText
                if (!Year.HasValue) throw new ArgumentOutOfRangeException("Year");

                return new DateTime(Year.Value, Month.Value, Day.Value);
            }
        }
    }
}