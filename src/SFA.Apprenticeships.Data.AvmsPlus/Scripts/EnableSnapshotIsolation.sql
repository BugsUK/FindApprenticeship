BEGIN
	DECLARE @AllowSnapshotIsolation    BIT;
	DECLARE @IsReadCommittedSnapshotOn BIT;
	DECLARE @Command                   VARCHAR(MAX);

	RAISERROR ('Checking snapshot isolation state', 0, 1) WITH NOWAIT
	SELECT @AllowSnapshotIsolation    = CASE snapshot_isolation_state_desc WHEN 'ON' THEN 1 ELSE 0 END,
	       @IsReadCommittedSnapshotOn = is_read_committed_snapshot_on
	FROM   sys.databases 
	WHERE  name = DB_NAME();

	IF @AllowSnapshotIsolation = 0
	BEGIN
		SET @Command = 'ALTER DATABASE [' + DB_NAME() + '] SET ALLOW_SNAPSHOT_ISOLATION ON';
		RAISERROR (@Command, 0, 1) WITH NOWAIT
		EXEC(@Command);
	END;

	IF @IsReadCommittedSnapshotOn = 0
	BEGIN
		SET @Command = 'ALTER DATABASE [' + DB_NAME() + '] SET READ_COMMITTED_SNAPSHOT ON';
		RAISERROR (@Command, 0, 1) WITH NOWAIT
		EXEC(@Command);
	END;

	RAISERROR ('Done', 0, 1) WITH NOWAIT
END