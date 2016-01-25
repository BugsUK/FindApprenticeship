MERGE Vacancy.VacancyType AS dest
USING (SELECT 'A', 'Apprenticeship') AS src (VacancyTypeCode, FullName)
ON (dest.VacancyTypeCode = src.VacancyTypeCode)
WHEN MATCHED THEN
	UPDATE SET FullName = src.FullName
WHEN NOT MATCHED THEN
	INSERT (VacancyTypeCode, FullName)
	VALUES (src.VacancyTypeCode, src.FullName)
;

MERGE Vacancy.VacancyType AS dest
USING (SELECT 'T', 'Traineeship') AS src (VacancyTypeCode, FullName)
ON (dest.VacancyTypeCode = src.VacancyTypeCode)
WHEN MATCHED THEN
	UPDATE SET FullName = src.FullName
WHEN NOT MATCHED THEN
	INSERT (VacancyTypeCode, FullName)
	VALUES (src.VacancyTypeCode, src.FullName)
;

SELECT * FROM Vacancy.VacancyType
