CREATE PROCEDURE [dbo].[uspFAQInsert]
@userType INT, @faqTitle nvarchar(100), @content nvarchar(2000)
AS
BEGIN
	SET NOCOUNT ON
	
	BEGIN TRY

		declare @sortOrder int
		if exists(SELECT  1  from [dbo].[FAQ] where [UserTypeId]=@userType)
			SELECT  @sortOrder = max([SortOrder])+1  from [dbo].[FAQ] where [UserTypeId]=@userType
		else
			SET @sortOrder = 1

		INSERT INTO [dbo].[FAQ] 
		([UserTypeId],[Title],[Content],[SortOrder])
		VALUES (@userType, @faqTitle, @content,@sortOrder)

	END TRY

    BEGIN CATCH
		EXEC RethrowError;
	END CATCH
    
    SET NOCOUNT OFF
END