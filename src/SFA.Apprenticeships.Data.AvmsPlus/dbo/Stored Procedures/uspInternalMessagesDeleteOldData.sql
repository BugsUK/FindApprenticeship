CREATE PROCEDURE [dbo].[uspInternalMessagesDeleteOldData]
AS

	DECLARE @NumDaysToKeep INT
	SELECT @NumDaysToKeep = CAST(ParameterValue as INT) FROM SystemParameters WHERE ParameterName = 'MaxAgeMessages'
	
	DELETE FROM InternalMessages
	WHERE ReceivedDate < DATEADD(DAY, -@NumDaysToKeep, GETDATE())

RETURN 0