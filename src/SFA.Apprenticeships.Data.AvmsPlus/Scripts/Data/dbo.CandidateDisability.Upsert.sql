SET IDENTITY_INSERT [dbo].[CandidateDisability] ON
GO

MERGE INTO [dbo].[CandidateDisability] AS Target 
USING (VALUES 
	(0, N'', N'', N'Please Select...'),
	(1, N'', N'', N'Aspergers'' syndrome'),
	(2, N'', N'', N'Disability affecting mobility'),
	(3, N'', N'', N'Emotional/behavioural difficulty'),
	(4, N'', N'', N'Hearing impairment'),
	(5, N'', N'', N'Medical condition'),
	(6, N'', N'', N'Mental health difficulty'),
	(7, N'', N'', N'Multiple disabilities'),
	(8, N'', N'', N'Physical disability'),
	(9, N'', N'', N'Profound complex disabilities'),
	(10, N'', N'', N'Temporary disability after illness or accident'),
	(11, N'', N'', N'Visual impairment'),
	(12, N'', N'', N'Not known/Information not provided'),
	(13, N'', N'', N'Other (please specify)'),
	(14, N'', N'', N'Prefer not to say')
) 
AS Source (CandidateDisabilityId, CodeName, ShortName, FullName) 
ON Target.CandidateDisabilityId = Source.CandidateDisabilityId 
-- update matched rows 
WHEN MATCHED THEN 
UPDATE SET CodeName = Source.CodeName, ShortName = Source.ShortName, FullName = Source.FullName
-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN 
INSERT (CandidateDisabilityId, CodeName, ShortName, FullName) 
VALUES (CandidateDisabilityId, CodeName, ShortName, FullName) 
-- delete rows that are in the target but not the source 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[CandidateDisability] OFF
GO