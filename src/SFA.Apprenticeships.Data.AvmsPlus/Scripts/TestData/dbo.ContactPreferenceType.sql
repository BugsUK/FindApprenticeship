SET IDENTITY_INSERT [dbo].[ContactPreferenceType] ON
GO

MERGE INTO [dbo].[ContactPreferenceType] AS Target 
USING (VALUES 
  (1, 'EM', 'Email', 'Email') -- Need more values
) 
AS Source (ContactPreferenceTypeId, CodeName, ShortName, FullName) 
ON Target.ContactPreferenceTypeId = Source.ContactPreferenceTypeId 
-- update matched rows 
WHEN MATCHED THEN 
UPDATE SET CodeName = Source.CodeName,
			ShortName = Source.ShortName,
			FullName = Source.FullName
-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN 
INSERT (ContactPreferenceTypeId, CodeName, ShortName, FullName) 
VALUES (ContactPreferenceTypeId, CodeName, ShortName, FullName) 
-- delete rows that are in the target but not the source 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[ContactPreferenceType] OFF
GO


