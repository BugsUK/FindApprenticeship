MERGE Reference.[Level] AS dest
USING (
	SELECT '0', 'Unknown'           UNION -- TODO: Confirm
	SELECT '2', 'Intermediate'      UNION
	SELECT '3', 'Advanced'          UNION
	SELECT '4', 'Higher'            UNION
	SELECT '5', 'Foundation Degree' UNION
	SELECT '6', 'Degree'            UNION
	SELECT '7', 'Masters'
) AS src (LevelCode, FullName)
ON (dest.LevelCode = src.LevelCode)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName
WHEN NOT MATCHED THEN
	INSERT (LevelCode, FullName)
	VALUES (src.LevelCode, src.FullName)
;

SELECT * FROM Reference.[Level]
