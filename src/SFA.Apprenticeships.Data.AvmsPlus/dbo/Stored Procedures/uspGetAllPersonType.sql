CREATE PROCEDURE  [dbo].[uspGetAllPersonType]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    
	SELECT * from PersonType
END