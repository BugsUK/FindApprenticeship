namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Admin
{
    using Domain.Entities.Raa.Vacancies;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Constants.ViewModels;

    public class StandardViewModel
    {
        public int Id { get; set; }

        [Display(Name = StandardViewModelMessages.Name.LabelText)]
        public string Name { get; set; }

        [Display(Name = StandardViewModelMessages.ApprenticeshipSectorId.LabelText)]
        public int ApprenticeshipSectorId { get; set; }

        public IEnumerable<SelectListItem> ApprenticeshipSectors { get; set; }

        [Display(Name = StandardViewModelMessages.ApprenticeshipLevel.LabelText)]
        public ApprenticeshipLevel ApprenticeshipLevel { get; set; }

        public List<SelectListItem> ApprenticeshipLevels { get; set; }
    }
}
