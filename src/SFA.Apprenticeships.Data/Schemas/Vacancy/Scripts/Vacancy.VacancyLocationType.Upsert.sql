MERGE Vacancy.VacancyLocationType AS dest
USING (SELECT 'S', 'Specific location') AS src (VacancyLocationTypeCode, FullName)
ON (dest.VacancyLocationTypeCode = src.VacancyLocationTypeCode)
WHEN MATCHED THEN
	UPDATE SET FullName = src.FullName
WHEN NOT MATCHED THEN
	INSERT (VacancyLocationTypeCode, FullName)
	VALUES (src.VacancyLocationTypeCode, src.FullName)
;

MERGE Vacancy.VacancyLocationType AS dest
USING (SELECT 'M', 'Multiple locations') AS src (VacancyLocationTypeCode, FullName)
ON (dest.VacancyLocationTypeCode = src.VacancyLocationTypeCode)
WHEN MATCHED THEN
	UPDATE SET FullName = src.FullName
WHEN NOT MATCHED THEN
	INSERT (VacancyLocationTypeCode, FullName)
	VALUES (src.VacancyLocationTypeCode, src.FullName)
;

MERGE Vacancy.VacancyLocationType AS dest
USING (SELECT 'N', 'Nationwide') AS src (VacancyLocationTypeCode, FullName)
ON (dest.VacancyLocationTypeCode = src.VacancyLocationTypeCode)
WHEN MATCHED THEN
	UPDATE SET FullName = src.FullName
WHEN NOT MATCHED THEN
	INSERT (VacancyLocationTypeCode, FullName)
	VALUES (src.VacancyLocationTypeCode, src.FullName)
;

SELECT * FROM Vacancy.VacancyLocationType
