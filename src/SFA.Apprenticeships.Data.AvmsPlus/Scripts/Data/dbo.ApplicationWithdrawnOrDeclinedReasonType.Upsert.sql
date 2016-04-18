SET IDENTITY_INSERT [dbo].[ApplicationWithdrawnOrDeclinedReasonType] ON
GO

MERGE INTO [dbo].[ApplicationWithdrawnOrDeclinedReasonType] AS Target 
USING (VALUES 
	(0, N'', N'', N'Please Select...'),
	(1, N'NLI', N'NLI', N'No longer interested'),
	(2, N'AAO', N'AAO', N'Accepted another Apprenticeship offer'),
	(3, N'AAJ', N'AAJ', N'Accepted an alternative job'),
	(4, N'DGC', N'DGC', N'Decided to go to college'),
	(5, N'D6F', N'D6F', N'Decided to stay on at 6th form'),
	(6, N'AOA', N'AOA', N'Want to be able to apply for other Apprenticeships'),
	(7, N'PRS', N'PRS', N'Personal reasons'),
	(8, N'MAY', N'MAY', N'Moving away'),
	(9, N'POC', N'POC', N'Pay or Conditions not acceptable'),
	(10, N'OPS', N'OPS', N'Other (please specify)')
) 
AS Source (ApplicationWithdrawnOrDeclinedReasonTypeId, CodeName, ShortName, FullName) 
ON Target.ApplicationWithdrawnOrDeclinedReasonTypeId = Source.ApplicationWithdrawnOrDeclinedReasonTypeId 
-- update matched rows 
WHEN MATCHED THEN 
UPDATE SET CodeName = Source.CodeName, ShortName = Source.ShortName, FullName = Source.FullName
-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN 
INSERT (ApplicationWithdrawnOrDeclinedReasonTypeId, CodeName, ShortName, FullName) 
VALUES (ApplicationWithdrawnOrDeclinedReasonTypeId, CodeName, ShortName, FullName) 
-- delete rows that are in the target but not the source 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[ApplicationWithdrawnOrDeclinedReasonType] OFF
GO