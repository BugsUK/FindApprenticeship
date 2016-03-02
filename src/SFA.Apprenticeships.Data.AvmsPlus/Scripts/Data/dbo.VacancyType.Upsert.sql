SET IDENTITY_INSERT [dbo].[VacancyType] ON
GO

MERGE INTO [dbo].[VacancyType] AS Target 
USING (VALUES 
  (0, 'UNK', 'UNK', 'Unknown'),
  (1, 'APP', 'APP', 'Apprenticeship'),
  (2, 'TRA', 'TRA', 'Traineeship')
) 
AS Source (VacancyTypeId, CodeName, ShortName, FullName) 
ON Target.VacancyTypeId = Source.VacancyTypeId 
-- update matched rows 
WHEN MATCHED THEN 
UPDATE SET CodeName = Source.CodeName, ShortName = Source.ShortName, FullName = Source.FullName
-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN 
INSERT (VacancyTypeId, CodeName, ShortName, FullName) 
VALUES (VacancyTypeId, CodeName, ShortName, FullName) 
-- delete rows that are in the target but not the source 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[VacancyType] OFF
GO
