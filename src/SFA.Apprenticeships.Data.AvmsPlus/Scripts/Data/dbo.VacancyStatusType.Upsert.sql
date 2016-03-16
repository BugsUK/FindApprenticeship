SET IDENTITY_INSERT [dbo].[VacancyStatusType] ON
GO

MERGE INTO [dbo].[VacancyStatusType] AS Target 
USING (VALUES 
	(0, 'UNK', 'Unknown', 'Unknown'),
	(1, 'DFT', 'Draft', 'Draft'),
	(2, 'Lve', 'Live',	'Live'),
	(3, 'Ref', 'Referred', 'Referred'),
	(4, 'Del', 'Deleted', 'Deleted'),
	(5, 'Sub', 'Sub', 'Submitted'),
	(6, 'Cld', 'Cld', 'Closed'),
	(7, 'Wdr', 'Wdr', 'Withdrawn'),
	(8, 'Com', 'Com', 'Completed'),
	(9, 'Pie', 'Pie', 'Posted In Error'),
	(10, 'Pqa', 'Pqa', 'PendingQA'),
	(11,'Rqa', 'Rqa', 'ReservedForQA'),
	(12, 'Pva', 'Pva', 'ParentVacancy')
) 
AS Source (VacancyStatusTypeId, CodeName, ShortName, FullName) 
ON Target.VacancyStatusTypeId = Source.VacancyStatusTypeId 
-- update matched rows 
WHEN MATCHED THEN 
UPDATE SET CodeName = Source.CodeName, ShortName = Source.ShortName, FullName = Source.FullName
-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN 
INSERT (VacancyStatusTypeId, CodeName, ShortName, FullName) 
VALUES (VacancyStatusTypeId, CodeName, ShortName, FullName) 
-- delete rows that are in the target but not the source 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[VacancyStatusType] OFF
GO
