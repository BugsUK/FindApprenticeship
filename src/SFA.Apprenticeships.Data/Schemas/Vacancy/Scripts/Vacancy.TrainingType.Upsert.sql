MERGE Vacancy.TrainingType AS dest
USING (
	SELECT 'F', 'Framework' UNION
	SELECT 'S', 'Sector'    UNION
	SELECT 'U', 'Unknown'
) AS src (TrainingTypeCode, FullName)
ON (dest.TrainingTypeCode = src.TrainingTypeCode)
WHEN MATCHED THEN
	UPDATE SET FullName = src.FullName
WHEN NOT MATCHED THEN
	INSERT (TrainingTypeCode, FullName)
	VALUES (src.TrainingTypeCode, src.FullName)
;

SELECT * FROM Vacancy.TrainingType
