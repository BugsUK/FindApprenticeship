SET IDENTITY_INSERT [dbo].[ApprenticeshipType] ON
GO

MERGE INTO [dbo].[ApprenticeshipType] AS Target 
USING (VALUES 
  (0, 'UNK', 'UNK', 'Unknown', 10),
  (1, 'APP', 'APP', 'Intermediate Level Apprenticeship', 11),
  (2, 'ADV', 'ADV', 'Advanced Level Apprenticeship', 12),
  (3, 'HIG', 'HIG', 'Higher Apprenticeship', 13),
  (4, 'TRA', 'TRA', 'Traineeship', 17),
  (5, 'FDG', 'FDG', 'Foundation Degree', 14),
  (6, 'DEG', 'DEG', 'Degree', 15),
  (7, 'MST', 'MST', 'Masters', 16)
) 

AS Source (ApprenticeshipTypeId, CodeName, ShortName, FullName, EducationLevelId) 
ON Target.ApprenticeshipTypeId = Source.ApprenticeshipTypeId 
-- update matched rows 
WHEN MATCHED THEN 
UPDATE SET CodeName = Source.CodeName, 
	ShortName = Source.ShortName, 
	FullName = Source.FullName,
	EducationLevelId = Source.EducationLevelId
-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN 
INSERT (ApprenticeshipTypeId, CodeName, ShortName, FullName, EducationLevelId) 
VALUES (ApprenticeshipTypeId, CodeName, ShortName, FullName, EducationLevelId) 
-- delete rows that are in the target but not the source 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[ApprenticeshipType] OFF
GO