CREATE PROCEDURE [dbo].[uspGetTermsAndConditionsByFullName]
@TermsAndConditionsFullName VARCHAR (200)
AS
BEGIN

	SET NOCOUNT ON
	
	SELECT
	[TAC].[TermsAndConditionsId] AS 'TermsAndConditionsId',
	[TAC].FullName AS 'FullName',
	[TAC].[Content] AS 'Content'
	FROM [dbo].[TermsAndConditions] [TAC]
	WHERE TAC.[FullName]= @TermsAndConditionsFullName

	SET NOCOUNT OFF
END