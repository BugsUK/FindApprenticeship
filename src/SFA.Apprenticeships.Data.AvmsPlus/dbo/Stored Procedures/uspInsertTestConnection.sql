CREATE PROCEDURE [dbo].[uspInsertTestConnection]  
	-- Add the parameters for the stored procedure here
	@TestName nvarchar(10)
	
AS
BEGIN

	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO TESTCONNECTION
	VALUES (@TestName)
END




/****** Object:  StoredProcedure [dbo].[uspGetTestConnection]    Script Date: 08/19/2008 09:56:56 ******/
SET ANSI_NULLS ON