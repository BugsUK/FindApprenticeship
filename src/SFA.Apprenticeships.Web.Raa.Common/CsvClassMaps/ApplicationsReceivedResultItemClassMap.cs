namespace SFA.Apprenticeships.Web.Raa.Common.CsvClassMaps
{
    using CsvHelper.Configuration;
    using Domain.Entities.Raa.Reporting;

    public sealed class ApplicationsReceivedResultItemClassMap : CsvClassMap<ApplicationsReceivedResultItem>
    {
        public ApplicationsReceivedResultItemClassMap()
        {
            Map(m => m.CandidateName).Name("Candidate_Name");
            Map(m => m.ApplicantId).Name("APPLICANT ID");
            Map(m => m.AddressLine1).Name("Address_Line1");
            Map(m => m.AddressLine2).Name("Address_Line2");
            Map(m => m.AddressLine3).Name("Address_Line3");
            Map(m => m.AddressLine4).Name("Address_Line4");
            //Map(m => m.AddressLine5).Name("Address_Line5");
            Map(m => m.Town).Name("Town");
            Map(m => m.County).Name("County");
            Map(m => m.Postcode).Name("Postcode");
            //Map(m => m.ShortAddress).Name("Gender");
            Map(m => m.CandidateTelephone).Name("Telephone");
            Map(m => m.Email).Name("Email");
            Map(m => m.School).Name("School");
            Map(m => m.DateOfBirth).Name("Date_of_Birth");
            Map(m => m.EthnicOrigin).Name("Ethnic_Origin");
            Map(m => m.Gender).Name("Gender");
            Map(m => m.Sector).Name("Sector");
            Map(m => m.Framework).Name("Framework");
            Map(m => m.FrameworkStatus).Name("Framework_Status");
            Map(m => m.Employer).Name("Employer");
            Map(m => m.VacancyPostcode).Name("Vacancy_Postcode");
            Map(m => m.TrainingProvider).Name("Learning_Provider");
            Map(m => m.ApplicationDate).Name("Application_Date");
            Map(m => m.ApplicationStatus).Name("Application_Status");
            Map(m => m.AllocatedTo).Name("Allocated_To");
            Map(m => m.ApplicationClosingDate).Name("Vacancy_Closing_Date");
            //Map(m => m.VacancyID).Name("Gender");
        }
    }
}