MERGE Vacancy.TrainingType AS dest
USING (SELECT 'F', 'Framework') AS src (TrainingTypeCode, FullName)
ON (dest.TrainingTypeCode = src.TrainingTypeCode)
WHEN MATCHED THEN
	UPDATE SET FullName = src.FullName
WHEN NOT MATCHED THEN
	INSERT (TrainingTypeCode, FullName)
	VALUES (src.TrainingTypeCode, src.FullName)
;

MERGE Vacancy.TrainingType AS dest
USING (SELECT 'S', 'Sector') AS src (TrainingTypeCode, FullName)
ON (dest.TrainingTypeCode = src.TrainingTypeCode)
WHEN MATCHED THEN
	UPDATE SET FullName = src.FullName
WHEN NOT MATCHED THEN
	INSERT (TrainingTypeCode, FullName)
	VALUES (src.TrainingTypeCode, src.FullName)
;

SELECT * FROM Vacancy.TrainingType
