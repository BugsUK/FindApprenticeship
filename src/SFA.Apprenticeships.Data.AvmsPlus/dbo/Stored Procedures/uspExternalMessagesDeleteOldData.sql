CREATE PROCEDURE [dbo].[uspExternalMessagesDeleteOldData]
AS

	DECLARE @NumDaysToKeep INT
	SELECT @NumDaysToKeep = CAST(ParameterValue as INT) FROM SystemParameters WHERE ParameterName = 'MaxAgeMessages'
	
	DELETE FROM ExternalMessages
	WHERE ReceivedDate < DATEADD(DAY, -@NumDaysToKeep, GETDATE())

RETURN 0