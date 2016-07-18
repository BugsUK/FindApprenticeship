SET IDENTITY_INSERT [dbo].[VacancySource] ON
GO

MERGE INTO [dbo].[VacancySource] AS Target 
USING (VALUES 
  (0, 'AV', 'AV', 'AVMS'),
  (1, 'API', 'API', 'API'),
  (2, 'RAA', 'RAA', 'RAA')
) 
AS Source (VacancySourceId, CodeName, ShortName, FullName) 
ON Target.VacancySourceId = Source.VacancySourceId 
-- update matched rows 
WHEN MATCHED THEN 
UPDATE SET CodeName = Source.CodeName, ShortName = Source.ShortName, FullName = Source.FullName
-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN 
INSERT (VacancySourceId, CodeName, ShortName, FullName) 
VALUES (VacancySourceId, CodeName, ShortName, FullName) 
-- delete rows that are in the target but not the source 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[VacancySource] OFF
GO
