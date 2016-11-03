SET IDENTITY_INSERT [dbo].[WageType] ON
GO

MERGE INTO [dbo].[WageType] AS Target 
USING (VALUES 
  (0, 'LET', 'LET', 'Legacy Text Wage'),
  (1, 'LEW', 'LEW', 'Legacy Weekly Wage'),
  (2, 'AMW', 'AMW', 'Apprenticeship Minimum Wage'),
  (3, 'NMW', 'NMW', 'National Minimum Wage'),
  (4, 'CUS', 'CUS', 'Custom Wage'),
  (5, 'CWR', 'CWR', 'Custom Wage Range'),
  (6, 'CPT', 'CPT', 'Competitive salary'),
  (7, 'TBA', 'TBA', 'To be agreed upon appointment'),
  (8, 'UNW', 'UNW', 'Unwaged')
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
