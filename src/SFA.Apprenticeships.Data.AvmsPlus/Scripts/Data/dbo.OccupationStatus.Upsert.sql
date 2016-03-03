MERGE Reference.OccupationStatus AS dest
USING (SELECT '1', 'ACT', 'ACT', 'Active') AS src (OccupationStatusId, CodeName, ShortName, FullName)
ON (dest.OccupationStatusId = src.OccupationStatusId)
WHEN MATCHED THEN 
	UPDATE SET CodeName = src.CodeName, ShortName = src.ShortName, FullName = src.FullName
WHEN NOT MATCHED THEN
	INSERT (OccupationStatusId, CodeName, ShortName, FullName)
	VALUES (src.OccupationStatusId, src.CodeName, src.ShortName, src.FullName)
;

MERGE Reference.OccupationStatus AS dest
USING (SELECT '2', 'CSD', 'CSD', 'Closed') AS src (OccupationStatusId, CodeName, ShortName, FullName)
ON (dest.OccupationStatusId = src.OccupationStatusId)
WHEN MATCHED THEN 
	UPDATE SET CodeName = src.CodeName, ShortName = src.ShortName, FullName = src.FullName
WHEN NOT MATCHED THEN
	INSERT (OccupationStatusId, CodeName, ShortName, FullName)
	VALUES (src.OccupationStatusId, src.CodeName, src.ShortName, src.FullName)
;

SELECT * FROM Reference.OccupationStatus
