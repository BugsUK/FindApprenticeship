SET IDENTITY_INSERT [dbo].[EmployerContact] ON

MERGE INTO [dbo].[EmployerContact] AS TARGET 
USING (VALUES (
	1, -- EmployerContactId
	1, -- PersonId
	1 -- ContactPreferenceTypeId: Email
  )
) 
AS SOURCE (
	EmployerContactId,
	PersonId,
	ContactPreferenceTypeId
) 
ON TARGET.PersonId = SOURCE.PersonId

WHEN MATCHED THEN 
UPDATE SET
	PersonId = SOURCE.PersonId,
	ContactPreferenceTypeId = SOURCE.ContactPreferenceTypeId
WHEN NOT MATCHED BY TARGET THEN 
INSERT (
	EmployerContactId,
	PersonId,
	ContactPreferenceTypeId
) 
VALUES (
	EmployerContactId,
	PersonId,
	ContactPreferenceTypeId
) 
;

SELECT * FROM [dbo].[EmployerContact]
WHERE EmployerContactId IN (
	1
)

SET IDENTITY_INSERT [dbo].[EmployerContact] OFF
