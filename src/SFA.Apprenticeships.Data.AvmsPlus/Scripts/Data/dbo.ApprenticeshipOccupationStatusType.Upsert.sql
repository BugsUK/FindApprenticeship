SET IDENTITY_INSERT [dbo].[ApprenticeshipOccupationStatusType] ON
GO

MERGE INTO [dbo].[ApprenticeshipOccupationStatusType] AS Target 
USING (VALUES 
  (1, 'ACT', 'ACT', 'Active'),
  (2, 'CSD', 'CSD', 'Ceased')
) 

AS Source (ApprenticeshipOccupationStatusTypeId, CodeName, ShortName, FullName) 
ON Target.ApprenticeshipOccupationStatusTypeId = Source.ApprenticeshipOccupationStatusTypeId 
-- update matched rows 
WHEN MATCHED THEN 
UPDATE SET CodeName = Source.CodeName, ShortName = Source.ShortName, FullName = Source.FullName
-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN 
INSERT (ApprenticeshipOccupationStatusTypeId, CodeName, ShortName, FullName) 
VALUES (ApprenticeshipOccupationStatusTypeId, CodeName, ShortName, FullName) 
-- delete rows that are in the target but not the source 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[ApprenticeshipOccupationStatusType] OFF
GO
