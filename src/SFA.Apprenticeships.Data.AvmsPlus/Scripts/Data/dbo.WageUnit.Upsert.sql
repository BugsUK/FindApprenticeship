SET IDENTITY_INSERT [dbo].[WageUnit] ON
GO

MERGE INTO [dbo].[WageUnit] AS Target 
USING (VALUES 
  (1, 'NotApplicable'),
  (2, 'Weekly'),
  (3, 'Monthly'),
  (4, 'Annually')
) 
AS Source (WageUnitId, WageUnitName) 
ON Target.WageUnitId = Source.WageUnitId 
-- update matched rows 
WHEN MATCHED THEN 
UPDATE SET WageUnitName = Source.WageUnitName
-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN 
INSERT (WageUnitId, WageUnitName) 
VALUES (WageUnitId, WageUnitName) 
-- delete rows that are in the target but not the source 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[WageUnit] OFF
GO
