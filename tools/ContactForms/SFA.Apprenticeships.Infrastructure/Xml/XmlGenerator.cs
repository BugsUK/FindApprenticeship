namespace SFA.Apprenticeships.Infrastructure.Xml
{
    using Domain.Exceptions;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Linq;
    using Application.Interfaces;
    using Application.Interfaces.Communications;
    using Common;
    using Common.Extensions;

    public class XmlGenerator : IXmlGenerator
    {
        public string SerializeToXmlFile(MessageTypes messageTypes, IEnumerable<CommunicationToken> tokens)
        {
            //Create expected XML structure first
            var xElement = XmlTemplateManager.GetTemplate(messageTypes);

            //Populate values in XML 
            foreach (var communicationToken in tokens)
            {
                SetXmlElementValueForCommunicationToken(communicationToken, xElement);
            }

            //Generate xml file in temp folder and return the filename with path
            return GetTempXmlFileName(xElement);
        }

        private string GetTempXmlFileName(XElement xElement)
        {
            string xmlAttachmentName = Path.Combine(Path.GetTempPath(), string.Format("{0}_{1}.xml", Constants.EmployerEnquiryXmlFilePrefix, Guid.NewGuid()));
            xElement.Save(xmlAttachmentName);
            return xmlAttachmentName;
        }

        private void SetXmlElementValueForCommunicationToken(CommunicationToken communicationToken, XElement xElement)
        {
            if (communicationToken == null)
            {
                throw new CustomException(string.Format("CommunicationToken object is null"));
            }
            if (xElement == null)
            {
                throw new CustomException(string.Format("XElement object is null"));
            }

            var tokenKey = communicationToken.Key;
            var tokenValue = communicationToken.Value.ToFirstCharToUpper();
            
            try
            {
                string xElementName;
                switch (tokenKey)
                {
                    case CommunicationTokens.Title:
                        xElementName = "Title";
                        xElement.Element("PrimaryContactName").Element(xElementName).Value = tokenValue;
                        break;
                    case CommunicationTokens.Firstname:
                        xElementName = "Forename";
                        xElement.Element("PrimaryContactName").Element(xElementName).Value = tokenValue;
                        break;
                    case CommunicationTokens.Lastname:
                        xElementName = "Surname";
                        xElement.Element("PrimaryContactName").Element(xElementName).Value = tokenValue;
                        break;
                    case CommunicationTokens.WorkPhoneNumber:
                        xElementName = "PrimaryContactLandlineTelephoneNumber";
                        xElement.Element(xElementName).Value = tokenValue;
                        break;
                    case CommunicationTokens.Email:
                        xElementName = "PrimaryContactEmailAddress";
                        xElement.Element(xElementName).Value = tokenValue;
                        break;
                    case CommunicationTokens.MobileNumber:
                        xElementName = "PrimaryContactMobileTelephoneNumber";
                        xElement.Element(xElementName).Value = tokenValue;
                        break;
                    case CommunicationTokens.Position:
                        xElementName = "Position";
                        xElement.Element("PrimaryContactName").Element(xElementName).Value = tokenValue;
                        break;
                    case CommunicationTokens.Address1:
                        xElementName = "AddressLine1";
                        xElement.Element("AddressDetails")
                                .Element(xElementName).Value = tokenValue;
                        break;
                    case CommunicationTokens.Address2:
                        xElementName = "AddressLine2";
                        xElement.Element("AddressDetails")
                                .Element(xElementName).Value = tokenValue;
                        break;
                    case CommunicationTokens.Address3:
                        xElementName = "AddressLine3";
                        xElement.Element("AddressDetails")
                                .Element(xElementName).Value = tokenValue;
                        break;
                    case CommunicationTokens.Postcode:
                        xElementName = "PostCode";
                        xElement.Element("AddressDetails")
                                .Element(xElementName).Value = tokenValue;
                        break;
                    case CommunicationTokens.City:
                        xElementName = "Town";
                        xElement.Element("AddressDetails")
                                .Element(xElementName).Value = tokenValue;
                        break;
                    case CommunicationTokens.Companyname:
                        xElementName = "EmployerName";
                        xElement.Element(xElementName).Value = tokenValue;
                        break;
                    #region Employer Enquiry Specific tokens start

                    case CommunicationTokens.WorkSector:
                        xElementName = "Sector";
                        xElement.Element(xElementName).Value = tokenValue;
                        break;
                    case CommunicationTokens.EmployeesCount:
                        xElementName = "NumberOfEmployees";
                        xElement.Element(xElementName).Value = tokenValue;
                        break;
                    case CommunicationTokens.EnquiryDescription:
                        xElementName = "Question";
                        xElement.Element(xElementName).Value = tokenValue;
                        break;
                    case CommunicationTokens.EnquirySource:
                        xElementName = "HowDidYOuhearaboutWebsite";
                        xElement.Element(xElementName).Value = tokenValue;
                        break;
                    case CommunicationTokens.PreviousExperienceType:
                        xElementName = "PreviousExperience";
                        xElement.Element(xElementName).Value = tokenValue;
                        break;
                    #endregion

                    default:
                        throw new ArgumentOutOfRangeException(string.Format("key: {0} not found", tokenKey));
                }
            }
            catch (Exception ex)
            {
                //todo: Log that mentioned no matching tokenTemplate avaialble but carry on to add other template tokens.
                //_logger.Log..
                //throw;
            }
        }
    }
}