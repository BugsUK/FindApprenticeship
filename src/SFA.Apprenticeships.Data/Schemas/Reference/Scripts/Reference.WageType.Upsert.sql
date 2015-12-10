MERGE Vacancy.WageType AS dest
USING (SELECT 'AMW', 'Apprenticeship Minimum Wage') AS src (WageTypeCode, FullName)
ON (dest.WageTypeCode = src.WageTypeCode)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName
WHEN NOT MATCHED THEN
	INSERT (WageTypeCode, FullName)
	VALUES (src.WageTypeCode, src.FullName)
;

MERGE Vacancy.WageType AS dest
USING (SELECT 'NMW', 'National Minimum Wage') AS src (WageTypeCode, FullName)
ON (dest.WageTypeCode = src.WageTypeCode)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName
WHEN NOT MATCHED THEN
	INSERT (WageTypeCode, FullName)
	VALUES (src.WageTypeCode, src.FullName)
;

MERGE Vacancy.WageType AS dest
USING (SELECT 'CUS', 'Custom') AS src (WageTypeCode, FullName)
ON (dest.WageTypeCode = src.WageTypeCode)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName
WHEN NOT MATCHED THEN
	INSERT (WageTypeCode, FullName)
	VALUES (src.WageTypeCode, src.FullName)
;

SELECT * FROM Vacancy.WageType
