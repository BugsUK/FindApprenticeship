using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.Apprenticeships.Application.Vacancies.Entities
{
    public class VacancyEligibleForClosure
    {
        public Guid EntityId { get; private set; }

        private VacancyEligibleForClosure()
        {
        }

        public VacancyEligibleForClosure(Guid entityId)
        {
            EntityId = entityId;
        }
    }
}
