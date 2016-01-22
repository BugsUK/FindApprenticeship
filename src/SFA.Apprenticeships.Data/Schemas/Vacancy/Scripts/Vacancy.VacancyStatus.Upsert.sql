MERGE Vacancy.VacancyStatus AS dest
USING (
	SELECT 'CLD', 'Closed'           UNION
	SELECT 'DRA', 'Draft'            UNION
	SELECT 'PQA', 'Pending QA'       UNION
	SELECT 'LIV', 'Live'             UNION
	SELECT 'RES', 'Reserved for QA'  UNION
	SELECT 'PAR', 'Parent vacancy'   UNION
	SELECT 'REF', 'Referred by QA'   UNION
	SELECT 'UNK', 'Unknown'
) AS src (VacancyStatusCode, FullName)
ON (dest.VacancyStatusCode = src.VacancyStatusCode)
WHEN MATCHED THEN
	UPDATE SET FullName = src.FullName
WHEN NOT MATCHED THEN
	INSERT (VacancyStatusCode, FullName)
	VALUES (src.VacancyStatusCode, src.FullName)
;

SELECT * FROM Vacancy.VacancyStatus
