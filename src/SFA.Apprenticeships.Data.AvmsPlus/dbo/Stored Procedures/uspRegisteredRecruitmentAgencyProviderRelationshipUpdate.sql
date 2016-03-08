CREATE PROCEDURE [dbo].[uspRegisteredRecruitmentAgencyProviderRelationshipUpdate]


@ProviderID INT, 
@ProviderSiteID INT,
@RelationShipType INT,
@isLinked BIT,
@ProviderSiteRelationshipID INT OUTPUT


AS
BEGIN  
SET NOCOUNT ON 
BEGIN TRY 
	IF (@isLinked=1)    
	BEGIN
	     
		IF NOT EXISTS( select ProviderSiteRelationshipID from [dbo].[ProviderSiteRelationship] where [ProviderID] = @ProviderID and [ProviderSiteID] = @ProviderSiteID AND ProviderSiteRelationShipTypeID=@RelationShipType)     
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
	END    
	 
	ELSE
	BEGIN
		IF EXISTS( select ProviderSiteRelationshipID from [dbo].[ProviderSiteRelationship] where [ProviderID] = @ProviderID and [ProviderSiteID] = @ProviderSiteID AND ProviderSiteRelationShipTypeID=@RelationShipType)     
		BEGIN
			DECLARE @ExistingPSRId int
			SELECT @ExistingPSRId = ProviderSiteRelationshipID
			FROM [dbo].[ProviderSiteRelationship] 
			WHERE [ProviderID] = @ProviderID and [ProviderSiteID] = @ProviderSiteID AND ProviderSiteRelationShipTypeID=@RelationShipType
			
			DELETE FROM RecruitmentAgentLinkedRelationships WHERE ProviderSiteRelationshipID=@ExistingPSRId
			DELETE FROM [ProviderSiteRelationship]  WHERE ProviderSiteRelationshipID=@ExistingPSRId
		END 
	END
END TRY
	 
BEGIN CATCH      
	EXEC RethrowError;      
END CATCH  
	          
SET NOCOUNT OFF      
	 
END