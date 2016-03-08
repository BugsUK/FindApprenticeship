SET IDENTITY_INSERT [dbo].[VacancyHistoryEventType] ON
GO

MERGE INTO [dbo].[VacancyHistoryEventType] AS Target 
USING (VALUES 
  (1, 'STC', 'STC', 'Status Change'),
  (2, 'NTE', 'NTE', 'Note')
) 
AS Source (VacancyHistoryEventTypeId, CodeName, ShortName, FullName) 
ON Target.VacancyHistoryEventTypeId = Source.VacancyHistoryEventTypeId 
-- update matched rows 
WHEN MATCHED THEN 
UPDATE SET CodeName = Source.CodeName, ShortName = Source.ShortName, FullName = Source.FullName
-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN 
INSERT (VacancyHistoryEventTypeId, CodeName, ShortName, FullName) 
VALUES (VacancyHistoryEventTypeId, CodeName, ShortName, FullName) 
-- delete rows that are in the target but not the source 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[VacancyHistoryEventType] OFF
GO
