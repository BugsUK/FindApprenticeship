 CREATE PROCEDURE uspCandidateSelectByEmail
	-- Add the parameters for the stored procedure here
	@EmailAddress VARCHAR(100)
AS
BEGIN
  SELECT     
[Person].[PersonId]    
--      ,[Person].[ADUsername]    : Fields deleted as per new datamodel.
      ,[Person].[Title]    
      ,[Person].[OtherTitle]    
      ,[Person].[FirstName]    
      ,[Person].[MiddleNames]    
      ,[Person].[Surname]    
      ,[Person].[LandlineNumber]    
      ,[Person].[MobileNumber]    
      ,[Candidate].[TextFailureCount]   
      ,[Person].[Email]    
      ,[Candidate].[EmailFailureCount]  
	  ,[Candidate].[LocalAuthorityId] AS 'LocalAuthorityId'  
      --,[Person].[Type]    
      --,[Candidate].[EmailAlertSent]    : Fields deleted as per new datamodel.
      ,[Candidate].[LastAccessedDate]    
   ,[Candidate].[CandidateId]    
      ,[Candidate].[PersonId]    
      ,[Candidate].[CandidateStatusTypeId]    
      ,[Candidate].[DateofBirth]    
      ,[Candidate].[AddressLine1]    
      ,[Candidate].[AddressLine2]    
      ,[Candidate].[AddressLine3]    
      ,[Candidate].[AddressLine4]    
      ,[Candidate].[Town]    
      ,[CT].[FullName]
      ,[Candidate].CountyId   
      ,LAG.LocalAuthorityGroupID
      ,[Candidate].[Postcode]    
      ,CONVERT(Decimal(20,5),[Candidate].[Longitude]) as 'Longitude'    
      ,CONVERT(Decimal(20,5),[Candidate].[Latitude]) as 'Latitude'
      ,CONVERT(Decimal(20,5),[Candidate].[GeocodeEasting]) as 'GeocodeEasting'
      ,CONVERT(Decimal(20,5),[Candidate].[GeocodeNorthing]) as 'GeocodeNorthing'
--      ,[Candidate].[Latitude]    
--      ,[Candidate].[GeocodeEasting]    
--      ,[Candidate].[GeocodeNorthing]    
      ,[Candidate].[NiReference]    
      ,[Candidate].[UniqueLearnerNumber]    
      --,[Candidate].[UlnStatus]    : Need To Verify the Status with Stuart
      --,[Candidate].[LEA]    
      ,[Candidate].[Gender]    
      ,[Candidate].[EthnicOrigin]    
      ,[Candidate].[EthnicOriginOther]    
      --,[Candidate].[Religion]    : Not Required as per new DataModel
     -- ,[Candidate].[ReligionOther]    : Not Required as per new DataModel
      --,[Candidate].[DefaultSearchCriteria]    : Not Required as per new DataModel
     -- ,[Candidate].[SkillAccount]    : Not Required as per new DataModel
      ,[Candidate].[ApplicationLimitEnforced]    
      ,[Candidate].[LastAccessedDate]    
      ,[Candidate].[AdditionalEmail]    
      ,[Candidate].[Disability]   
   ,[Candidate].[DisabilityOther]   
      ,[Candidate].[HealthProblems]    
      --,[Candidate].[Childcare]    : Not Required as per new DataModel
      --,[Candidate].[CriminalRecord]    : Need To Verify the Status with Stuart
      --,[Candidate].[Nationality]    : Not Required as per new DataModel
      ,[Candidate].[ReceivePushedContent]    
      ,[Candidate].[ReferralAgent]    
      ,[Candidate].[DisableAlerts]    
      ,[Candidate].[UnconfirmedEmailAddress]    
      ,[Candidate].[MobileNumberUnconfirmed]  
	  ,[Candidate].[HealthProblems]  
	  
    ,ISNULL(RIGHT('0000000000'+ CAST(VoucherReferenceNumber AS VARCHAR(9)),9),0) as 'VoucherReferenceNumber'
	  --,[Candidate].[VoucherReferenceNumber]
   ,[Candidate].[ReferralPoints]
    ,[Candidate].[BeingSupportedBy]
    ,[Candidate].[LockedForSupportUntil]
    ,LA.[LocalAuthorityId]
    ,LA.[CodeName]
	,CH.EventDate as 'RegisteredDate'
	,CP.[FirstFrameworkId]
	,CP.[FirstOccupationId]
	,CP.[SecondFrameworkId] 
	,CP.[SecondOccupationId]  	
	,[Candidate].[AllowMarketingMessages]  
 FROM Candidate
 INNER JOIN LocalAuthority LA ON LA.LocalAuthorityId = Candidate.LocalAuthorityId
 INNER JOIN dbo.LocalAuthorityGroupMembership LAGM ON LA.LocalAuthorityId = LAGM.LocalAuthorityID
 INNER JOIN dbo.LocalAuthorityGroup LAG ON LAGM.LocalAuthorityGroupID = LAG.LocalAuthorityGroupID
 INNER JOIN dbo.LocalAuthorityGroupType LAGT ON LAG.LocalAuthorityGroupTypeID = LAGT.LocalAuthorityGroupTypeID
 AND LocalAuthorityGroupTypeName = N'Region'
 LEFT OUTER JOIN CandidatePreferences CP ON CP.CandidateId =  Candidate.CandidateId    
 INNER JOIN Person 	    
 ON Candidate.PersonId=Person.PersonId 
 LEFT JOIN County CT ON CT.CountyId = Candidate.CountyId
 left outer join CandidateHistory CH on candidate.CandidateId = CH.CandidateId
 and CH.CandidateHistoryEventTypeId = (select CHE.CandidateHistoryEventId from CandidateHistoryEvent CHE where  CHE.CodeName = 'STA')
 and CH.CandidateHistorySubEventTypeId= (select CS.CandidateStatusId from CandidateStatus CS where CS.CodeName = 'ATV')

 WHERE Person.Email = @EmailAddress
	--SELECT Candidate.CandidateID
	--FROM Person INNER JOIN Candidate ON Person.PersonID = Candidate.PersonID
	--WHERE Person.Email = @EmailAddress
END