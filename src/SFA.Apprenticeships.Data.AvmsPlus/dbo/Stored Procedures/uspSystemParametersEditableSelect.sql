CREATE PROCEDURE [dbo].[uspSystemParametersEditableSelect]

AS
BEGIN
	SET NOCOUNT ON
	
	SELECT SystemParametersId,
		   ParameterName,
		   ParameterType,
		   ParameterValue,
		   Editable,
		   LowerLimit,
		   UpperLimit,
		   Description
	FROM [dbo].[SystemParameters]
	WHERE Editable = 1
	
	SET NOCOUNT OFF
END