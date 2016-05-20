-- =============================================
-- Author:		Matthew Bates
-- Create date: 31st March 2016
-- Description:	Provides the services available for the username and password passed in
-- =============================================
CREATE PROCEDURE [dbo].[uspReturnProviderSystemPermissions] 
	@UserNamePassed uniqueidentifier,
	@PwdPassed varchar(64),
	@StatusId INT OUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DECLARE @PwdFromDB Binary(64)
    DECLARE @EncPassword Binary(64)
    DECLARE @HashedStatusFlag INT
    DECLARE @PwdHashed BINARY(64)
    
    IF NOT (@UserNamePassed IS NULL) AND NOT (@PwdPassed IS NULL)
       BEGIN
           -- Check if username exists i.e. is valid
	       IF EXISTS(SELECT Username FROM ExternalSystemPermission WHERE Username = @UserNamePassed)
	       BEGIN
	         SELECT @PwdFromDB = Password FROM ExternalSystemPermission WHERE Username = @UserNamePassed
	         
	         -- Call encrypt function to encrypt password passed in
	         exec uspHashPwd @PwdPassed, @HashedStatusFlag OUT, @PwdHashed OUT
	   
	         -- If hash has been successful, compare hashed password to password retrieved from DB. Also set status id to 1.
	         -- If failed return a status id of 3
	         IF @HashedStatusFlag = 1
	         BEGIN
	               IF (@PwdHashed = @PwdFromDB)
	               BEGIN
	                  SELECT UserParameters, Businesscategory, Company, Employeetype FROM ExternalSystemPermission WHERE Username = @UserNamePassed
	                  SET @StatusId = 1
	               END
	         ELSE
	                  SET @StatusId = 3
	         END
         END
    ELSE
           -- Set status to 2 to indicate username does not exist
           SET @StatusId = 2
END
END