MERGE Reference.[Level] AS dest
USING (SELECT '2', 'Intermediate') AS src (LevelCode, FullName)
ON (dest.LevelCode = src.LevelCode)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName
WHEN NOT MATCHED THEN
	INSERT (LevelCode, FullName)
	VALUES (src.LevelCode, src.FullName)
;

MERGE Reference.[Level] AS dest
USING (SELECT '3', 'Advanced') AS src (LevelCode, FullName)
ON (dest.LevelCode = src.LevelCode)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName
WHEN NOT MATCHED THEN
	INSERT (LevelCode, FullName)
	VALUES (src.LevelCode, src.FullName)
;

MERGE Reference.[Level] AS dest
USING (SELECT '4', 'Higher') AS src (LevelCode, FullName)
ON (dest.LevelCode = src.LevelCode)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName
WHEN NOT MATCHED THEN
	INSERT (LevelCode, FullName)
	VALUES (src.LevelCode, src.FullName)
;

MERGE Reference.[Level] AS dest
USING (SELECT '5', 'Foundation Degree') AS src (LevelCode, FullName)
ON (dest.LevelCode = src.LevelCode)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName
WHEN NOT MATCHED THEN
	INSERT (LevelCode, FullName)
	VALUES (src.LevelCode, src.FullName)
;

MERGE Reference.[Level] AS dest
USING (SELECT '6', 'Degree') AS src (LevelCode, FullName)
ON (dest.LevelCode = src.LevelCode)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName
WHEN NOT MATCHED THEN
	INSERT (LevelCode, FullName)
	VALUES (src.LevelCode, src.FullName)
;

MERGE Reference.[Level] AS dest
USING (SELECT '7', 'Masters') AS src (LevelCode, FullName)
ON (dest.LevelCode = src.LevelCode)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName
WHEN NOT MATCHED THEN
	INSERT (LevelCode, FullName)
	VALUES (src.LevelCode, src.FullName)
;

SELECT * FROM Reference.[Level]
