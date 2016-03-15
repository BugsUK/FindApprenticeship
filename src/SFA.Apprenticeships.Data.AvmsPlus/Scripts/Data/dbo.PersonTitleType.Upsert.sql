SET IDENTITY_INSERT [dbo].[PersonTitleType] ON
GO

MERGE INTO [dbo].[PersonTitleType] AS TARGET 
USING (VALUES 
  (0, N'', N'', N'Please Select'),
  (1, N'', N'', N'Mr'),
  (2, N'', N'', N'Ms'),
  (3, N'', N'', N'Miss'),
  (4, N'', N'', N'Mrs'),
  (5, N'', N'', N'Master'),
  (6, N'', N'', N'Other (please specify)')
) 
AS SOURCE (PersonTitleTypeId, CodeName, ShortName, FullName) 
ON TARGET.PersonTitleTypeId = SOURCE.PersonTitleTypeId 

WHEN MATCHED THEN 
UPDATE SET CodeName = SOURCE.CodeName, ShortName = SOURCE.ShortName, FullName = SOURCE.FullName

WHEN NOT MATCHED BY TARGET THEN 
INSERT (PersonTitleTypeId, CodeName, ShortName, FullName) 
VALUES (PersonTitleTypeId, CodeName, ShortName, FullName) 

WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SELECT * FROM [dbo].[PersonTitleType]

SET IDENTITY_INSERT [dbo].[PersonTitleType] OFF
GO
