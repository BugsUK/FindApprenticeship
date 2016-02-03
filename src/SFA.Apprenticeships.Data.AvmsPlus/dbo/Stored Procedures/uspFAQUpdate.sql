CREATE PROCEDURE [dbo].[uspFAQUpdate]
@faqId INT, @userType INT, @faqTitle nvarchar(100), @content nvarchar(2000)
AS
BEGIN
	SET NOCOUNT ON
	
	BEGIN TRY

		
		UPDATE [dbo].[FAQ] 
		SET [Title] = @faqTitle,
			[Content] = @content
		WHERE 
			[UserTypeId]= @userType
		AND [FaqId] = @faqId

	END TRY

    BEGIN CATCH
		EXEC RethrowError;
	END CATCH
    
    SET NOCOUNT OFF
END