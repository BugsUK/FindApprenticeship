CREATE PROCEDURE [dbo].[uspUpdateSubContractorStatus]
	@providerSiteId int, 
	@contractorId int, 
	@subContracted bit
AS
	BEGIN TRY
		BEGIN TRANSACTION
		SET NOCOUNT ON

		IF @subContracted = 1
		BEGIN
			IF NOT EXISTS (SELECT 1 FROM ProviderSiteRelationship WHERE ProviderID = @contractorId AND ProviderSiteID = @providerSiteId AND ProviderSiteRelationShipTypeID = 2)
			BEGIN
				INSERT INTO [ProviderSiteRelationship]
					(ProviderID, ProviderSiteID, ProviderSiteRelationShipTypeID)
				 VALUES
					   (@contractorId, @providerSiteId, 2)
			END
		END
		ELSE
		BEGIN
			EXEC uspTrainingProviderOfferDeleteByTrainingProviderId @providerSiteId, 2, @contractorId
			EXEC uspTrainingProviderLocationDeleteByRelationship @providerSiteId, 2, @contractorId
			EXEC uspTrainingProviderFrameworkDeleteByRelationship @providerSiteId, 2, @contractorId
		
			DELETE FROM ProviderSiteRelationship 
			WHERE ProviderID = @contractorId AND ProviderSiteID = @providerSiteId AND ProviderSiteRelationShipTypeID = 2
		END
		
		COMMIT TRANSACTION
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