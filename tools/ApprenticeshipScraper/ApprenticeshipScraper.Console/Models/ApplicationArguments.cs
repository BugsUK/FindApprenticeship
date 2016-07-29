namespace ApprenticeshipScraper.CmdLine.Models
{
    using System;

    public sealed class ApplicationArguments
    {
        public ApplicationArguments()
        {
            this.StartTime = DateTime.Now;
        }

        public SiteEnum Site { get; set; }
        public string Directory { get; set; }

        public bool Force { get; set; }

        public DateTime StartTime { get; }
    }
}