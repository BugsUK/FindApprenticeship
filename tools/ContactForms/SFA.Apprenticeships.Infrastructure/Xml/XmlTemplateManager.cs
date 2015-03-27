
namespace SFA.Apprenticeships.Infrastructure.Xml
{
    using System;
    using System.Xml.Linq;
    using Application.Interfaces.Communications;

    public class XmlTemplateManager
    {
        public static XElement GetTemplate(MessageTypes messageType)
        {
            XElement xElementTemplate;
            switch (messageType)
            {
                case MessageTypes.EmployerEnquiry:
                case MessageTypes.GlaEmployerEnquiry:
                    //todo: there is a spelling mistake in word EmployerBroadsytemRecord although as CRM cant make changes on their side before given deadline. 
                    //So this will need to be fixed in next release.
                    xElementTemplate = new XElement("EmployerBroadsytemRecord",
                                                new XElement("PrimaryContactName", new XElement("Title", CommunicationTokens.Title),
                                                                                   new XElement("Forename", CommunicationTokens.Firstname),
                                                                                   new XElement("Surname", CommunicationTokens.Lastname),
                                                                                   new XElement("Position", CommunicationTokens.Position)),
                                                new XElement("PrimaryContactEmailAddress", CommunicationTokens.Email),
                                                new XElement("PrimaryContactLandlineTelephoneNumber", CommunicationTokens.WorkPhoneNumber),
                                                new XElement("PrimaryContactFaxnumber", string.Empty),
                                                new XElement("PrimaryContactMobileTelephoneNumber", CommunicationTokens.MobileNumber),
                                                new XElement("Question", CommunicationTokens.EnquiryDescription),
                                                new XElement("EmployerName", CommunicationTokens.Companyname),
                                                new XElement("AddressDetails", new XElement("AddressLine1", CommunicationTokens.Address1),
                                                                                   new XElement("AddressLine2", CommunicationTokens.Address2),
                                                                                   new XElement("AddressLine3", CommunicationTokens.Address3),
                                                                                   new XElement("PostCode", CommunicationTokens.Postcode),
                                                                                   new XElement("County", string.Empty),
                                                                                   new XElement("Town", CommunicationTokens.City),
                                                                                   new XElement("LscRegionDetails", new XElement("FullName", "CRM to Validate"))),
                                                new XElement("NumberOfEmployees", CommunicationTokens.EmployeesCount),
                                                new XElement("Sector", CommunicationTokens.WorkSector),
                                                new XElement("PreviousExperience", CommunicationTokens.PreviousExperienceType),
                                                new XElement("HowDidYOuhearaboutWebsite", CommunicationTokens.EnquirySource));
                    break;
                case MessageTypes.WebAccessRequest:
                    //todo: Update with correct template once received from WILL @ Panlogic
                    xElementTemplate = new XElement("ApprenticeshipVacanciesRecord",
                                                new XElement("UserType", CommunicationTokens.UserType),
                                                new XElement("PrimaryContactName", new XElement("Title", CommunicationTokens.Title),
                                                                                   new XElement("Firstname", CommunicationTokens.Firstname),
                                                                                   new XElement("Lastname", CommunicationTokens.Lastname),
                                                                                   new XElement("Position", CommunicationTokens.Position)),
                                                new XElement("WorkPhoneNumber", CommunicationTokens.WorkPhoneNumber),
                                                new XElement("MobilePhoneNumber", CommunicationTokens.MobileNumber),
                                                new XElement("EmailAddress", CommunicationTokens.Email),
                                                new XElement("ConfirmEmailAddress", CommunicationTokens.Email),
                                                new XElement("OrganisationName", CommunicationTokens.Companyname),
                                                new XElement("AddressDetails", new XElement("CompanyName", CommunicationTokens.Companyname), 
                                                                                   new XElement("AddressLine1", CommunicationTokens.Address1),
                                                                                   new XElement("AddressLine2", CommunicationTokens.Address2),
                                                                                   new XElement("AddressLine3", CommunicationTokens.Address3),
                                                                                   new XElement("Town", CommunicationTokens.City),
                                                                                   new XElement("County", string.Empty),
                                                                                   new XElement("PostCode", CommunicationTokens.Postcode)),
                                                new XElement("VacancyEnquiry", CommunicationTokens.HasApprenticeshipVacancies),
                                                new XElement("SystemDetails", new XElement("ContactName", CommunicationTokens.Contactname),
                                                                                   new XElement("ContactPhoneNumber", CommunicationTokens.AdditionalPhoneNumber),
                                                                                   new XElement("ContactEmailAddress", CommunicationTokens.AdditionalEmail),
                                                                                   new XElement("SystemName", CommunicationTokens.Systemname))); 
                    break;
                default:
                    throw new ArgumentOutOfRangeException(string.Format("no XML template found for messageType {0}", messageType));
            }
            return xElementTemplate;
        }
    }
}