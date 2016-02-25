SET IDENTITY_INSERT [dbo].[DurationType] ON
GO

MERGE INTO [dbo].[DurationType] AS Target 
USING (VALUES 
  (0, 'Unknown'),
  (1, 'Weeks'),
  (2, 'Months'),
  (3, 'Years')
) 
AS Source (DurationTypeId, FullName) 
ON Target.DurationTypeId = Source.DurationTypeId 
-- update matched rows 
WHEN MATCHED THEN 
UPDATE SET FullName = Source.FullName
-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN 
INSERT (DurationTypeId, FullName) 
VALUES (DurationTypeId, FullName) 
-- delete rows that are in the target but not the source 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[DurationType] OFF
GO