namespace SFA.Apprenticeships.Web.Manage.CsvClassMaps
{
    using CsvHelper.Configuration;
    using Domain.Raa.Interfaces.Reporting.Models;

    public sealed class ReportUnsuccessfulCandidatesResultItemClassMap : CsvClassMap<ReportUnsuccessfulCandidatesResultItem>
    {
        public ReportUnsuccessfulCandidatesResultItemClassMap()
        {
            Map(m => m.FirstName).Name("First_Name");
            Map(m => m.MiddleName).Name("Middle_Name");
            Map(m => m.SurName).Name("Surname");
            Map(m => m.Gender).Name("Gender");
            Map(m => m.Disability).Name("Disability");
            Map(m => m.DateofBirth).Name("Date_of_Birth");
            Map(m => m.AllowMarketingMessages).Name("AllowMarketingMessages");
            Map(m => m.AddressLine1).Name("Address_Line1");
            Map(m => m.AddressLine2).Name("Address_Line2");
            Map(m => m.AddressLine3).Name("Address_Line3");
            Map(m => m.AddressLine4).Name("Address_Line4");
            Map(m => m.AddressLine5).Name("Address_Line5");
            Map(m => m.Town).Name("Town");
            Map(m => m.AuthorityArea).Name("County");
            Map(m => m.Postcode).Name("Postcode");
            Map(m => m.TelephoneNumber).Name("Telephone_Number");
            Map(m => m.MobileNumber).Name("Mobile_Number");
            Map(m => m.Email).Name("Email");
            Map(m => m.Unsuccessful).Name("Unsuccessful");
            Map(m => m.Ongoing).Name("Ongoing");
            Map(m => m.Withdrawn).Name("Withdrawn");
            Map(m => m.DateApplied).Name("Date_Applied");
            Map(m => m.VacancyClosingDate).Name("Vacancy_Closing_Date");
            Map(m => m.DateOfUnsuccessfulNotification).Name("Date_Of_Unsuccessful_Notification");
            Map(m => m.LearningProvider).Name("Learning_Provider");
            Map(m => m.LearningProviderUKPRN).Name("Learning_Provider_UKPRN");
            Map(m => m.VacancyReferenceNumber).Name("Vacancy_Reference_Number");
            Map(m => m.VacancyTitle).Name("Vacancy_Title");
            Map(m => m.VacancyLevel).Name("Vacancy_Level");
            Map(m => m.Sector).Name("Sector");
            Map(m => m.Framework).Name("Framework");
            Map(m => m.UnsuccessfulReason).Name("Unsuccessful_Reason");
            Map(m => m.Notes).Name("Notes");
        }
    }

    public sealed class ReportUnsuccessfulCandidatesWithIdsResultItemClassMap : CsvClassMap<ReportUnsuccessfulCandidatesResultItem>
    {
        public ReportUnsuccessfulCandidatesWithIdsResultItemClassMap()
        {
            Map(m => m.CandidateId).Name("Candidate_Id");
            Map(m => m.CandidateGuid).Name("Candidate_Guid");
            Map(m => m.FirstName).Name("First_Name");
            Map(m => m.MiddleName).Name("Middle_Name");
            Map(m => m.SurName).Name("Surname");
            Map(m => m.Gender).Name("Gender");
            Map(m => m.Disability).Name("Disability");
            Map(m => m.DateofBirth).Name("Date_of_Birth");
            Map(m => m.AllowMarketingMessages).Name("AllowMarketingMessages");
            Map(m => m.AddressLine1).Name("Address_Line1");
            Map(m => m.AddressLine2).Name("Address_Line2");
            Map(m => m.AddressLine3).Name("Address_Line3");
            Map(m => m.AddressLine4).Name("Address_Line4");
            Map(m => m.AddressLine5).Name("Address_Line5");
            Map(m => m.Town).Name("Town");
            Map(m => m.AuthorityArea).Name("County");
            Map(m => m.Postcode).Name("Postcode");
            Map(m => m.TelephoneNumber).Name("Telephone_Number");
            Map(m => m.MobileNumber).Name("Mobile_Number");
            Map(m => m.Email).Name("Email");
            Map(m => m.Unsuccessful).Name("Unsuccessful");
            Map(m => m.Ongoing).Name("Ongoing");
            Map(m => m.Withdrawn).Name("Withdrawn");
            Map(m => m.DateApplied).Name("Date_Applied");
            Map(m => m.VacancyClosingDate).Name("Vacancy_Closing_Date");
            Map(m => m.DateOfUnsuccessfulNotification).Name("Date_Of_Unsuccessful_Notification");
            Map(m => m.LearningProvider).Name("Learning_Provider");
            Map(m => m.LearningProviderUKPRN).Name("Learning_Provider_UKPRN");
            Map(m => m.VacancyReferenceNumber).Name("Vacancy_Reference_Number");
            Map(m => m.VacancyTitle).Name("Vacancy_Title");
            Map(m => m.VacancyLevel).Name("Vacancy_Level");
            Map(m => m.Sector).Name("Sector");
            Map(m => m.Framework).Name("Framework");
            Map(m => m.UnsuccessfulReason).Name("Unsuccessful_Reason");
            Map(m => m.Notes).Name("Notes");
        }
    }
}