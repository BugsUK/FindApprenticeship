CREATE PROCEDURE [dbo].[uspTrainingProviderSiteInsert]
@fullName NVARCHAR (255), @tradingName NVARCHAR (255), @EDSURN INT, @addressLine1 NVARCHAR (50), @addressLine2 NVARCHAR (50), @addressLine3 NVARCHAR (50), @addressLine4 NVARCHAR (50), @addressLine5 NVARCHAR (50), @town NVARCHAR (50), @countyId INT, @postCode NVARCHAR (8), @localAuthorityId INT, @ManagingAreaId INT, @longitude DECIMAL (30, 15), @latitude DECIMAL (30, 15), @geocodeEasting DECIMAL (30, 15), @geocodeNorthing DECIMAL (30, 15), @ownerOrganisation NVARCHAR (255), @employerDescription NVARCHAR (MAX), @candidateDescription NVARCHAR (MAX), @webPage NVARCHAR (100), @outofDate BIT, @contactDetailsForEmployer NVARCHAR (256), @contactDetailsForCandidate NVARCHAR (256), @hideFromSearch BIT, @trainingProviderId INT OUTPUT, @trainingProviderStatusTypeId INT
AS
BEGIN      
SET NOCOUNT ON      
       
BEGIN TRY      
IF NOT EXISTS( select EDSURN from [dbo].[ProviderSite] where EDSURN = @EDSURN)    
BEGIN    
INSERT INTO [dbo].[ProviderSite]    
           ([FullName]    
           ,[TradingName]    
         --  ,[UKPRN]    
          -- ,[UPIN]    
           ,[EDSURN]    
           ,[AddressLine1]    
           ,[AddressLine2]    
           ,[AddressLine3]    
           ,[AddressLine4]    
           ,[AddressLine5]    
           ,[Town]    
           ,[CountyId]    
           ,[PostCode]    
           ,[LocalAuthorityId]    
           ,ManagingAreaID    
           ,[Longitude]    
           ,[Latitude]    
           ,[GeocodeEasting]    
           ,[GeocodeNorthing]    
           ,[OwnerOrganisation]    
           ,[EmployerDescription]    
           ,[CandidateDescription]    
           ,[WebPage]    
           ,[OutofDate]    
           ,[ContactDetailsForEmployer]    
           ,[ContactDetailsForCandidate]    
           ,[HideFromSearch]    
   ,[TrainingProviderStatusTypeId])    
     VALUES    
           (@fullName,    
           @tradingName,    
        --   @UKPRN,    
       --    @UPIN,    
           @EDSURN,    
           @addressLine1,    
           @addressLine2,    
           @addressLine3,    
           @addressLine4,    
           @addressLine5,    
           @town,    
           @countyId,    
           @postCode,    
           @localAuthorityId,    
           @ManagingAreaId,    
           @longitude,    
           @latitude,    
           @geocodeEasting,    
           @geocodeNorthing,    
           @ownerOrganisation,    
           @employerDescription,    
           @candidateDescription,    
           @webPage,    
           @outofDate,    
           @contactDetailsForEmployer,    
           @contactDetailsForCandidate,    
           @hideFromSearch,    
     @trainingProviderStatusTypeId)    
    
SET @trainingProviderId = SCOPE_IDENTITY()      
END    
    
ELSE    
    
BEGIN    
SELECT @trainingProviderId = ProviderSiteID from [dbo].[ProviderSite] where EDSURN = @EDSURN    
  
UPDATE [dbo].[ProviderSite]    
   SET [FullName] = ISNULL(@fullName,[FullName])    
      ,[TradingName] = ISNULL(@tradingName,[TradingName])    
     -- ,[UKPRN] = ISNULL(@UKPRN,[UKPRN])    
     -- ,[UPIN] = ISNULL(@UPIN,[UPIN])    
      ,[EDSURN] = ISNULL(@EDSURN,[EDSURN])    
      ,[AddressLine1] = ISNULL(@addressLine1,[AddressLine1])    
      ,[AddressLine2] = ISNULL(@addressLine2,[AddressLine2])    
      ,[AddressLine3] = ISNULL(@addressLine3,[AddressLine3])    
      ,[AddressLine4] = ISNULL(@addressLine4,[AddressLine4])    
      ,[AddressLine5] = ISNULL(@addressLine5,[AddressLine5])    
      ,[Town] = ISNULL(@town,[Town])    
      ,[CountyId] = ISNULL(@countyId,[CountyId])    
      ,[PostCode] = ISNULL(@postCode,[PostCode])    
      ,[LocalAuthorityId] = ISNULL(@localAuthorityId,[LocalAuthorityId])    
      ,ManagingAreaID = ISNULL(@ManagingAreaId,[ManagingAreaId])    
      ,[Longitude] = ISNULL(@longitude,[Longitude])    
      ,[Latitude] = ISNULL(@latitude,[Latitude])    
      ,[GeocodeEasting] = ISNULL(@geocodeEasting,[GeocodeEasting])    
      ,[GeocodeNorthing] = ISNULL(@geocodeNorthing,[GeocodeNorthing])    
      ,[OwnerOrganisation] = ISNULL(@ownerOrganisation,[OwnerOrganisation])    
      ,[EmployerDescription] = ISNULL(@employerDescription,[EmployerDescription])    
      ,[CandidateDescription] = ISNULL(@candidateDescription,[CandidateDescription])    
      ,[WebPage] = ISNULL(@webPage,[WebPage])    
      ,[OutofDate] = ISNULL(@outofDate,[OutofDate])    
      ,[ContactDetailsForEmployer] = ISNULL(@contactDetailsForEmployer,[ContactDetailsForEmployer])    
      ,[ContactDetailsForCandidate] = ISNULL(@contactDetailsForCandidate,[ContactDetailsForCandidate])    
      ,[HideFromSearch] = ISNULL(@hideFromSearch,[HideFromSearch])    
   ,[TrainingProviderStatusTypeId] = ISNULL(@trainingProviderStatusTypeId,[TrainingProviderStatusTypeId])    
 WHERE ProviderSiteID=@trainingProviderId     
  
END    
      
END TRY      
      
BEGIN CATCH      
 EXEC RethrowError;      
END CATCH      
          
SET NOCOUNT OFF      
    
END