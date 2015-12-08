MERGE Vacancy.DurationType AS dest
USING (SELECT 'W', 'Weeks') AS src (DurationTypeCode, FullName)
ON (dest.DurationTypeCode = src.DurationTypeCode)
WHEN MATCHED THEN
	UPDATE SET FullName = src.FullName
WHEN NOT MATCHED THEN
	INSERT (DurationTypeCode, FullName)
	VALUES (src.DurationTypeCode, src.FullName)
;

MERGE Vacancy.DurationType AS dest
USING (SELECT 'M', 'Months') AS src (DurationTypeCode, FullName)
ON (dest.DurationTypeCode = src.DurationTypeCode)
WHEN MATCHED THEN
	UPDATE SET FullName = src.FullName
WHEN NOT MATCHED THEN
	INSERT (DurationTypeCode, FullName)
	VALUES (src.DurationTypeCode, src.FullName)
;

MERGE Vacancy.DurationType AS dest
USING (SELECT 'Y', 'Years') AS src (DurationTypeCode, FullName)
ON (dest.DurationTypeCode = src.DurationTypeCode)
WHEN MATCHED THEN
	UPDATE SET FullName = src.FullName
WHEN NOT MATCHED THEN
	INSERT (DurationTypeCode, FullName)
	VALUES (src.DurationTypeCode, src.FullName)
;

SELECT * FROM Vacancy.DurationType
