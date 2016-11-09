using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Admin
{
    public class SectorViewModel
    {
        public int SectorId { get; set; }

        public string Name { get; set; }

        public int ApprenticeshipOccupationId { get; set; }

        public IEnumerable<SelectListItem> ApprenticeshipOccupations { get; set; }
    }
}
