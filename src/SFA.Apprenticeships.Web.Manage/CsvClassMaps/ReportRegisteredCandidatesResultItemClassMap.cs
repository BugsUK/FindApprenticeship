namespace SFA.Apprenticeships.Web.Manage.CsvClassMaps
{
    using CsvHelper.Configuration;
    using Domain.Raa.Interfaces.Reporting.Models;

    public class ReportRegisteredCandidatesResultItemClassMap : CsvClassMap<ReportRegisteredCandidatesResultItem>
    {
        public ReportRegisteredCandidatesResultItemClassMap()
        {
            Map(m => m.FirstName).Name("First_Name");
            Map(m => m.MiddleNames).Name("Middle_Name");
            Map(m => m.Surname).Name("Surname");
            Map(m => m.AddressLine1).Name("Address_Line1");
            Map(m => m.AddressLine2).Name("Address_Line2");
            Map(m => m.AddressLine3).Name("Address_Line3");
            Map(m => m.AddressLine4).Name("Address_Line4");
            Map(m => m.AddressLine5).Name("Address_Line5");
            Map(m => m.Town).Name("Town");
            Map(m => m.County).Name("County");
            Map(m => m.Postcode).Name("Postcode");
            Map(m => m.DateofBirth).Name("Date_of_Birth");
            Map(m => m.Gender).Name("Gender");
            Map(m => m.LandlineNumber).Name("Telephone_Number");
            Map(m => m.MobileNumber).Name("Mobile_Number");
            Map(m => m.Email).Name("Email");
            Map(m => m.Ethnicity).Name("Ethnicity");
            Map(m => m.RegistrationDate).Name("Date_Registered");
            Map(m => m.DateLastActive).Name("Date_Last_Logged_On");
            Map(m => m.LastSchool).Name("Last_School");
            Map(m => m.Region).Name("Region");
            Map(m => m.AllowMarketingMessages).Name("AllowMarketingMessages");
            Map(m => m.Sector).Name("Sector");
            Map(m => m.Framework).Name("Framework");
            Map(m => m.Keyword).Name("Keyword");
            Map(m => m.CandidateStatus).Name("Status");
            //Map(m => m.CandidateId).Name("");
            //Map(m => m.Name).Name("");
            //Map(m => m.ShortAddress).Name("");
            //Map(m => m.Address).Name("");
            //Map(m => m.ApplicationCount).Name("");
            //Map(m => m.Inactive).Name("");
            //Map(m => m.DregisteredCandidate).Name("");
        }
    }
}