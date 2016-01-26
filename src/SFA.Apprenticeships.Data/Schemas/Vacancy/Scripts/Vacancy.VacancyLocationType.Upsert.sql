MERGE Vacancy.VacancyLocationType AS dest
USING (
	SELECT 'E', 'Employer location'                            UNION
	SELECT 'S', 'Specific location (different from employer)'  UNION
	SELECT 'M', 'Multiple locations'                           UNION
	SELECT 'N', 'Nationwide'
) AS src (VacancyLocationTypeCode, FullName)
ON (dest.VacancyLocationTypeCode = src.VacancyLocationTypeCode)
WHEN MATCHED THEN
	UPDATE SET FullName = src.FullName
WHEN NOT MATCHED THEN
	INSERT (VacancyLocationTypeCode, FullName)
	VALUES (src.VacancyLocationTypeCode, src.FullName)
;

SELECT * FROM Vacancy.VacancyLocationType
