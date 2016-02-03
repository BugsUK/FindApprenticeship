CREATE PROCEDURE  [dbo].[uspGetAllPersonTitleType]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    
	SELECT * from PersonTitleType
END