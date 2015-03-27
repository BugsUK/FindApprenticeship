using System.Text;
using System.Xml;

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
        public string SerializeToXmlFile(MessageTypes messageType, IEnumerable<CommunicationToken> tokens)
        {
            //Create expected XML structure first
            var xElement = XmlTemplateManager.GetTemplate(messageType);

            //Populate values in XML 
            foreach (var communicationToken in tokens)
            {
                SetXmlElementValueForCommunicationToken(messageType, communicationToken, xElement);
            }

            //Generate xml file in temp folder and return the filename with path
            return GetTempXmlFileName(xElement);
        }

        private string GetTempXmlFileName(XElement xElement)
        {
            string xmlAttachmentName = Path.Combine(Path.GetTempPath(), string.Format("{0}_{1}.xml", Constants.EmployerEnquiryXmlFilePrefix, Guid.NewGuid()));
            var doc = new XDocument(new XDeclaration("1.0", "utf-8", null), xElement);

            using (var writer = new XmlTextWriter(xmlAttachmentName, new UTF8Encoding(false)))
            {
                doc.Save(writer);
            }

            // read it straight in, write it straight back out. 
            //can be improved by using xDocument 
            //xElement.Save(xmlAttachmentName);
            //string[] lines = File.ReadAllLines(xmlAttachmentName);
            //File.WriteAllLines(xmlAttachmentName, lines);
            return xmlAttachmentName;
        }

        private void SetXmlElementValueForCommunicationToken(MessageTypes messageType, CommunicationToken communicationToken, XElement xElement)
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

            switch (messageType)
            {
                case MessageTypes.EmployerEnquiry:
                case MessageTypes.GlaEmployerEnquiry:
                    ResolveEmployerEnquiryElementValues(tokenKey, tokenValue, xElement);
                    break;
                case MessageTypes.WebAccessRequest:
                    ResolveAccessRequestElementValues(tokenKey, tokenValue, xElement);
                    break;
                default:
                    throw new CustomException(string.Format("MessageTypes {0} resolver does not exist", messageType));
            }
        }

        private void ResolveAccessRequestElementValues(CommunicationTokens tokenKey, string tokenValue, XElement xElement)
        {
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
                        xElementName = "Firstname";
                        xElement.Element("PrimaryContactName").Element(xElementName).Value = tokenValue;
                        break;
                    case CommunicationTokens.Position:
                        xElementName = "Position";
                        xElement.Element("PrimaryContactName").Element(xElementName).Value = tokenValue;
                        break;
                    case CommunicationTokens.Lastname:
                        xElementName = "Lastname";
                        xElement.Element("PrimaryContactName").Element(xElementName).Value = tokenValue;
                        break;
                    case CommunicationTokens.WorkPhoneNumber:
                        xElementName = "WorkPhoneNumber";
                        xElement.Element(xElementName).Value = tokenValue;
                        break;
                    case CommunicationTokens.MobileNumber:
                        xElementName = "MobilePhoneNumber";
                        xElement.Element(xElementName).Value = tokenValue;
                        break;
                    case CommunicationTokens.Email:
                        xElementName = "EmailAddress";
                        xElement.Element(xElementName).Value = tokenValue;

                        xElementName = "ConfirmEmailAddress";
                        xElement.Element(xElementName).Value = tokenValue;
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
                        xElementName = "OrganisationName";
                        xElement.Element(xElementName).Value = tokenValue;

                        xElementName = "CompanyName";
                        xElement.Element("AddressDetails").Element(xElementName).Value = tokenValue;
                        break;
                    case CommunicationTokens.HasApprenticeshipVacancies:
                        xElementName = "VacancyEnquiry";
                        xElement.Element(xElementName).Value = tokenValue;
                        break;
                    case CommunicationTokens.Contactname:
                        xElementName = "ContactName";
                        xElement.Element("SystemDetails").Element(xElementName).Value = tokenValue;
                        break;
                    case CommunicationTokens.AdditionalPhoneNumber:
                        xElementName = "ContactPhoneNumber";
                        xElement.Element("SystemDetails").Element(xElementName).Value = tokenValue;
                        break;
                    case CommunicationTokens.AdditionalEmail:
                        xElementName = "ContactEmailAddress";
                        xElement.Element("SystemDetails").Element(xElementName).Value = tokenValue;
                        break;
                    case CommunicationTokens.Systemname:
                        xElementName = "SystemName";
                        xElement.Element("SystemDetails").Element(xElementName).Value = tokenValue;
                        break;
                    case CommunicationTokens.SelectedServiceTypeIds:
                        xElementName = "ServiceRequired";
                        xElement.Element("SystemDetails").Element(xElementName).Value = tokenValue;
                        break;
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

        private void ResolveEmployerEnquiryElementValues(CommunicationTokens tokenKey, string tokenValue, XElement xElement)
        {
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