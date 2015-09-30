namespace SFA.Apprenticeships.Web.Recruit.ViewModels.Frameworks
{
    using System.Collections.Generic;

    public class SectorSelectItemViewModel
    {
        public string CodeName { get; set; }

        public string FullName { get; set; }

        public List<FrameworkSelectItemViewModel> Frameworks { get; set; } 
    }
}
