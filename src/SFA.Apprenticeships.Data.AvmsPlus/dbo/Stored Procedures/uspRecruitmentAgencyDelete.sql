CREATE PROCEDURE [dbo].[uspRecruitmentAgencyDelete]
	@RecAgentId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from              
	-- interfering with SELECT statements.              
	SET NOCOUNT ON;        
	
	BEGIN TRY
		BEGIN TRAN

	      
   
		-- is the RA standalone?
		IF EXISTS ( SELECT 1 FROM ProviderSiteRelationship WHERE ProviderSiteID = @RecAgentId AND ProviderSiteRelationShipTypeID IN (1,2))
		BEGIN 
			-- RA is NOT standalone
			
			-- remove employer links
			DELETE FROM RecruitmentAgentLinkedRelationships 
			WHERE ProviderSiteRelationshipID IN 
				(SELECT ProviderSiteRelationshipID 
				 FROM ProviderSiteRelationship 
				 WHERE ProviderSiteID = @RecAgentId AND ProviderSiteRelationShipTypeID = 3);
			
			-- remove the type 3 links
			DELETE FROM ProviderSiteRelationship WHERE ProviderSiteID = @RecAgentId AND ProviderSiteRelationShipTypeID = 3
			
			-- make Provider Site not a recruitment agancy
			UPDATE ProviderSite
			SET IsRecruitmentAgency = 0
			WHERE ProviderSiteID = @RecAgentId
		END
		ELSE
		BEGIN
			IF EXISTS(SELECT 1 FROM ProviderSiteRelationship WHERE ProviderSiteID = @RecAgentId AND ProviderSiteRelationShipTypeID = 3)
			BEGIN
			
				-- remove employer links
				DELETE FROM RecruitmentAgentLinkedRelationships 
				WHERE ProviderSiteRelationshipID IN 
					(SELECT ProviderSiteRelationshipID 
					 FROM ProviderSiteRelationship 
					 WHERE ProviderSiteID = @RecAgentId AND ProviderSiteRelationShipTypeID = 3);
			
				-- remove the type 3 links
				DELETE FROM ProviderSiteRelationship WHERE ProviderSiteID = @RecAgentId AND ProviderSiteRelationShipTypeID = 3
			
				DECLARE @StatusId int
				SELECT @StatusId=EmployerTrainingProviderStatusId
				FROM EmployerTrainingProviderStatus
				WHERE CodeName='DEL'
			
			
				UPDATE ProviderSite
				SET TrainingProviderStatusTypeId = @StatusId, IsRecruitmentAgency = 0
				WHERE ProviderSiteID = @RecAgentId
			END
			ELSE
			BEGIN
				DELETE FROM ProviderSite 
				WHERE ProviderSiteID = @RecAgentId
			END
		END 
		
		COMMIT TRAN;
	END TRY
	BEGIN CATCH
		IF (@@TRANCOUNT > 0)
			ROLLBACK TRAN;

		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;

		SELECT @ErrorMessage = ERROR_MESSAGE(),
			   @ErrorSeverity = ERROR_SEVERITY(),
			   @ErrorState = ERROR_STATE();

		-- Use RAISERROR inside the CATCH block to return 
		-- error information about the original error that 
		-- caused execution to jump to the CATCH block.
		RAISERROR (@ErrorMessage, -- Message text.
				   @ErrorSeverity, -- Severity.
				   @ErrorState -- State.
				   );
	END CATCH
END