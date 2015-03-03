namespace SFA.Apprenticeships.Application.Services.CommunicationService
{
    using System;
    using System.Text;
    using Common.AppSettings;
    using Common.Extensions;
    using Domain.Entities;
    using Interfaces.Communications;

    public class CommunciationService : ICommunciationService
    {
        private readonly IEmailDispatcher _emailDispatcher;
        public CommunciationService(IEmailDispatcher emailDispatcher)
        {
            _emailDispatcher = emailDispatcher;
        }

        public void SubmitEnquiry(EmployerEnquiry enquiryData)
        {
            string toEmailAddress = BaseAppSettingValues.ToEmailAddress;
            string fromEmailAddress = enquiryData.Email;
            string fromName = enquiryData.Firstname;
            bool isDemoModeEnabled = BaseAppSettingValues.IsDemoModeEnabled;

            EmailRequest emailRequest = new EmailRequest
            {
                FromEmail = fromEmailAddress,
                FromName = fromName,
                ToEmail = isDemoModeEnabled ? enquiryData.Email : toEmailAddress, //todo: temporary set to driven by flag IsDemoModeEnabled for testing purposes
                Subject = string.Format("Enquiry from applicant '{0} {1} {2}' at {3} on {4}", enquiryData.Title, enquiryData.Firstname.ToFirstCharToUpper(), enquiryData.Lastname.ToFirstCharToUpper(), DateTime.Now.ToString("hh:mm tt"), DateTime.Now.ToString("dd-MMM-yyyy"))
            };

            #region Build the email content
            //todo: replace this with some html template file
            var builder = new StringBuilder();
            builder.AppendLine(string.Format("Full name : {0} {1} {2}",enquiryData.Title, enquiryData.Firstname.ToFirstCharToUpper(), enquiryData.Lastname.ToFirstCharToUpper()));
            builder.AppendLine(string.Format("Email : {0}", enquiryData.Email));
            builder.AppendLine(string.Format("Position at company : {0}", enquiryData.Position.ToFirstCharToUpper()));
            builder.AppendLine(string.Format("Phone number : {0}", enquiryData.WorkPhoneNumber));
            if (!string.IsNullOrEmpty(enquiryData.MobileNumber))
            {
                builder.AppendLine(string.Format("Mobile : {0}", enquiryData.MobileNumber));
            }

            builder.AppendLine(string.Format("Company name : {0}", enquiryData.Companyname.ToFirstCharToUpper()));
            builder.AppendLine(string.Format("Address: line 1 - {0}", enquiryData.ApplicantAddress.AddressLine1));

            if (!string.IsNullOrEmpty(enquiryData.ApplicantAddress.AddressLine2))
            {
                builder.AppendLine(string.Format("Address : line 2 - {0}", enquiryData.ApplicantAddress.AddressLine2));
            }
            if (!string.IsNullOrEmpty(enquiryData.ApplicantAddress.AddressLine3))
            {
                builder.AppendLine(string.Format("Address : line 3 - {0}", enquiryData.ApplicantAddress.AddressLine3));
            }
            if (!string.IsNullOrEmpty(enquiryData.ApplicantAddress.City))
            {
                builder.AppendLine(string.Format("Address : city - {0}", enquiryData.ApplicantAddress.City.ToFirstCharToUpper()));
            }

            builder.AppendLine(string.Format("Address : postcode - {0}", enquiryData.ApplicantAddress.Postcode));
            builder.AppendLine(string.Format("Total no of employees : {0}", enquiryData.EmployeesCount));
            builder.AppendLine(string.Format("Industry sector : {0}", enquiryData.WorkSector));
            builder.AppendLine(string.Format("Previous experience with Apprenticeships/Traineeships? : {0}", enquiryData.PreviousExperienceType));
            builder.AppendLine(string.Format("What prompted to make enquiry : {0}", enquiryData.EnquirySource));
            builder.AppendLine(string.Format("Nature of query : {0}", enquiryData.EnquiryDescription.ToFirstCharToUpper()));
            #endregion

            emailRequest.EmailContent = builder.ToString();

            _emailDispatcher.SendEmail(emailRequest);
        }
    }
}