SET IDENTITY_INSERT [dbo].[CandidateHistoryEvent] ON
GO

MERGE INTO [dbo].[CandidateHistoryEvent] AS Target 
USING (VALUES 
	(1, N'STA', N'STA', N'Status Change'),
	(2, N'REF', N'REF', N'Referral'),
	(3, N'NTE', N'NTE', N'Note')
) 
AS Source (CandidateHistoryEventId, CodeName, ShortName, FullName) 
ON Target.CandidateHistoryEventId = Source.CandidateHistoryEventId 
-- update matched rows 
WHEN MATCHED THEN 
UPDATE SET CodeName = Source.CodeName, ShortName = Source.ShortName, FullName = Source.FullName
-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN 
INSERT (CandidateHistoryEventId, CodeName, ShortName, FullName) 
VALUES (CandidateHistoryEventId, CodeName, ShortName, FullName) 
-- delete rows that are in the target but not the source 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[CandidateHistoryEvent] OFF
GO