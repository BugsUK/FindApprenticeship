-- =============================================
-- Author:		Matthew Bates
-- Create date: 31st March 2016
-- Description:	Hash the non encrypted password passed in and return encrypted password
-- =============================================
CREATE PROCEDURE [dbo].[uspHashPwd]
	@Pwd varchar(64),
	@HashedStatusFlag INT OUT,
	@PwdHashed Binary(64) OUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- HASH Password passed in
    BEGIN TRY
       SET @PwdHashed=(HASHBYTES('SHA1', @Pwd))
       SET @HashedStatusFlag = 1
	END TRY
	BEGIN CATCH
	-- Status of failed, unable to hash the password provided
	   SET @HashedStatusFlag = 2
	END CATCH
END