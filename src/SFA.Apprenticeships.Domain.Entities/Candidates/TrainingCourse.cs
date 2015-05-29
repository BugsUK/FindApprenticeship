namespace SFA.Apprenticeships.Domain.Entities.Candidates
{
    using System;

    public class TrainingCourse
    {
        public string Provider { get; set; }
        public string Title { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}