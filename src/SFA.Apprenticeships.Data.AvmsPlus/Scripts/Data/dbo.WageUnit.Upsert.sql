SET IDENTITY_INSERT [dbo].[WageUnit] ON
GO

MERGE INTO [dbo].[WageUnit] AS Target 
USING (VALUES 
  (1, 'NAP', 'NAP', 'NotApplicable'),
  (2, 'WKY', 'WKY', 'Weekly'),
  (3, 'MTY', 'MTY', 'Monthly'),
  (4, 'ANY', 'ANY', 'Annually')
) 
AS Source (WageUnitId, CodeName, ShortName, FullName) 
ON Target.WageUnitId = Source.WageUnitId 
-- update matched rows 
WHEN MATCHED THEN 
UPDATE SET CodeName = Source.CodeName, ShortName = Source.ShortName, FullName = Source.FullName
-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN 
INSERT (WageUnitId, CodeName, ShortName, FullName) 
VALUES (WageUnitId, CodeName, ShortName, FullName) 
-- delete rows that are in the target but not the source 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[WageUnit] OFF
GO
