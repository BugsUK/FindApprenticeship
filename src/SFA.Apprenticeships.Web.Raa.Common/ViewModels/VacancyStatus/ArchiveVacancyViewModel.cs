using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.VacancyStatus
{
    public class ArchiveVacancyViewModel
    {
        public ArchiveVacancyViewModel(bool hasOutstandingActions1, int vacancyId1)
        {
            HasOutstandingActions = hasOutstandingActions1;
            VacancyId = vacancyId1;
        }

        public bool HasOutstandingActions { get; private set; }

        public int VacancyId { get; private set; }
    }
}
