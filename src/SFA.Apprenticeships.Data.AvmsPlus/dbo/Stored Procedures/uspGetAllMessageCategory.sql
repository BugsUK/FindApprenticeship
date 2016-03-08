CREATE PROCEDURE  [dbo].[uspGetAllMessageCategory]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    
	SELECT
        *
    FROM
        MessageCategory 
END