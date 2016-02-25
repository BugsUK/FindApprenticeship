CREATE PROCEDURE [dbo].[uspSICCodeSelectBySICCode]
	@SICCode int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT 
		 [SICCodeId]
		,[Year]
		,[SICCode]
		,[Description]
	FROM [SICCode]
	WHERE [SICCode]=@SICCode
END