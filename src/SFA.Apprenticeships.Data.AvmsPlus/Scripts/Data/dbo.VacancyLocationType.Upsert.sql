SET IDENTITY_INSERT [dbo].[VacancyLocationType] ON
GO

MERGE INTO [dbo].[VacancyLocationType] AS Target 
USING (VALUES 
  (1, 'STD', 'STD', 'Specific Location'),
  (2, 'MUL', 'MUL', 'Multiple Locations'),
  (3, 'NAT', 'NAT', 'Nationwide')
) 
AS Source (VacancyLocationTypeId, CodeName, ShortName, FullName) 
ON Target.VacancyLocationTypeId = Source.VacancyLocationTypeId 
-- update matched rows 
WHEN MATCHED THEN 
UPDATE SET CodeName = Source.CodeName, ShortName = Source.ShortName, FullName = Source.FullName
-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN 
INSERT (VacancyLocationTypeId, CodeName, ShortName, FullName) 
VALUES (VacancyLocationTypeId, CodeName, ShortName, FullName) 
-- delete rows that are in the target but not the source 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[VacancyLocationType] OFF
GO
