SET IDENTITY_INSERT [dbo].[ApprenticeshipFrameworkStatusType] ON
GO

MERGE INTO [dbo].[ApprenticeshipFrameworkStatusType] AS Target 
USING (VALUES 
  (1, 'ACT', 'ACT', 'Active'),
  (2, 'CSD', 'CSD', 'Ceased'),
  (3, 'PDC', 'PDC', 'Pending Closure')
) 
AS Source (ApprenticeshipFrameworkStatusTypeId, CodeName, ShortName, FullName) 
ON Target.ApprenticeshipFrameworkStatusTypeId = Source.ApprenticeshipFrameworkStatusTypeId 
-- update matched rows 
WHEN MATCHED THEN 
UPDATE SET CodeName = Source.CodeName, ShortName = Source.ShortName, FullName = Source.FullName
-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN 
INSERT (ApprenticeshipFrameworkStatusTypeId, CodeName, ShortName, FullName) 
VALUES (ApprenticeshipFrameworkStatusTypeId, CodeName, ShortName, FullName) 
-- delete rows that are in the target but not the source 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[ApprenticeshipFrameworkStatusType] OFF
GO