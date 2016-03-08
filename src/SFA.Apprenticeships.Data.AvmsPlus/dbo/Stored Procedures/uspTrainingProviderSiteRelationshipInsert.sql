CREATE PROCEDURE [dbo].[uspTrainingProviderSiteRelationshipInsert]


@ProviderID INT, 
@ProviderSiteID INT,
@RelationShipType INT,
@ProviderSiteRelationshipID INT OUTPUT


AS
BEGIN      
SET NOCOUNT ON      
 
       
BEGIN TRY      
IF NOT EXISTS( select ProviderSiteRelationshipID from [dbo].[ProviderSiteRelationship] where [ProviderID] = @ProviderID and [ProviderSiteID] = @ProviderSiteID )    
BEGIN    
INSERT INTO [dbo].[ProviderSiteRelationship]    
           ([ProviderID]
           ,[ProviderSiteID]
           ,[ProviderSiteRelationShipTypeID])    
     VALUES    
           (@ProviderID
           ,@ProviderSiteID    
           ,@RelationShipType)    
    
SET @ProviderSiteRelationshipID = SCOPE_IDENTITY()      
END    
    
END TRY      
      
BEGIN CATCH      
 EXEC RethrowError;      
END CATCH      
          
SET NOCOUNT OFF      
    
END