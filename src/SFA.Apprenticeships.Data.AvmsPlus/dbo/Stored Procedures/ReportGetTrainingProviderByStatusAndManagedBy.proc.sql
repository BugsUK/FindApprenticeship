/*----------------------------------------------------------------------                  
Name  : ReportGetTrainingProviderByStatusAndManagedBy                
Description :  returns Learning Provider Based on Status and ManagingArea Id 

                
History:                  
--------                  
Date			Version		Author			Comment
======================================================================
21-Feb-2012		1.0			Mayank Khare	first version

---------------------------------------------------------------------- */                 

CREATE procedure [dbo].[ReportGetTrainingProviderByStatusAndManagedBy]
 @ManagedBy  NVARCHAR(3),
  @RegionID INT,
 @EmployerTrainingProviderStatusId INT,
 @rowcount			int = 0
as

BEGIN  
			SET NOCOUNT ON  
			SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 

		BEGIN TRY  
		SET ROWCOUNT @rowcount;
		SELECT      
				P.TradingName as ProviderOrganisation
				,P.UKPRN as SiteOwnerUKPRN
				,P.IsContracted as Contracted
				,PS.[TradingName] AS SiteName
				,PS.EDSURN as EDSURN
				,ISNULL(owner.[ukprn],'') AS SiteUKPRN
				,ISNULL(PS.AddressLine1,'') as AddressLine1
				,ISNULL(PS.AddressLine2,'') as AddressLine2
				,ISNULL(PS.AddressLine3,'') as AddressLine3
				,ISNULL(PS.AddressLine4,'') as AddressLine4
				,ISNULL(PS.AddressLine5,'') as AddressLine5
				,ISNULL(PS.Postcode, '') AS Postcode
				,ISNULL(PS.[Town],'') as Town
				,PSRT.ProviderSiteRelationshipTypeName AS [Type]
				,TPS.[FullName] AS TrainingProviderStatus	
				,MA.ManagingAreaFullName AS 'ManagedBy'
			  FROM Provider P
			  INNER JOIN ProviderSiteRelationship PSR ON P.ProviderID = PSR.ProviderID
			  INNER JOIN ProviderSiteRelationshipType PSRT ON PSR.ProviderSiteRelationshipTYpeID = PSRT.ProviderSiteRelationshipTYpeID 
			  INNER JOIN  [dbo].[ProviderSite] PS ON PSR.ProviderSiteID = PS.ProviderSiteID
			  INNER JOIN ProviderSiteRelationship AS OwnedBy on PS.ProviderSiteID = OwnedBy.ProviderSiteID
			  AND OwnedBy.ProviderSiteRelationshipTypeID = 1 --Owner
			  INNER JOIN Provider AS [Owner] ON OwnedBy.ProviderID = [Owner].ProviderID
			  INNER JOIN dbo.EmployerTrainingProviderStatus TPS ON TPS.EmployerTrainingProviderStatusId = PS.TrainingProviderStatusTypeId
			  INNER JOIN dbo.vwManagingAreas MA ON MA.ManagingAreaID = PS.ManagingAreaID
			  inner join LocalAuthority LA on LA.LocalAuthorityId = PS.LocalAuthorityId
			  INNER JOIN vwRegionsAndLocalAuthority RLA	ON LA.LocalAuthorityID = RLA.LocalAuthorityID
			  WHERE (@EmployerTrainingProviderStatusId = -1 OR  PS.[TrainingProviderStatusTypeId] = @EmployerTrainingProviderStatusId)
			  AND (@ManagedBy = '-1' OR PS.ManagingAreaID IN (SELECT LocalAuthorityGroupID FROM dbo.ReportGetChildManagingAreas(@ManagedBy)))
			  AND (@RegionID= -1 or rla.GeographicRegionID = @RegionID) 
		 ORDER BY P.[TradingName]		
		END TRY  
		BEGIN CATCH  
				EXEC RethrowError
		END CATCH  
      
    SET NOCOUNT OFF  
END