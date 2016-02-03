CREATE PROCEDURE [dbo].[uspGetALLPIMSOrgTypeBlackList]
AS
	SET NOCOUNT ON;

	SELECT [OrgType]
	FROM [PIMSOrgTypeBlackList]