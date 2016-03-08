CREATE PROCEDURE [dbo].[uspFAQDelete]
@faqId INT
AS
BEGIN
	SET NOCOUNT ON
	
	BEGIN TRY

		
		DELETE FROM [dbo].[FAQ] 
		WHERE 
		 [FaqId] = @faqId

	END TRY

    BEGIN CATCH
		EXEC RethrowError;
	END CATCH
    
    SET NOCOUNT OFF
END