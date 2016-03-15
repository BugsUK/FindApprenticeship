SET IDENTITY_INSERT [dbo].[ContactPreferenceType] ON
GO

MERGE INTO [dbo].[ContactPreferenceType] AS TARGET 
USING (VALUES 
  (0, N'', N'', N'Please Select...'),
  (1, N'', N'', N'Email'),
  (2, N'', N'', N'Mail'),
  (3, N'', N'', N'Phone')
) 
AS SOURCE (ContactPreferenceTypeId, CodeName, ShortName, FullName) 
ON TARGET.ContactPreferenceTypeId = SOURCE.ContactPreferenceTypeId 

WHEN MATCHED THEN 
UPDATE SET CodeName = SOURCE.CodeName, ShortName = SOURCE.ShortName, FullName = SOURCE.FullName

WHEN NOT MATCHED BY TARGET THEN 
INSERT (ContactPreferenceTypeId, CodeName, ShortName, FullName) 
VALUES (ContactPreferenceTypeId, CodeName, ShortName, FullName) 

WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[ContactPreferenceType] OFF
GO
