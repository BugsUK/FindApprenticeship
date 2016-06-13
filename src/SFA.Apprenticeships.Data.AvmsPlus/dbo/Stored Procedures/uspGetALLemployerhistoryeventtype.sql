CREATE PROCEDURE  [dbo].[uspGetALLemployerhistoryeventtype]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    
	SELECT * from EmployerHistoryEventType
END