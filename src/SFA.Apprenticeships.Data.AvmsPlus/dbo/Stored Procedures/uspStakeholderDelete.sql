CREATE PROCEDURE [dbo].[uspStakeholderDelete]
@stakeholderid INT
AS
begin
set nocount on

	BEGIN TRY

			declare	@personid int

			
			select @personid = (select personid 
								from stakeholder
								where stakeholderid = @stakeholderid)

			delete from stakeholder
			where stakeholderid = @stakeholderid

			delete from person
			where personid = @personid
					
			DELETE FROM CandidateDeRegistrationControl 
			WHERE candidateid = @stakeholderid
			AND roleId = 2
			

	END TRY
	
	BEGIN CATCH
		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;

		SELECT 
			@ErrorMessage = ERROR_MESSAGE(),
			@ErrorSeverity = ERROR_SEVERITY(),
			@ErrorState = ERROR_STATE();

		-- Use RAISERROR inside the CATCH block to return error
		-- information about the original error that caused
		-- execution to jump to the CATCH block.
		RAISERROR (@ErrorMessage, -- Message text.
				   @ErrorSeverity, -- Severity.
				   @ErrorState -- State.
				   );
	END CATCH

end