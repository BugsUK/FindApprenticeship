SET IDENTITY_INSERT [dbo].[ApprenticeshipType] ON
GO

MERGE INTO [dbo].[ApprenticeshipType] AS Target 
USING (VALUES 
  (0, 'UNK', 'UNK', 'Unknown', 0),
  (1, 'APP', 'APP', 'Intermediate Level Apprenticeship', 2),
  (2, 'ADV', 'ADV', 'Advanced Level Apprenticeship', 3),
  (3, 'HIG', 'HIG', 'Higher Apprenticeship', 4),
  (4, 'TRA', 'TRA', 'Traineeship', 8),
  (5, 'FDG', 'FDG', 'Foundation Degree', 5),
  (6, 'DEG', 'DEG', 'Degree', 6),
  (7, 'MST', 'MST', 'Masters', 7)
) 

AS Source (ApprenticeshipTypeId, CodeName, ShortName, FullName, EducationLevel) 
ON Target.ApprenticeshipTypeId = Source.ApprenticeshipTypeId 
-- update matched rows 
WHEN MATCHED THEN 
UPDATE SET CodeName = Source.CodeName, 
	ShortName = Source.ShortName, 
	FullName = Source.FullName,
	EducationLevel = Source.EducationLevel
-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN 
INSERT (ApprenticeshipTypeId, CodeName, ShortName, FullName, EducationLevel) 
VALUES (ApprenticeshipTypeId, CodeName, ShortName, FullName, EducationLevel) 
-- delete rows that are in the target but not the source 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[ApprenticeshipType] OFF
GO