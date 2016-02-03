Create PROCEDURE  [dbo].[uspGetUniqueKeyRegister]
	@KeyValue nvarchar(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;    
	SELECT count(1) as KeyCount from UniqueKeyRegister Where 
	KeyValue=@KeyValue
END