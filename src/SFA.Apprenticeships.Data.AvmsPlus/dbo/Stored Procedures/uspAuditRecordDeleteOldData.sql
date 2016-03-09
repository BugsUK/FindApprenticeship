CREATE PROCEDURE [dbo].[uspAuditRecordDeleteOldData]
AS

	DECLARE @NumDaysToKeep INT
	SELECT @NumDaysToKeep = CAST(ParameterValue as INT) FROM SystemParameters WHERE ParameterName = 'MaxAgeAudit'
	
	DELETE FROM AuditRecord
	WHERE ChangeDate < DATEADD(DAY, -@NumDaysToKeep, GETDATE())

RETURN 0