CREATE PROCEDURE [dbo].[uspGetTermsAndConditionsById]
@TermsAndConditionsId INT
AS
BEGIN

	SET NOCOUNT ON
	
	SELECT
	[TAC].[TermsAndConditionsId] AS 'TermsAndConditionsId',
	[TAC].FullName AS 'FullName',
	[TAC].[Content] AS 'Content',
	[UT].[FullName] AS 'UserTypeFullName'
	FROM [dbo].[TermsAndConditions] [TAC]
	inner join UserType UT on
	UT.UserTypeID = TAC.UserTypeID
	WHERE TAC.[TermsAndConditionsId]= @TermsAndConditionsId

	SET NOCOUNT OFF
END