CREATE PROCEDURE [dbo].[uspUpdateTermsAndConditions]
@TermsAndConditionsId INT, @content NTEXT
AS
BEGIN
	SET NOCOUNT ON
	
	BEGIN TRY

		UPDATE [dbo].[TermsAndConditions] 
		SET [Content] = @content
		WHERE 
			[TermsAndConditionsId]= @TermsAndConditionsId

	END TRY

    BEGIN CATCH
		EXEC RethrowError;
	END CATCH
    
    SET NOCOUNT OFF
END