MERGE Reference.FrameworkStatus AS dest
USING (SELECT 1, 'ACT', 'ACT', 'Active') AS src (FrameworkStatusId, CodeName, ShortName, FullName)
ON (dest.FrameworkStatusId = src.FrameworkStatusId)
WHEN MATCHED THEN 
	UPDATE SET CodeName = src.CodeName, ShortName = src.ShortName, FullName = src.FullName
WHEN NOT MATCHED THEN
	INSERT (FrameworkStatusId, CodeName, ShortName, FullName)
	VALUES (src.FrameworkStatusId, src.CodeName, src.ShortName, src.FullName)
;

MERGE Reference.FrameworkStatus AS dest
USING (SELECT 2, 'CSD', 'CSD', 'Ceased') AS src (FrameworkStatusId, CodeName, ShortName, FullName)
ON (dest.FrameworkStatusId = src.FrameworkStatusId)
WHEN MATCHED THEN 
	UPDATE SET CodeName = src.CodeName, ShortName = src.ShortName, FullName = src.FullName
WHEN NOT MATCHED THEN
	INSERT (FrameworkStatusId, CodeName, ShortName, FullName)
	VALUES (src.FrameworkStatusId, src.CodeName, src.ShortName, src.FullName)
;

MERGE Reference.FrameworkStatus AS dest
USING (SELECT 3, 'PDC', 'PDC', 'Pending Closure') AS src (FrameworkStatusId, CodeName, ShortName, FullName)
ON (dest.FrameworkStatusId = src.FrameworkStatusId)
WHEN MATCHED THEN 
	UPDATE SET CodeName = src.CodeName, ShortName = src.ShortName, FullName = src.FullName
WHEN NOT MATCHED THEN
	INSERT (FrameworkStatusId, CodeName, ShortName, FullName)
	VALUES (src.FrameworkStatusId, src.CodeName, src.ShortName, src.FullName)
;

SELECT * FROM Reference.FrameworkStatus
