CREATE PROCEDURE [dbo].[uspGetTestConnection]  
	-- Add the parameters for the stored procedure here
	
AS
BEGIN

	SET NOCOUNT ON;

    -- Insert statements for procedure here
	
	select *
	from TestConnection

END