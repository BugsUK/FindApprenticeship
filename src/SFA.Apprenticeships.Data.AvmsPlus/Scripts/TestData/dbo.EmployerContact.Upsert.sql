SET IDENTITY_INSERT [dbo].[EmployerContact] ON
GO

MERGE INTO [dbo].[EmployerContact] AS Target 
USING (VALUES 
  (1, 1, 1)
) 
AS Source (EmployerContactId, PersonId, ContactPreferenceTypeId) 
ON Target.EmployerContactId = Source.EmployerContactId 
-- update matched rows 
WHEN MATCHED THEN 
UPDATE SET ContactPreferenceTypeId = Source.ContactPreferenceTypeId,
			PersonId = Source.PersonId
-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN 
INSERT (EmployerContactId, PersonId, ContactPreferenceTypeId) 
VALUES (EmployerContactId, PersonId, ContactPreferenceTypeId) 
-- delete rows that are in the target but not the source 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[EmployerContact] OFF
GO