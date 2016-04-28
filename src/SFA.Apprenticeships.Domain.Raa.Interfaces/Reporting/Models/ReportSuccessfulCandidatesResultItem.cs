using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.Apprenticeships.Domain.Raa.Interfaces.Reporting.Models
{
    public class ReportSuccessfulCandidatesResultItem
    {
        public string Name { get; set; }
        public string Gender { get; set; }
        public string Postcode { get; set; }
        public string LearningProvider { get; set; }
        public string VacancyReferenceNumber { get; set; }
        public string VacancyTitle { get; set; }
        public string VacancyType { get; set; }
        public string Sector { get; set; }
        public string Framework { get; set; }
        public string FrameworkStatus { get; set; }
        public string Employer { get; set; }
        public string SuccessfulAppDate { get; set; }
        public string ILRStartDate { get; set; }
        public string ILRReference { get; set; }
    }
}
