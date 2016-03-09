SET IDENTITY_INSERT [dbo].[WageType] ON
GO

MERGE INTO [dbo].[WageType] AS Target 
USING (VALUES 
  (1, 'AMW', 'AMW', 'ApprenticeshipMinimumWage'),
  (2, 'NMW', 'NMW', 'NationalMinimumWage'),
  (3, 'CUS', 'CUS', 'Custom')
) 
AS Source (WageTypeId, CodeName, ShortName, FullName) 
ON Target.WageTypeId = Source.WageTypeId 
-- update matched rows 
WHEN MATCHED THEN 
UPDATE SET CodeName = Source.CodeName, ShortName = Source.ShortName, FullName = Source.FullName
-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN 
INSERT (WageTypeId, CodeName, ShortName, FullName) 
VALUES (WageTypeId, CodeName, ShortName, FullName) 
-- delete rows that are in the target but not the source 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[WageType] OFF
GO
