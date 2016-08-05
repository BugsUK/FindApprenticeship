SET IDENTITY_INSERT [dbo].[ApplicationStatusType] ON
GO

MERGE INTO [dbo].[ApplicationStatusType] AS Target 
USING (VALUES 
	(0, N'SAV', N'SAV', N'Saved'),
	(1, N'DRF', N'DRF', N'Unsent'),
	(2, N'NEW', N'NEW', N'Sent'),
	(3, N'APP', N'APP', N'In progress'),
	(4, N'WTD', N'WTD', N'Withdrawn'),
	(5, N'REJ', N'REJ', N'Unsuccessful'),
	(6, N'SUC', N'SUC', N'Successful'),
	(7, N'PST', N'PST', N'Past Application')
) 
AS Source (ApplicationStatusTypeId, CodeName, ShortName, FullName) 
ON Target.ApplicationStatusTypeId = Source.ApplicationStatusTypeId 
-- update matched rows 
WHEN MATCHED THEN 
UPDATE SET CodeName = Source.CodeName, ShortName = Source.ShortName, FullName = Source.FullName
-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN 
INSERT (ApplicationStatusTypeId, CodeName, ShortName, FullName) 
VALUES (ApplicationStatusTypeId, CodeName, ShortName, FullName) 
-- delete rows that are in the target but not the source 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[ApplicationStatusType] OFF
GO