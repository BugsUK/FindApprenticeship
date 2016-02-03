CREATE PROCEDURE  [dbo].[uspGetALLGeographicalRegions]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

   SELECT [LocalAuthorityGroupID]
      ,[CodeName]
      ,[ShortName]
      ,[FullName]
      ,[LocalAuthorityGroupTypeID]
      ,[LocalAuthorityGroupPurposeID]
      ,[ParentLocalAuthorityGroupID]
  FROM [LocalAuthorityGroup] LAG
  WHERE [LocalAuthorityGroupTypeID] = 4
  ORDER BY CASE WHEN FullName = 'Unspecified' THEN 0 ELSE 1 END, FullName
END