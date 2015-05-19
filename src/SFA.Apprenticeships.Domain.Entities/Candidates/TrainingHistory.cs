namespace SFA.Apprenticeships.Domain.Entities.Candidates
{
    using System;

    public class TrainingHistory
    {
        public string Provider { get; set; }
        public string CourseTitle { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}