CREATE PROCEDURE [dbo].[uspDeleteTestConnection]  
	-- Add the parameters for the stored procedure here
	@TestName nvarchar(10)
	
AS
BEGIN

	SET NOCOUNT ON;

    -- Insert statements for procedure here
	delete from TestConnection
	where testname = @TestName
END