-- =============================================  
-- Script Template  
--25-01-2012 Modified by sanjeev to return provider id
-- =============================================  
CREATE PROCEDURE [dbo].[uspTrainingProviderOrganisationRegistration]  
@UKPRN INT,  
@organisationFullName  NVARCHAR (255),   
@organisationTradingName  NVARCHAR (255),   
@UPIN INT,   
@isContracted BIT,  
@contractedStartDate datetime,  
@contractedEndDate datetime,  
-- site parameters  
@fullName NVARCHAR (255),   
@tradingName NVARCHAR (255),  
 @EDSURN INT,   
 @addressLine1 NVARCHAR (50),   
 @addressLine2 NVARCHAR (50),   
 @addressLine3 NVARCHAR (50),   
 @addressLine4 NVARCHAR (50),   
 @addressLine5 NVARCHAR (50),   
 @town NVARCHAR (50),   
 @countyId INT,   
 @postCode NVARCHAR (8),  
  @localAuthorityId INT,   
  @ManagingAreaId INT,   
  @longitude DECIMAL (30, 15),   
  @latitude DECIMAL (30, 15),   
  @geocodeEasting DECIMAL (30, 15),   
  @geocodeNorthing DECIMAL (30, 15),   
  @ownerOrganisation NVARCHAR (255),   
  @employerDescription NVARCHAR (MAX),   
  @candidateDescription NVARCHAR (MAX),   
  @webPage NVARCHAR (100),   
  @outofDate BIT,   
  @contactDetailsForEmployer NVARCHAR (256),   
  @contactDetailsForCandidate NVARCHAR (256),   
  @hideFromSearch BIT,   
  @trainingProviderStatusTypeId INT,
  @providerSiteId INT OUTPUT   
AS  
BEGIN  
  
DECLARE @ProviderID INT;  
--DECLARE @ProviderSiteId INT;  
DECLARE @RelationshipTypeID INT = 1; --Owner  
DECLARE @ProviderSIteRelationShipID INT;  
  
BEGIN TRAN  
  
EXEC uspTrainingProviderInsert @UKPRN , @UPIN, @organisationFullName, @organisationTradingName , @isContracted ,@contractedStartDate, @contractedEndDate, @ProviderId  OUTPUT;  
EXEC uspTrainingProviderSiteInsert @fullName , @tradingName , @EDSURN , @addressLine1 , @addressLine2 , @addressLine3 , @addressLine4 , @addressLine5 , @town , @countyId , @postCode , @localAuthorityId , @ManagingAreaId, @longitude, @latitude, @geocodeEasting, @geocodeNorthing, @ownerOrganisation , @employerDescription , @candidateDescription, @webPage , @outofDate , @contactDetailsForEmployer , @contactDetailsForCandidate , @hideFromSearch , @ProviderSiteId  OUTPUT, @trainingProviderStatusTypeId ;  
EXEC uspTrainingProviderSiteRelationshipInsert @ProviderID, @ProviderSiteID, @RelationshipTypeID, @ProviderSiteRelationshipID OUTPUT;  
  
COMMIT  
  
  Return 0
END