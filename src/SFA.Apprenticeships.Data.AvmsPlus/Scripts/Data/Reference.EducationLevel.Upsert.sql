SET IDENTITY_INSERT [Reference].[EducationLevel] ON
GO

MERGE INTO [Reference].[EducationLevel] AS Target 
USING (VALUES 
  (10, '0', '0', 'Unknown'),
  (11, '2', '2', 'Intermediate'),
  (12, '3', '3', 'Advanced'),
  (13, '4', '4', 'Higher'),
  (14, '5', '5', 'Foundation Degree'),
  (15, '6', '6', 'Degree'),
  (16, '7', '7', 'Masters'),
  (17, '8', '8', 'Traineeship')

) 
AS Source (EducationLevelId, CodeName, ShortName, FullName) 
ON Target.EducationLevelId = Source.EducationLevelId 
-- update matched rows 
WHEN MATCHED THEN 
UPDATE SET CodeName = Source.CodeName, ShortName = Source.ShortName, FullName = Source.FullName
-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN 
INSERT (EducationLevelId, CodeName, ShortName, FullName) 
VALUES (EducationLevelId, CodeName, ShortName, FullName) 
-- delete rows that are in the target but not the source 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [Reference].[EducationLevel] OFF
GO
