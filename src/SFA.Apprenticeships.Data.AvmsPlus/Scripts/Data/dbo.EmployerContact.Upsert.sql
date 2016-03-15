SET IDENTITY_INSERT [dbo].[EmployerContact] ON
GO

MERGE INTO [dbo].[EmployerContact] AS Target 
--Inserting unspecified employer contact records to easily identify missing data
USING (VALUES 
  (-1, -2, 0)
) 
AS Source (EmployerContactId, PersonId, ContactPreferenceTypeId) 
ON Target.EmployerContactId = Source.EmployerContactId 
-- update matched rows 
WHEN MATCHED THEN 
UPDATE SET PersonId = Source.PersonId, ContactPreferenceTypeId = Source.ContactPreferenceTypeId
-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN 
INSERT (EmployerContactId, PersonId, ContactPreferenceTypeId) 
VALUES (EmployerContactId, PersonId, ContactPreferenceTypeId);

SET IDENTITY_INSERT [dbo].[EmployerContact] OFF
GO