MERGE Vacancy.VacancyPartyRelationshipType AS dest
USING (SELECT 'C', 'Contractual') AS src (VacancyPartyRelationshipTypeCode, FullName)
ON (dest.VacancyPartyRelationshipTypeCode = src.VacancyPartyRelationshipTypeCode)
WHEN MATCHED THEN
	UPDATE SET FullName = src.FullName
WHEN NOT MATCHED THEN
	INSERT (VacancyPartyRelationshipTypeCode, FullName)
	VALUES (src.VacancyPartyRelationshipTypeCode, src.FullName)
;

SELECT * FROM Vacancy.VacancyPartyRelationshipType
