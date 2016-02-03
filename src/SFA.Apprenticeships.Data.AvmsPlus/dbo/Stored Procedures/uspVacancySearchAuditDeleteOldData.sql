CREATE PROCEDURE [dbo].[uspVacancySearchAuditDeleteOldData]
AS

	DECLARE @NumDaysToKeep INT
	SELECT @NumDaysToKeep = CAST(ParameterValue as INT) FROM SystemParameters WHERE ParameterName = 'MaxAgeAudit'
	
	DELETE FROM VacancySearchAudit
	WHERE SearchDate < DATEADD(DAY, -@NumDaysToKeep, GETDATE())

RETURN 0