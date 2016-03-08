CREATE PROCEDURE  [dbo].[uspGetALLCountyAndAlias]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @CountyTemp TABLE(ID int, CodeName nvarchar(3), ShortName nvarchar(50), FullName nvarchar(150), CountyAlias nvarchar(1))

	INSERT INTO @CountyTemp (ID, CodeName, ShortName, FullName, CountyAlias)
		SELECT CountyID, CodeName, ShortName, FullName, '' FROM County WHERE CountyID = 0

	INSERT INTO @CountyTemp (ID, CodeName, ShortName, FullName, CountyAlias)
		SELECT CountyID, CodeName, ShortName, FullName, 'C' FROM County WHERE CountyID > 0 
		UNION
		SELECT LocalAuthorityGroupID
			   ,CodeName
			   ,ShortName
			   ,FullName
			   ,'A' 
		FROM dbo.LocalAuthorityGroup LAG
		INNER JOIN dbo.LocalAuthorityGroupType AS LAGT ON LAG.LocalAuthorityGroupTypeID = LAGT.LocalAuthorityGroupTypeID
		AND   LAGT.LocalAuthorityGroupTypeName = N'Alias'
		ORDER BY FullName
		
	SELECT * FROM @CountyTemp
	
END