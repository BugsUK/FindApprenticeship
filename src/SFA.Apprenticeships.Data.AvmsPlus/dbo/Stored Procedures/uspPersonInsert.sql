CREATE PROCEDURE [dbo].[uspPersonInsert]
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
       --@PersonTypeId int = NULL,
        @EmailAlertSent bit = NULL,
        @PersonId int out
AS
BEGIN
	SET NOCOUNT ON
-- TODO:- new Fld/table name to be updated as fnd missing on 18/Jul/08 	
	BEGIN TRY

--		IF EXISTS(SELECT PersonId FROM Person WHERE ADUsername=@ADUsername)
--			RAISERROR('Concurrent update error. Updated aborted.', 16, 2)  

        INSERT INTO [dbo].[Person] 
        (
       --PersonId,
       -- ADUsername,
        Title,
        OtherTitle,
        FirstName,
        MiddleNames,
        Surname,
        LandlineNumber,
        MobileNumber,
		--TextFailureCount,
        Email,
		--EmailFailureCount,
        PersonTypeId
		--EmailAlertSent,
        --LastAccessed
        )
	      VALUES 
        (
        --@PersonId,
      --  @ADUsername,
        @Title,
        @OtherTitle,
        @FirstName,
        @MiddleNames,
        @Surname,
        @LandlineNumber,
        @MobileNumber,
		--@TextFailureCount,
        @Email,
       -- @EmailFailureCount,
        2--@PersonTypeId,
		--@EmailAlertSent,
        --getdate()
        )
    SET @PersonId = SCOPE_IDENTITY()
    
--   INSERT INTO [dbo].[Person] ([ADUsername], [Availability], [ContactPreference], [Email], [EmailAlertSent], [EmailFailureCount], [FirstName], [JobTitle], [LandlineNumber], [MiddleNames], [MobileNumber], [PersonId], [Surname], [TextFailureCount], [Title], [Type])
--	VALUES (@aDUsername, @availability, @contactPreference, @email, @emailAlertSent, @emailFailureCount, @firstName, @jobTitle, @landlineNumber, @middleNames, @mobileNumber, @personId, @surname, @textFailureCount, @title, @type)

    END TRY

    BEGIN CATCH
		EXEC RethrowError;
	END CATCH
    
    SET NOCOUNT OFF
END