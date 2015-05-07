using System;
using System.Xml.Linq;
using SFA.Apprenticeships.Common.Extensions;

namespace SFA.Apprenticeships.Web.ContactForms.Providers
{
    using Application.Interfaces.Communications;
    using Domain.Entities;
    using Interfaces;
    using Mappers.Interfaces;
    using ViewModels;

    public class EmployerEnquiryProvider : IEmployerEnquiryProvider
    {
        private readonly ICommunciationService _communciationService;
        private readonly IViewModelToDomainMapper<EmployerEnquiryViewModel, EmployerEnquiry> _employerEnquiryViewModelToDomainMapper;

        public EmployerEnquiryProvider(ICommunciationService communciationService,
            IViewModelToDomainMapper<EmployerEnquiryViewModel, EmployerEnquiry> employerEnquiryVtoDMapper)
        {
            _communciationService = communciationService;
            _employerEnquiryViewModelToDomainMapper = employerEnquiryVtoDMapper;
        }

        public SubmitQueryStatus SubmitEnquiry(EmployerEnquiryViewModel employerEnquiryData)
        {
            try
            {
                var result = _employerEnquiryViewModelToDomainMapper.ConvertToDomain(employerEnquiryData);
                
                var communicationTokens = new[]
                {
                    //common tokens 
                    new CommunicationToken(CommunicationTokens.Address1, result.Address.AddressLine1),
                    new CommunicationToken(CommunicationTokens.Address2, result.Address.AddressLine2),
                    new CommunicationToken(CommunicationTokens.Address3, result.Address.AddressLine3),
                    new CommunicationToken(CommunicationTokens.City, result.Address.City),
                    new CommunicationToken(CommunicationTokens.Postcode, result.Address.Postcode),
                    new CommunicationToken(CommunicationTokens.FullAddress, String.Format("{0} {1} {2} {3} {4}",result.Address.AddressLine1, result.Address.AddressLine2,result.Address.AddressLine3,result.Address.City,result.Address.Postcode)),
                    new CommunicationToken(CommunicationTokens.Title, result.Title),
                    new CommunicationToken(CommunicationTokens.Firstname, result.Firstname),
                    new CommunicationToken(CommunicationTokens.Lastname, result.Lastname),
                    new CommunicationToken(CommunicationTokens.Email, result.Email),
                    new CommunicationToken(CommunicationTokens.Fullname, String.Format("{0} {1} {2}",result.Title.ToFirstCharToUpper(), result.Firstname.ToFirstCharToUpper(), result.Lastname.ToFirstCharToUpper())),
                    new CommunicationToken(CommunicationTokens.WorkPhoneNumber, result.WorkPhoneNumber),
                    new CommunicationToken(CommunicationTokens.MobileNumber, result.MobileNumber),
                    new CommunicationToken(CommunicationTokens.Companyname, result.Companyname),
                    new CommunicationToken(CommunicationTokens.Position, result.Position),
                    //Employer enquiry specific tokens 
                    new CommunicationToken(CommunicationTokens.EmployeesCount, result.EmployeesCount),
                    new CommunicationToken(CommunicationTokens.EnquiryDescription, result.EnquiryDescription),
                    new CommunicationToken(CommunicationTokens.EnquirySource, result.EnquirySource),
                    new CommunicationToken(CommunicationTokens.PreviousExperienceType, result.PreviousExperienceType),
                    new CommunicationToken(CommunicationTokens.WorkSector, result.WorkSector),
                    new CommunicationToken(CommunicationTokens.EnquiryRelatesTo, result.EnquiryRelatesTo)
                    };
                //Send confirmation email to applicant
                _communciationService.SendMessage(MessageTypes.EmployerEnquiry, communicationTokens);
                //Send email to helpdesk
                _communciationService.SendMessage(MessageTypes.EmployerEnquiryConfirmation, communicationTokens);
                return SubmitQueryStatus.Success;
            }
            catch (System.Exception exception)
            {
                //todo: log error using preferred logging mechanism
                return SubmitQueryStatus.Error;
            }
        }


        public SubmitQueryStatus SubmitGlaEnquiry(EmployerEnquiryViewModel employerEnquiryData)
        {
            try
            {
                var result = _employerEnquiryViewModelToDomainMapper.ConvertToDomain(employerEnquiryData);

                var enquirySource = result.EnquirySource == null ? null : result.EnquirySource.ToUpper();
                var natureOfEnquiry = string.Format("HOW DID YOU HEAR ABOUT GLA: {0}\r\n{1}", enquirySource, result.EnquiryDescription);

                var communicationTokens = new[]
                {
                    //common tokens 
                    new CommunicationToken(CommunicationTokens.Address1, result.Address.AddressLine1),
                    new CommunicationToken(CommunicationTokens.Address2, result.Address.AddressLine2),
                    new CommunicationToken(CommunicationTokens.Address3, result.Address.AddressLine3),
                    new CommunicationToken(CommunicationTokens.City, result.Address.City),
                    new CommunicationToken(CommunicationTokens.Postcode, result.Address.Postcode),
                    new CommunicationToken(CommunicationTokens.FullAddress, String.Format("{0} {1} {2} {3} {4}",result.Address.AddressLine1, result.Address.AddressLine2,result.Address.AddressLine3,result.Address.City,result.Address.Postcode)),
                    new CommunicationToken(CommunicationTokens.Title, result.Title),
                    new CommunicationToken(CommunicationTokens.Firstname, result.Firstname),
                    new CommunicationToken(CommunicationTokens.Lastname, result.Lastname),
                    new CommunicationToken(CommunicationTokens.Email, result.Email),
                    new CommunicationToken(CommunicationTokens.Fullname, String.Format("{0} {1} {2}",result.Title.ToFirstCharToUpper(), result.Firstname.ToFirstCharToUpper(), result.Lastname.ToFirstCharToUpper())),
                    new CommunicationToken(CommunicationTokens.WorkPhoneNumber, result.WorkPhoneNumber),
                    new CommunicationToken(CommunicationTokens.MobileNumber, result.MobileNumber),
                    new CommunicationToken(CommunicationTokens.Companyname, result.Companyname),
                    new CommunicationToken(CommunicationTokens.Position, result.Position),
                    //Employer enquiry specific tokens 
                    new CommunicationToken(CommunicationTokens.EmployeesCount, result.EmployeesCount),
                    new CommunicationToken(CommunicationTokens.EnquiryDescription, natureOfEnquiry),
                    new CommunicationToken(CommunicationTokens.EnquirySource, result.EnquirySource),
                    new CommunicationToken(CommunicationTokens.PreviousExperienceType, result.PreviousExperienceType),
                    new CommunicationToken(CommunicationTokens.WorkSector, result.WorkSector),
                    new CommunicationToken(CommunicationTokens.EnquiryRelatesTo, result.EnquiryRelatesTo)
                    };
                //Send confirmation email to applicant
                _communciationService.SendMessage(MessageTypes.GlaEmployerEnquiry, communicationTokens);
                //Send email to helpdesk
                _communciationService.SendMessage(MessageTypes.GlaEmployerEnquiryConfirmation, communicationTokens);
                return SubmitQueryStatus.Success;
            }
            catch (System.Exception exception)
            {
                //todo: log error using preferred logging mechanism
                return SubmitQueryStatus.Error;
            }
        }
    }
}