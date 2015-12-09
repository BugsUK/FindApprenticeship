MERGE Vacancy.VacancyStatus AS dest
USING (SELECT 'DRA', 'Draft') AS src (VacancyStatusCode, FullName)
ON (dest.VacancyStatusCode = src.VacancyStatusCode)
WHEN MATCHED THEN
	UPDATE SET FullName = src.FullName
WHEN NOT MATCHED THEN
	INSERT (VacancyStatusCode, FullName)
	VALUES (src.VacancyStatusCode, src.FullName)
;

MERGE Vacancy.VacancyStatus AS dest
USING (SELECT 'PEN', 'Pending QA') AS src (VacancyStatusCode, FullName)
ON (dest.VacancyStatusCode = src.VacancyStatusCode)
WHEN MATCHED THEN
	UPDATE SET FullName = src.FullName
WHEN NOT MATCHED THEN
	INSERT (VacancyStatusCode, FullName)
	VALUES (src.VacancyStatusCode, src.FullName)
;

MERGE Vacancy.VacancyStatus AS dest
USING (SELECT 'LIV', 'Live') AS src (VacancyStatusCode, FullName)
ON (dest.VacancyStatusCode = src.VacancyStatusCode)
WHEN MATCHED THEN
	UPDATE SET FullName = src.FullName
WHEN NOT MATCHED THEN
	INSERT (VacancyStatusCode, FullName)
	VALUES (src.VacancyStatusCode, src.FullName)
;

MERGE Vacancy.VacancyStatus AS dest
USING (SELECT 'RES', 'Reserved for QA') AS src (VacancyStatusCode, FullName)
ON (dest.VacancyStatusCode = src.VacancyStatusCode)
WHEN MATCHED THEN
	UPDATE SET FullName = src.FullName
WHEN NOT MATCHED THEN
	INSERT (VacancyStatusCode, FullName)
	VALUES (src.VacancyStatusCode, src.FullName)
;

MERGE Vacancy.VacancyStatus AS dest
USING (SELECT 'REF', 'Referred by QA') AS src (VacancyStatusCode, FullName)
ON (dest.VacancyStatusCode = src.VacancyStatusCode)
WHEN MATCHED THEN
	UPDATE SET FullName = src.FullName
WHEN NOT MATCHED THEN
	INSERT (VacancyStatusCode, FullName)
	VALUES (src.VacancyStatusCode, src.FullName)
;

SELECT * FROM Vacancy.VacancyStatus
