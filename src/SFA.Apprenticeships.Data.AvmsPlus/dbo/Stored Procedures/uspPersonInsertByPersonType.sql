CREATE PROCEDURE [dbo].[uspPersonInsertByPersonType]
        @Title int = NULL,
        @OtherTitle nvarchar(10) = NULL,
        @FirstName nvarchar(35) = NULL,
        @MiddleNames nvarchar(35)= NULL,
        @Surname nvarchar(35)= NULL,
        @LandlineNumber nvarchar(16) = NULL,
        @MobileNumber nvarchar(16) = NULL,
        @TextFailureCount smallint = NULL,
        @Email nvarchar(50) = NULL,
        @EmailFailureCount smallint = NULL,      
        @EmailAlertSent bit = NULL,
		@PersonTypeId int = NULL,
        @PersonId int out
AS
BEGIN
	SET NOCOUNT ON
	BEGIN TRY

        INSERT INTO [dbo].[Person] 
        (
        Title,
        OtherTitle,
        FirstName,
        MiddleNames,
        Surname,
        LandlineNumber,
        MobileNumber,
        Email,
        PersonTypeId
        )
	      VALUES 
        (
        @Title,
        @OtherTitle,
        @FirstName,
        @MiddleNames,
        @Surname,
        @LandlineNumber,
        @MobileNumber,
        @Email,
        @PersonTypeId
        )
    SET @PersonId = SCOPE_IDENTITY()

    END TRY

    BEGIN CATCH
		EXEC RethrowError;
	END CATCH
    
    SET NOCOUNT OFF
END