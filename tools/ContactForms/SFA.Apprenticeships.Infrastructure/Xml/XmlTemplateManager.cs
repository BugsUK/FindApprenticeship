
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
                    //todo: there is a spelling mistake in word EmployerBroadsytemRecord although as CRM cant make changes on their side before given deadline. So this will need to be fixed in next release.
                    xElementTemplate = new XElement("EmployerBroadsytemRecord",
                                                new XElement("PrimaryContactEmailAddress", CommunicationTokens.Email),
                                                new XElement("PrimaryContactLandlineTelephoneNumber", CommunicationTokens.WorkPhoneNumber),
                                                new XElement("PrimaryContactFaxnumber", string.Empty),
                                                new XElement("PrimaryContactMobileTelephoneNumber", CommunicationTokens.MobileNumber),
                                                new XElement("Question", CommunicationTokens.EnquiryDescription),
                                                new XElement("EmployerName", CommunicationTokens.Companyname),
                                                new XElement("NumberOfEmployees", CommunicationTokens.EmployeesCount),
                                                new XElement("Sector", CommunicationTokens.WorkSector),
                                                new XElement("PreviousExperience", CommunicationTokens.PreviousExperienceType),
                                                new XElement("HowDidYOuhearaboutWebsite", CommunicationTokens.EnquirySource),
                                                new XElement("PrimaryContactName", new XElement("Title", CommunicationTokens.Title),
                                                                                   new XElement("Forename", CommunicationTokens.Firstname),
                                                                                   new XElement("Surname", CommunicationTokens.Lastname),
                                                                                   new XElement("Position", CommunicationTokens.Position)),
                                                new XElement("AddressDetails", new XElement("AddressLine1", CommunicationTokens.Address1),
                                                                                   new XElement("AddressLine2", CommunicationTokens.Address2),
                                                                                   new XElement("AddressLine3", CommunicationTokens.Address3),
                                                                                   new XElement("Town", CommunicationTokens.City),
                                                                                   new XElement("County", string.Empty),
                                                                                   new XElement("PostCode", CommunicationTokens.Postcode),
                                                                                   new XElement("LscRegionDetails", new XElement("FullName", "CRM to Validate"))));
                    break;
                case MessageTypes.WebAccessRequest:
                    //todo: Update with correct template once received from WILL @ Panlogic
                    xElementTemplate = new XElement("WebServiceRequestRecord"); 
                    break;
                default:
                    throw new ArgumentOutOfRangeException(string.Format("no XML template found for messageType {0}", messageType));
            }
            return xElementTemplate;
        }
    }
}