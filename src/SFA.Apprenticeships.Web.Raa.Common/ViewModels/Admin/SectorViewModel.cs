namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Admin
{
    using System.ComponentModel.DataAnnotations;
    using Constants.ViewModels;
    using FluentValidation.Attributes;
    using Validators.Sector;
    using System.Collections.Generic;
    using System.Web.Mvc;

    [Validator(typeof(SectorViewModelClientValidator))]
    public class SectorViewModel
    {
        public int SectorId { get; set; }

        [Display(Name = SectorViewModelMessages.Name.LabelText)]
        public string Name { get; set; }

        [Display(Name = SectorViewModelMessages.ApprenticeshipOccupationId.LabelText)]
        public int ApprenticeshipOccupationId { get; set; }

        public IEnumerable<SelectListItem> ApprenticeshipOccupations { get; set; }
    }
}
