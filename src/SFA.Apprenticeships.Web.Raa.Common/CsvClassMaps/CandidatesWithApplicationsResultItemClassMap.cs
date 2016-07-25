namespace SFA.Apprenticeships.Web.Raa.Common.CsvClassMaps
{
    using CsvHelper.Configuration;
    using Domain.Entities.Raa.Reporting;

    public class CandidatesWithApplicationsResultItemClassMap : CsvClassMap<CandidatesWithApplicationsResultItem>
    {
        public CandidatesWithApplicationsResultItemClassMap()
        {
            Map(m => m.Name).Name("Candidate_Name");
            Map(m => m.addressLine1).Name("Candidate_Address_Line1");
            Map(m => m.addressLine2).Name("Candidate_Address_Line2");
            Map(m => m.addressLine3).Name("Candidate_Address__Line3");
            Map(m => m.addressLine4).Name("Candidate_Address_Line4");
            Map(m => m.addressLine5).Name("Candidate_Address_Line5");
            Map(m => m.town).Name("Candidate_Town");
            Map(m => m.CandidateRegion).Name("Candidate_County");
            Map(m => m.Postcode).Name("Candidate_Postcode");
            Map(m => m.DateofBirth).Name("Date_Of_Birth");
            Map(m => m.Ethnicity).Name("Ethnicity");
            Map(m => m.Disability).Name("Disability");
            Map(m => m.Gender).Name("Gender");
            Map(m => m.Email).Name("Candidate_Email");
            Map(m => m.LandlineNumber).Name("Candidate_Tel_No");
            Map(m => m.MobileNumber).Name("Candidate_Mobile_No");
            Map(m => m.SchoolName).Name("School_Name");
            Map(m => m.SchoolAddress1).Name("School_Address1");
            Map(m => m.SchoolAddress2).Name("School_Address2");
            Map(m => m.SchoolArea).Name("School_Area");
            Map(m => m.SchoolTown).Name("School_Town");
            Map(m => m.SchoolCounty).Name("School_County");
            Map(m => m.SchoolPostcode).Name("School_Postcode");
            Map(m => m.DateRegistered).Name("Date_Registered");
            Map(m => m.DateLastLoggedOn).Name("Date_Last_Logged_On");
            Map(m => m.EmployerName).Name("Employer_Name");
            Map(m => m.empAddressLine1).Name("Employer_Address_Line1");
            Map(m => m.empAddressLine2).Name("Employer_Address_Line2");
            Map(m => m.empAddressLine3).Name("Employer_Address_Line3");
            Map(m => m.empAddressLine4).Name("Employer_Address_Line4");
            Map(m => m.empAddressLine5).Name("Employer_Address_Line5");
            Map(m => m.empTown).Name("Employer_Town");
            Map(m => m.empCounty).Name("Employer_County");
            Map(m => m.empPostCode).Name("Employer_Postcode");
            Map(m => m.VacancyTitle).Name("Vacancy_Title");
            Map(m => m.VancacyType).Name("Vancacy_Type");
            Map(m => m.VacancyCategory).Name("Vacancy_Category");
            Map(m => m.VacancyStatus).Name("Vacancy_Status");
            Map(m => m.VacancyReferenceNumber).Name("Vacancy_Reference_Number");
            Map(m => m.LearningProvider).Name("Learning_Provider");
            Map(m => m.ApplicationStatus).Name("Application_Status");
            Map(m => m.NumberOfDaysApplicationAtThisStatus).Name("Number_Of_Days_App_At_This_Status");
            Map(m => m.ApplicationHistoryEventDate).Name("Date_Application_Made");
            Map(m => m.ApplicationStatusSetDate).Name("Application_Status_Set_Date");
            Map(m => m.VacancyClosingDate).Name("Vacancy_Closing_Date");
        }
    }
}