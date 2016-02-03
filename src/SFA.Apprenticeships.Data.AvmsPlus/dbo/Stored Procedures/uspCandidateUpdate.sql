create PROCEDURE [dbo].[uspCandidateUpdate]    
 @candidateId int,    
 @firstName varchar(60),    
 @middleName varchar(60),    
 @surName varchar(60),    
 @dateofBirth Datetime,    
 @addressLine1 varchar(100),    
 @addressLine2 varchar(100),    
 @addressLine3 varchar(100),    
 @addressLine4 varchar(100),    
 @addressLine5 varchar(100),    
 @town varchar(100),    
 @countyId int,    
 @postCode varchar(20),    
 @ulnStatusId int,    
 @gender int,    
 @landlineNumber varchar(32),    
 @mobileNumber varchar(32),    
 @email varchar(100),    
 @UnconfirmedEmailAddress nvarchar(200),    
 @mobileNumberUnconfirmed bit,    
 @ethnicOrigin int,    
 @ethnicOriginOther nvarchar(50),  
 @disability int,  
 @disabilityOther varchar(512),  
 @HealthProblems nvarchar(256),
 @ReferralPoints int =0,
 @BeingSupportedBy NVARCHAR(100)= NULL,
 @LockedForSupportUntil  datetime,
 @AllowMarketingMessages  bit
 AS    
 BEGIN    
  
  SET NOCOUNT ON    
  Declare @personId int    
     
  Select @personId = PersonId from Candidate where CandidateId = @candidateId    
    
 BEGIN TRY    
 UPDATE [dbo].[candidate]     
  SET [DateofBirth] = ISNULL(@dateofBirth,[DateofBirth]),     
  [AddressLine1] = ISNULL(@addressLine1,[AddressLine1]),     
  [AddressLine2] = ISNULL(@addressLine2,[AddressLine2]),    
  [AddressLine3] = ISNULL(@addressLine3,[AddressLine3]),    
  [AddressLine4] = ISNULL(@addressLine4,[AddressLine4]),    
  [AddressLine5] = ISNULL(@addressLine5,[AddressLine5]),    
  [Town] = ISNULL(@town,[Town]),    
  [CountyId] = ISNULL(@countyId,[CountyId]),
  [Postcode] = ISNULL(@postCode,[Postcode]),    
  [UlnStatusId] = ISNULL(@ulnStatusId,[UlnStatusId]),   
  [Gender] = ISNULL(@gender,[Gender]),    
  [UnconfirmedEmailAddress] = ISNULL(@UnconfirmedEmailAddress,[UnconfirmedEmailAddress]),    
  [MobileNumberUnconfirmed] = ISNULL(@mobileNumberUnconfirmed,[MobileNumberUnconfirmed]),    
  [EthnicOrigin] = ISNULL(@ethnicOrigin,[EthnicOrigin]),    
  [EthnicOriginOther] = ISNULL(@ethnicOriginOther,[EthnicOriginOther]),  
  [Disability] = ISNULL(@disability,[Disability]),  
  [DisabilityOther] = ISNULL(@disabilityOther,[DisabilityOther]),  
  [HealthProblems] = ISNULL(@HealthProblems ,[HealthProblems]),
  [ReferralPoints] = ISNULL(@ReferralPoints,[ReferralPoints]),
  [BeingSupportedBy] = ISNULL(@BeingSupportedBy,[BeingSupportedBy]),
  [LockedForSupportUntil] = ISNULL(@LockedForSupportUntil,[LockedForSupportUntil]),
  [AllowMarketingMessages] = ISNULL(@AllowMarketingMessages,[AllowMarketingMessages])

 WHERE [CandidateId]=@candidateId    
    
    
 UPDATE [dbo].[person] SET    
  [FirstName] = ISNULL(@firstName,[FirstName]),    
  [MiddleNames] = ISNULL(@middleName,[MiddleNames]),    
  [Surname] = ISNULL(@surName,[Surname]),    
  [LandlineNumber] = ISNULL(@landlineNumber,[LandlineNumber]),    
  [MobileNumber] = ISNULL(@mobileNumber,[MobileNumber]),    
  [Email] =ISNULL( @email,[Email])    
 WHERE PersonId = @personId    
    
 IF @@ROWCOUNT = 0    
 BEGIN    
  RAISERROR('Concurrent update error. Updated aborted.', 16, 2)    
 END    
    END TRY    
    
    BEGIN CATCH    
  EXEC RethrowError;    
 END CATCH     
    
 SET NOCOUNT OFF    
END