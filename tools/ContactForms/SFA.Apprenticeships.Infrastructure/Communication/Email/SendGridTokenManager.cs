namespace SFA.Apprenticeships.Infrastructure.Communication.Email
{
    using System;
    using Application.Interfaces.Communications;
    using Application.Interfaces;
using SFA.Apprenticeships.Infrastructure.Logging;

    public static class SendGridTokenManager
    {        
        const string TemplateTokenDelimiter = "-";

        public static string GetEmailTemplateTokenForCommunicationToken(CommunicationTokens key)
        {
            string emailTemplateToken;
            switch (key)
            {
                case CommunicationTokens.Title:
                    emailTemplateToken = "Title";
                    break;
                case CommunicationTokens.Firstname:
                    emailTemplateToken = "Firstname";
                    break;
                case CommunicationTokens.Lastname:
                    emailTemplateToken = "Lastname";
                    break;
                case CommunicationTokens.WorkPhoneNumber:
                    emailTemplateToken = "WorkPhoneNumber";
                    break;
                case CommunicationTokens.Email:
                    emailTemplateToken = "Email";
                    break;
                case CommunicationTokens.MobileNumber:
                    emailTemplateToken = "MobileNumber";
                    break;
                case CommunicationTokens.Position:
                    emailTemplateToken = "Position";
                    break;
                case CommunicationTokens.Address1:
                    emailTemplateToken = "Address1";
                    break;
                case CommunicationTokens.Address2:
                    emailTemplateToken = "Address2";
                    break;
                case CommunicationTokens.Address3:
                    emailTemplateToken = "Address3";
                    break;
                case CommunicationTokens.Postcode:
                    emailTemplateToken = "Postcode";
                    break;
                case CommunicationTokens.City:
                    emailTemplateToken = "City";
                    break;
                case CommunicationTokens.Companyname:
                    emailTemplateToken = "Companyname";
                    break;

                #region Employer Enquiry Specific tokens start  
                    
                case CommunicationTokens.WorkSector:
                    emailTemplateToken = "WorkSector";
                    break;
                case CommunicationTokens.EmployeesCount:
                    emailTemplateToken = "EmployeesCount";
                    break;                
                case CommunicationTokens.EnquiryDescription:
                    emailTemplateToken = "EnquiryDescription";
                    break;
                case CommunicationTokens.EnquirySource:
                    emailTemplateToken = "EnquirySource";
                    break;
                case CommunicationTokens.PreviousExperienceType:
                    emailTemplateToken = "PreviousExperienceType";
                    break; 
                #endregion

                #region Web Access Request specific tokens

                case CommunicationTokens.ContactEmail:
                    emailTemplateToken = "ContactEmail";
                    break;
                case CommunicationTokens.ContactName:
                    emailTemplateToken = "ContactName";
                    break;
                case CommunicationTokens.ContactPhoneNumber:
                    emailTemplateToken = "ContactPhoneNumber";
                    break;
                case CommunicationTokens.Services:
                    emailTemplateToken = "Services";
                    break;
                case CommunicationTokens.UserType:
                    emailTemplateToken = "UserType";
                    break;
                case CommunicationTokens.HasApprenticeshipVacancies:
                    emailTemplateToken = "HasApprenticeshipVacancies";
                    break;
                case CommunicationTokens.SystemName:
                    emailTemplateToken = "SystemName";
                    break;
                case CommunicationTokens.ServiceTypeIds:
                    emailTemplateToken = "ServiceTypeIds";
                    break;
                case CommunicationTokens.AdditionalPhoneNumber:
                    emailTemplateToken = "AdditionalPhoneNumber";
                    break;
                case CommunicationTokens.AdditionalEmail:
                    emailTemplateToken = "AdditionalEmail";
                    break;                
                #endregion

                default:
                    try
                    {
                        throw new ArgumentOutOfRangeException(string.Format("key: {0} not found", key));
                    }
                    catch (Exception ex)
                    {
                        emailTemplateToken = ex.Message;
                        //todo: Log that mentioned no matching tokenTemplate avaialble but carry on to add other template tokens.
                        //_logger.Log... 
                    }
                    break;
            }

            return string.Format("{0}{1}{0}", TemplateTokenDelimiter, emailTemplateToken);
        }
    }
}