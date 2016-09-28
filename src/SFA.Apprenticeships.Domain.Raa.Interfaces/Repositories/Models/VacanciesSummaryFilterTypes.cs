using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.Apprenticeships.Domain.Raa.Interfaces.Repositories.Models
{
    public enum VacanciesSummaryFilterTypes
    {
        All = 0,
        Live = 1,
        Submitted,

        /// <summary>AKA Referred</summary>
        Rejected,

        ClosingSoon,
        Closed,
        Draft,
        NewApplications,
        Withdrawn,
        Completed
    }
}
