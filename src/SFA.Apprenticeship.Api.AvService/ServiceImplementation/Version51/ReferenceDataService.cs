namespace SFA.Apprenticeship.Api.AvService.ServiceImplementation.Version51
{
    using System;
    using System.ServiceModel;
    using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF;
    using MessageContracts.Version51;
    using Namespaces.Version51;
    using ServiceContracts.Version51;

    /*
    <s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
       <s:Body>
          <GetErrorCodesResponse xmlns="http://services.imservices.org.uk/AVMS/Interfaces/5.1">
             <ErrorCodes xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
                <ErrorCodesData>
                   <InterfaceErrorCode>-10001</InterfaceErrorCode>
                   <InterfaceErrorDescription>"WorkingWeek" must be 100 characters or less</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10002</InterfaceErrorCode>
                   <InterfaceErrorDescription>"WorkingWeek" is mandatory</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10003</InterfaceErrorCode>
                   <InterfaceErrorDescription>"WeeklyWage" should be atleast £40</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10004</InterfaceErrorCode>
                   <InterfaceErrorDescription>"WeeklyWage" is mandatory</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10005</InterfaceErrorCode>
                   <InterfaceErrorDescription>"VacancyType" is mandatory</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10006</InterfaceErrorCode>
                   <InterfaceErrorDescription>"Title" must be 200 characters or less</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10007</InterfaceErrorCode>
                   <InterfaceErrorDescription>"Title" is mandatory</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10008</InterfaceErrorCode>
                   <InterfaceErrorDescription>"PossibleStartDate" is mandatory</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10009</InterfaceErrorCode>
                   <InterfaceErrorDescription>"ShortDescription" must be 512 characters or less</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10010</InterfaceErrorCode>
                   <InterfaceErrorDescription>"ShortDescription" is mandatory</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10011</InterfaceErrorCode>
                   <InterfaceErrorDescription>"NumberOfPositions"  is mandatory for standard vacancies</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10012</InterfaceErrorCode>
                   <InterfaceErrorDescription>"LearningProviderEdsUrn" is mandatory</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10013</InterfaceErrorCode>
                   <InterfaceErrorDescription>"InterviewStartDate" is mandatory</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10014</InterfaceErrorCode>
                   <InterfaceErrorDescription>"LongDescription"  is mandatory</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10015</InterfaceErrorCode>
                   <InterfaceErrorDescription>"ApprenticeshipFramework" is mandatory</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10016</InterfaceErrorCode>
                   <InterfaceErrorDescription>"ApprenticeshipFramework" must be 3 characters</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10017</InterfaceErrorCode>
                   <InterfaceErrorDescription>"EmployerWebsite" must be 512 characters or less</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10018</InterfaceErrorCode>
                   <InterfaceErrorDescription>"EmployerExternalApplicationWebsite" must be 512 characters or less</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10019</InterfaceErrorCode>
                   <InterfaceErrorDescription>"EmployerEdsUrn" is mandatory</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10020</InterfaceErrorCode>
                   <InterfaceErrorDescription>"EmployerDescription" must be 8000 characters or less</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10021</InterfaceErrorCode>
                   <InterfaceErrorDescription>"EmployerAnonymousName" must be 510 characters or less</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10022</InterfaceErrorCode>
                   <InterfaceErrorDescription>"ContactName" must be 200 characters or less</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10023</InterfaceErrorCode>
                   <InterfaceErrorDescription>"ClosingDate" is mandatory</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10024</InterfaceErrorCode>
                   <InterfaceErrorDescription>"ApplicationInstructions" must be 8000 characters or less</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10025</InterfaceErrorCode>
                   <InterfaceErrorDescription>"NumberOfVacancies"  is mandatory for multi-site vacancies</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10026</InterfaceErrorCode>
                   <InterfaceErrorDescription>"ClosingDate" is Invalid</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10027</InterfaceErrorCode>
                   <InterfaceErrorDescription>"EmployerImage" size must be less than 10K</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10028</InterfaceErrorCode>
                   <InterfaceErrorDescription>"LongDescription" must be 2147483648 characters or less</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10029</InterfaceErrorCode>
                   <InterfaceErrorDescription>"InterviewStartDate" is Invalid</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10030</InterfaceErrorCode>
                   <InterfaceErrorDescription>"PossibleStartDate" is Invalid</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10031</InterfaceErrorCode>
                   <InterfaceErrorDescription>"EmployerExternalApplicationWebsite " in mandatory for offline vacancies</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10032</InterfaceErrorCode>
                   <InterfaceErrorDescription>"EmployerDescription" is mandatory for anonymous employer</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10033</InterfaceErrorCode>
                   <InterfaceErrorDescription>Learning Provider is not authorised for this vacancy</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10034</InterfaceErrorCode>
                   <InterfaceErrorDescription>Vacancy reference number already exists</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10035</InterfaceErrorCode>
                   <InterfaceErrorDescription>Invalid relationship for training provider and employer</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10036</InterfaceErrorCode>
                   <InterfaceErrorDescription>"ApprenticeshipFramework" is invalid</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10037</InterfaceErrorCode>
                   <InterfaceErrorDescription>"County" for standard location is invalid</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10038</InterfaceErrorCode>
                   <InterfaceErrorDescription>"County" for multiple location is invalid</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10039</InterfaceErrorCode>
                   <InterfaceErrorDescription>"County" for standard location is mandatory</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10040</InterfaceErrorCode>
                   <InterfaceErrorDescription>"County" for multiple location is mandatory</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10041</InterfaceErrorCode>
                   <InterfaceErrorDescription>"AddressLine1" for standard location is mandatory</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10042</InterfaceErrorCode>
                   <InterfaceErrorDescription>"AddressLine1" for multiple location is mandatory</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10043</InterfaceErrorCode>
                   <InterfaceErrorDescription>"EmployerImage" is not valid.</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10044</InterfaceErrorCode>
                   <InterfaceErrorDescription>Entered Training Provider and Employer cannot have national vacancy</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10045</InterfaceErrorCode>
                   <InterfaceErrorDescription>"EmployerEdsUrn" is invalid</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10046</InterfaceErrorCode>
                   <InterfaceErrorDescription>"LearningProviderEdsUrn" is invalid</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10047</InterfaceErrorCode>
                   <InterfaceErrorDescription>"PostCode" is mandatory for standard vacancies</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10048</InterfaceErrorCode>
                   <InterfaceErrorDescription>"PostCode" is mandatory for multisite vacancies</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10049</InterfaceErrorCode>
                   <InterfaceErrorDescription>"PostCode" is invalid for standard vacancy</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10050</InterfaceErrorCode>
                   <InterfaceErrorDescription>"PostCode" is invalid for multisite vacancy</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-1</InterfaceErrorCode>
                   <InterfaceErrorDescription>Unknown System Error</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-20001</InterfaceErrorCode>
                   <InterfaceErrorDescription>Unknown Vacancy Reference</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-20002</InterfaceErrorCode>
                   <InterfaceErrorDescription>InvalidVacancyReference</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-20003</InterfaceErrorCode>
                   <InterfaceErrorDescription>Invalid Update Value</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-20004</InterfaceErrorCode>
                   <InterfaceErrorDescription>You cannot record this number of candidates as successful as the total number of successes is greater than the number of vacancies available for this advert. Either the number of successful candidates reported is incorrect or the number of vacancies for this advert needs to be increased.</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-20005</InterfaceErrorCode>
                   <InterfaceErrorDescription>Updates Not Allowed</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10051</InterfaceErrorCode>
                   <InterfaceErrorDescription>Unsupported HTML Tags</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10052</InterfaceErrorCode>
                   <InterfaceErrorDescription>"DisplayRecruitmentAgency" is mandatory</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10053</InterfaceErrorCode>
                   <InterfaceErrorDescription>"SmallEmployerWageIncentive" is mandatory</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10054</InterfaceErrorCode>
                   <InterfaceErrorDescription>"DeliveryOrganisation" does not exist</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10055</InterfaceErrorCode>
                   <InterfaceErrorDescription>"VacancyManager" does not exist</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10056</InterfaceErrorCode>
                   <InterfaceErrorDescription>"ContractOwner" is not authorised for this vacancy</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10057</InterfaceErrorCode>
                   <InterfaceErrorDescription>"VacancyManager" is not authorised for this vacancy</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10058</InterfaceErrorCode>
                   <InterfaceErrorDescription>"DeliveryOrganisation" is not authorised for this vacancy</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10059</InterfaceErrorCode>
                   <InterfaceErrorDescription>"ContractOwnerUKPRN" is mandatory</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10060</InterfaceErrorCode>
                   <InterfaceErrorDescription>"DeliveryProviderEdsUrn" is mandatory</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10061</InterfaceErrorCode>
                   <InterfaceErrorDescription>"VacancyManagerEdsUrn" is mandatory</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10062</InterfaceErrorCode>
                   <InterfaceErrorDescription>"VacancyOwnerEdsUrn" is mandatory</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10063</InterfaceErrorCode>
                   <InterfaceErrorDescription>"LocalAuthority" does not exist</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10064</InterfaceErrorCode>
                   <InterfaceErrorDescription>"Address" is mandatory</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10065</InterfaceErrorCode>
                   <InterfaceErrorDescription>"SiteVacancyDetails" is mandatory</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10066</InterfaceErrorCode>
                   <InterfaceErrorDescription>"WageType" is invalid</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10067</InterfaceErrorCode>
                   <InterfaceErrorDescription>"PostCode" does not exist</InterfaceErrorDescription>
                </ErrorCodesData>
                <ErrorCodesData>
                   <InterfaceErrorCode>-10068</InterfaceErrorCode>
                   <InterfaceErrorDescription>"WeeklyWage" must be £0 for a traineeship</InterfaceErrorDescription>
                </ErrorCodesData>
             </ErrorCodes>
             <MessageId>14950389-76fb-1211-956e-188a35220001</MessageId>
          </GetErrorCodesResponse>
       </s:Body>
    </s:Envelope>
    */

    [ExceptionShielding("Default Exception Policy")]
    [ServiceBehavior(Namespace = Namespace.Uri)]
    public class ReferenceDataService : IReferenceData
    {
        public GetErrorCodesResponse GetErrorCodes(GetErrorCodesRequest request)
        {
            // TODO: AVMS: stored procedure: uspGetALLInterfaceErrorCodes.proc.sql.
            throw new NotImplementedException();
        }

        public GetApprenticeshipFrameworksResponse GetApprenticeshipFrameworks(GetApprenticeshipFrameworksRequest request)
        {
            throw new NotImplementedException();
        }

        public GetRegionResponse GetRegion(GetRegionRequest request)
        {
            throw new NotImplementedException();
        }

        public GetCountiesResponse GetCounties(GetCountiesRequest request)
        {
            throw new NotImplementedException();
        }

        public GetLocalAuthoritiesResponse GetLocalAuthorities(GetLocalAuthoritiesRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
