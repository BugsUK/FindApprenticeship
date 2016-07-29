SET IDENTITY_INSERT [dbo].[CandidateStatus] ON
GO

MERGE INTO [dbo].[CandidateStatus] AS Target 
USING (VALUES 
	(1, N'PRT', N'PRT', N'Pre-Registered'),
	(2, N'ATV', N'ATV', N'Activated'),
	(3, N'REG', N'REG', N'Registered'),
	(4, N'SUS', N'SUS', N'Suspended'),
	(5, N'PDL', N'PDL', N'Pending Delete'),
	(6, N'DEL', N'DEL', N'Deleted')
) 
AS Source (CandidateStatusId, CodeName, ShortName, FullName) 
ON Target.CandidateStatusId = Source.CandidateStatusId 
-- update matched rows 
WHEN MATCHED THEN 
UPDATE SET CodeName = Source.CodeName, ShortName = Source.ShortName, FullName = Source.FullName
-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN 
INSERT (CandidateStatusId, CodeName, ShortName, FullName) 
VALUES (CandidateStatusId, CodeName, ShortName, FullName) 
-- delete rows that are in the target but not the source 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[CandidateStatus] OFF
GO