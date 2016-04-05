SET IDENTITY_INSERT [dbo].[Person] ON

MERGE INTO [dbo].[Person] AS TARGET 
USING (VALUES (
	1, -- PersonId
	2, -- PersonTypeId: Employer Contact
	1, -- Title: Mr
	'John', -- FirstName
	'Smith' -- Surname
  )
) 
AS SOURCE (
	PersonId,
	PersonTypeId,
	Title,
	FirstName,
	Surname
) 
ON TARGET.PersonId = SOURCE.PersonId

WHEN MATCHED THEN 
UPDATE SET
	PersonTypeId = SOURCE.PersonTypeId,
	Title = SOURCE.Title,
	FirstName = SOURCE.FirstName,
	Surname = SOURCE.Surname

WHEN NOT MATCHED BY TARGET THEN 
INSERT (
	PersonId,
	PersonTypeId,
	Title,
	FirstName,
	Surname
) 
VALUES (
	PersonId,
	PersonTypeId,
	Title,
	FirstName,
	Surname
) 
;

SELECT * FROM [dbo].[Person]
WHERE PersonId IN (
	1
)

SET IDENTITY_INSERT [dbo].[Person] OFF
