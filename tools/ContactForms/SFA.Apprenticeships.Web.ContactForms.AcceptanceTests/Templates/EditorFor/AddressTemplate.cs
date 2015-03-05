namespace SFA.Apprenticeships.Web.ContactForms.AcceptanceTests.Templates.EditorFor
{
    using OpenQA.Selenium;
    using SpecBind.Pages;
    using SpecBind.Selenium;

    [ElementLocator(Id = "address-details")]
    public class AddressTemplate : WebElement
    {
        public AddressTemplate(ISearchContext searchContext)
            : base(searchContext)
        {
        }

        [ElementLocator(Id = "Address_AddressLine1")]
        public IWebElement AddressLine1 { get; set; }

        [ElementLocator(Id = "Address_AddressLine2")]
        public IWebElement AddressLine2 { get; set; }

        [ElementLocator(Id = "Address_AddressLine3")]
        public IWebElement AddressLine3 { get; set; }

        [ElementLocator(Id = "Address_City")]
        public IWebElement City { get; set; }

        [ElementLocator(Id = "Address_Postcode")]
        public IWebElement Postcode { get; set; }

        [ElementLocator(Id = "Address_Latitude")]
        public IWebElement Latitude { get; set; }

        [ElementLocator(Id = "Address_Longitude")]
        public IWebElement Longitude { get; set; }

    }
}
