MERGE Vacancy.WageInterval AS dest
USING (
	SELECT 'N', 'Not Applicable' UNION
	SELECT 'W', 'Weekly'         UNION
	SELECT 'M', 'Monthly'        UNION
	SELECT 'A', 'Annually'

) AS src (WageIntervalCode, FullName)
ON (dest.WageIntervalCode = src.WageIntervalCode)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName
WHEN NOT MATCHED THEN
	INSERT (WageIntervalCode, FullName)
	VALUES (src.WageIntervalCode, src.FullName)
;

SELECT * FROM Vacancy.WageInterval
