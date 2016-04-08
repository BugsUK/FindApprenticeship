SET IDENTITY_INSERT [dbo].[ApplicationHistoryEvent] ON
GO

MERGE INTO [dbo].[ApplicationHistoryEvent] AS Target 
USING (VALUES 
	(1, N'STC', N'STC', N'Status Change'),
	(2, N'NTE', N'NTE', N'Note')
) 
AS Source (ApplicationHistoryEventId, CodeName, ShortName, FullName) 
ON Target.ApplicationHistoryEventId = Source.ApplicationHistoryEventId 
-- update matched rows 
WHEN MATCHED THEN 
UPDATE SET CodeName = Source.CodeName, ShortName = Source.ShortName, FullName = Source.FullName
-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN 
INSERT (ApplicationHistoryEventId, CodeName, ShortName, FullName) 
VALUES (ApplicationHistoryEventId, CodeName, ShortName, FullName) 
-- delete rows that are in the target but not the source 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[ApplicationHistoryEvent] OFF
GO