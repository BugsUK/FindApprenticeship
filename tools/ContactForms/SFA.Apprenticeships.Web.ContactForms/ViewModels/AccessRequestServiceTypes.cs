namespace SFA.Apprenticeships.Web.ContactForms.ViewModels
{
    public class AccessRequestServiceTypes
    {
        public string Id { get; set; }           // Integer value of a checkbox
        public string Description { get; set; }      // String Description of a checkbox
        public object Tags { get; set; }      // Object of html tags to be applied to checkbox, e.g.: 'new { tagName = "tagValue" }'
        public bool IsSelected { get; set; }  // Boolean value to select a checkbox on the list 
    }
}