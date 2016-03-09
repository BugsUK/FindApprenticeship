IF (SELECT COUNT(*) FROM [Sync].[SyncParams]) = 0
BEGIN
	INSERT INTO [Sync].[SyncParams] (
		[LastSyncVersion]
	)
	VALUES (
		NULL
	)
END
GO
