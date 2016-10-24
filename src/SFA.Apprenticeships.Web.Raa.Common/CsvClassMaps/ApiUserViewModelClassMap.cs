namespace SFA.Apprenticeships.Web.Raa.Common.CsvClassMaps
{
    using CsvHelper.Configuration;
    using ViewModels.Api;

    public sealed class ApiUserViewModelClassMap : CsvClassMap<ApiUserViewModel>
    {
        public ApiUserViewModelClassMap()
        {
            Map(m => m.ExternalSystemId).Name("Username");
            Map(m => m.CompanyId).Name("Company Id");
            Map(m => m.CompanyName).Name("Company Name");
            Map(m => m.BusinessCategory).Name("Company Type");
            Map(m => m.AuthorisedApiEndpoints).Name("Authorised API Endpoints");
            Map(m => m.ContactName).Name("Contact Name");
            Map(m => m.ContactEmail).Name("Contact Email");
            Map(m => m.ContactNumber).Name("Contact Number");
        }
    }
}