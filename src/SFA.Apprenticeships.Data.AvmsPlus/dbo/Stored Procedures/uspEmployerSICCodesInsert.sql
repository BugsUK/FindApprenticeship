CREATE PROCEDURE [dbo].[uspEmployerSICCodesInsert]
   @EmployerId int,
   @SicId int,
   @SicInsertId int out
   

AS
BEGIN
	SET NOCOUNT ON
	
	BEGIN TRY
	DECLARE @SicCodeId int
	SELECT @SicCodeId=SicCodeId FROM SICCode WHERE SICCode=@SicId

    INSERT INTO EmployerSICCodes
    Values ( @EmployerId,@SicCodeId)    

        SET @SicInsertId = SCOPE_IDENTITY()

    END TRY

    BEGIN CATCH
		EXEC RethrowError;
	END CATCH
    
    SET NOCOUNT OFF
END