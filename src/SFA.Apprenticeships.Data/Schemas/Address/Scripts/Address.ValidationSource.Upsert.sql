MERGE [Address].ValidationSource AS dest
USING (SELECT 'PCA', 'Postcode Anywhere') AS src (ValidationSourceCode, FullName)
ON (dest.ValidationSourceCode = src.ValidationSourceCode)
WHEN MATCHED THEN
	UPDATE SET FullName = src.FullName
WHEN NOT MATCHED THEN
	INSERT (ValidationSourceCode, FullName)
	VALUES (src.ValidationSourceCode, src.FullName)
;

SELECT * FROM [Address].ValidationSource
