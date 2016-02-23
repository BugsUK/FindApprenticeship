CREATE PROCEDURE  [dbo].[uspGetALLcounty]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- CCR11576 - Added temp table to sort Counties by name APART from the first record that has the id of 0 - i.e. Please Select...
	DECLARE @CountyTemp TABLE(CountyID int, CodeName nvarchar(3), ShortName nvarchar(50), FullName nvarchar(150))

	INSERT INTO @CountyTemp (CountyID, CodeName, ShortName, FullName)
		SELECT * FROM County WHERE CountyID = 0

	INSERT INTO @CountyTemp (CountyID, CodeName, ShortName, FullName)
		SELECT * FROM County WHERE CountyID > 0 ORDER BY FullName

	SELECT * FROM @CountyTemp
	
END