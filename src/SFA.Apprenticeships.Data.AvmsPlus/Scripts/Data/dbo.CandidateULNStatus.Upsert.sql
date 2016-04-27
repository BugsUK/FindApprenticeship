SET IDENTITY_INSERT [dbo].[CandidateULNStatus] ON
GO

MERGE INTO [dbo].[CandidateULNStatus] AS Target 
USING (VALUES 
	(0, N'', N'', N'Please Select...'),
	(1, N'', N'', N'No match'),
	(2, N'', N'', N'Too many matches'),
	(3, N'', N'', N'Possible match'),
	(4, N'', N'', N'Exact match'),
	(5, N'', N'', N'Master substituted')
) 
AS Source (CandidateULNStatusId, CodeName, ShortName, FullName) 
ON Target.CandidateULNStatusId = Source.CandidateULNStatusId 
-- update matched rows 
WHEN MATCHED THEN 
UPDATE SET CodeName = Source.CodeName, ShortName = Source.ShortName, FullName = Source.FullName
-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN 
INSERT (CandidateULNStatusId, CodeName, ShortName, FullName) 
VALUES (CandidateULNStatusId, CodeName, ShortName, FullName) 
-- delete rows that are in the target but not the source 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[CandidateULNStatus] OFF
GO