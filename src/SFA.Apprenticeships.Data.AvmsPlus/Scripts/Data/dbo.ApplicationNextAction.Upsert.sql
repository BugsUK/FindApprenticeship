SET IDENTITY_INSERT [dbo].[ApplicationNextAction] ON
GO

MERGE INTO [dbo].[ApplicationNextAction] AS Target 
USING (VALUES 
	(0, N'NUL', N'NUL', N'Please select...'),
	(1, N'E2E', N'E2E', N'Referred to e2e'),
	(2, N'ALE', N'ALE', N'Referred to alternative learning opportunities (external)'),
	(3, N'ALI', N'ALI', N'Referred to alternative learning opportunities (internal)'),
	(4, N'CPA', N'CPA', N'Referred to Connexions Personal Advisor'),
	(5, N'NS', N'NS', N'Referred to NextStep'),
	(6, N'JCP', N'JCP', N'Referred to Jobcentre Plus'),
	(7, N'PLA', N'PLA', N'Referred to Programme Led Apprenticeship (PLA)'),
	(8, N'SP', N'SP', N'Referred to specialist provider (e.g. language support)'),
	(9, N'NAR', N'NAR', N'No action required'),
	(10, N'OTH', N'OTH', N'Other')
) 
AS Source (ApplicationNextActionId, CodeName, ShortName, FullName) 
ON Target.ApplicationNextActionId = Source.ApplicationNextActionId 
-- update matched rows 
WHEN MATCHED THEN 
UPDATE SET CodeName = Source.CodeName, ShortName = Source.ShortName, FullName = Source.FullName
-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN 
INSERT (ApplicationNextActionId, CodeName, ShortName, FullName) 
VALUES (ApplicationNextActionId, CodeName, ShortName, FullName) 
-- delete rows that are in the target but not the source 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[ApplicationNextAction] OFF
GO