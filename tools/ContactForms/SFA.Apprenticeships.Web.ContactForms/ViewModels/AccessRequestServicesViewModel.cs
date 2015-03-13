namespace SFA.Apprenticeships.Web.ContactForms.ViewModels
{
    using System.Collections.Generic;

    public class AccessRequestServicesViewModel
    {
        public IList<AccessRequestServiceTypes> AvailableServices { get; set; }
        public IList<AccessRequestServiceTypes> SelectedServices { get; set; }
        public string[] PostedServiceIds { get; set; }
    }
}