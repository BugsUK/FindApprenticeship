MERGE Vacancy.VacancyPartyType AS dest
USING (SELECT 'ES', 'Employer Site') AS src (VacancyPartyTypeCode, FullName)
ON (dest.VacancyPartyTypeCode = src.VacancyPartyTypeCode)
WHEN MATCHED THEN
	UPDATE SET FullName = src.FullName
WHEN NOT MATCHED THEN
	INSERT (VacancyPartyTypeCode, FullName)
	VALUES (src.VacancyPartyTypeCode, src.FullName)
;

MERGE Vacancy.VacancyPartyType AS dest
USING (SELECT 'PS', 'Provider Site') AS src (VacancyPartyTypeCode, FullName)
ON (dest.VacancyPartyTypeCode = src.VacancyPartyTypeCode)
WHEN MATCHED THEN
	UPDATE SET FullName = src.FullName
WHEN NOT MATCHED THEN
	INSERT (VacancyPartyTypeCode, FullName)
	VALUES (src.VacancyPartyTypeCode, src.FullName)
;

SELECT * FROM Vacancy.VacancyPartyType
