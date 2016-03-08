SET IDENTITY_INSERT [dbo].[VacancyTextFieldValue] ON
GO

MERGE INTO [dbo].[VacancyTextFieldValue] AS Target 
USING (VALUES 
  (1, 'TBP', 'TBP', 'Training to be provided'),
  (2, 'QR', 'QR', 'Qualifications Required'),
  (3, 'SR', 'SR', 'Skills Required'),
  (4, 'PQ', 'PQ', 'Personal Qualities'),
  (5, 'OII', 'OII', 'Other important information'),
  (6, 'FP', 'FP', 'Future Prospects'),
  (7, 'RC', 'RC', 'Reality Check')
) 
AS Source (VacancyTextFieldValueId, CodeName, ShortName, FullName) 
ON Target.VacancyTextFieldValueId = Source.VacancyTextFieldValueId 
-- update matched rows 
WHEN MATCHED THEN 
UPDATE SET CodeName = Source.CodeName, ShortName = Source.ShortName, FullName = Source.FullName
-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN 
INSERT (VacancyTextFieldValueId, CodeName, ShortName, FullName) 
VALUES (VacancyTextFieldValueId, CodeName, ShortName, FullName) 
-- delete rows that are in the target but not the source 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[VacancyTextFieldValue] OFF
GO
