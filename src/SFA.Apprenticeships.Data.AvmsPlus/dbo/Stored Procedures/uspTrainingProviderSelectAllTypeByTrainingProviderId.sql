CREATE PROCEDURE dbo.uspTrainingProviderSelectAllTypeByTrainingProviderId
@trainingProviderId INT
AS
BEGIN   
            
	SET NOCOUNT ON          
	       
	SELECT  TOP 1
		   isnull([ProviderSite].[FullName],'') AS 'FullName',   
		   [ProviderSite].[OutofDate] AS 'OutofDate',            
		   isnull([ProviderSite].[TradingName],'') AS 'TradingName',            
		   [ProviderSite].[ProviderSIteID] AS 'TrainingProviderId',            
		   [Provider].[UPIN] AS 'UPIN',
		   [ProviderSite].[EdsUrn] AS 'EDSURN',
		   isnull([AddressLine1],'') AS 'AddressLine1',              
		   isnull([AddressLine2],'') AS 'AddressLine2',              
		   isnull([AddressLine3],'') AS 'AddressLine3',             
		   isnull([AddressLine4],'') AS 'AddressLine4',          
		   County.FullName AS 'County',              
		   isnull([Postcode],'') AS 'Postcode',              
		   isnull([Town],'') AS 'Town',      
		   LAG.FullName AS 'Region',  
		   [ProviderSite].ManagingAreaID,  
		   [ProviderSite].[CountyId]     
		   ,ISNULL(EmployerDescription,'') AS EmployerDescription
		   ,ISNULL(ContactDetailsForEmployer,'') AS  ContactDetailsForEmployer
			,ISNULL(WebPage,'') AS  WebPage    
			,ISNULL(HideFromSearch,'') AS HideFromSearch
			,ISNULL(ContactDetailsForCandidate,'') AS ContactDetailsForCandidate
			,ISNULL(CandidateDescription,'') AS CandidateDescription
			,[Provider].UKPRN
			,[ProviderSite].OwnerOrganisation
			,[ProviderSite].TrainingProviderStatusTypeId
			,[Provider].ProviderID
			,isnull([Provider].TradingName,'') AS 'ProviderTradingName'          
			,isnull([Provider].FullName,'') AS 'ProviderFullName'
			,[Provider].IsContracted
			,[Provider].ContractedFrom
			,[Provider].ContractedTo
			,[Provider].IsNASProvider
	FROM [dbo].[ProviderSite]  
			JOIN ProviderSiteRelationship ON ProviderSite.ProviderSiteID = ProviderSiteRelationship.ProviderSiteID
			JOIN ProviderSiteRelationshipType ON ProviderSiteRelationship.ProviderSiteRelationshipTYpeID = ProviderSiteRelationshipTYpe.ProviderSiteRelationshipTYpeID 
			JOIN Provider ON ProviderSiteRelationship.ProviderID = Provider.ProviderID         
			LEFT JOIN County ON [County].[CountyId] = [ProviderSite].[CountyId]      
			INNER JOIN dbo.LocalAuthorityGroup LAG ON  LAG.LocalAuthorityGroupID = [ProviderSite].ManagingAreaID 
			INNER JOIN dbo.LocalAuthorityGroupType LAGT ON LAG.LocalAuthorityGroupTypeID = LAGT.LocalAuthorityGroupTypeID
			AND LocalAuthorityGroupTypeName = N'Managing Area'  
	WHERE 
			[ProviderSite].ProviderSiteID = @trainingProviderId   
	  
	SET NOCOUNT OFF 
	         
END