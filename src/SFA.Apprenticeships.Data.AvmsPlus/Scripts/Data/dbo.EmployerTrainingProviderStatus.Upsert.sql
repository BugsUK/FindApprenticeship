SET IDENTITY_INSERT [dbo].[EmployerTrainingProviderStatus] ON
GO

MERGE INTO [dbo].[EmployerTrainingProviderStatus] AS Target 
USING (VALUES 
  (1, N'ATV', N'ATV', N'Activated'),
  (2, N'DEL', N'DEL', N'Deleted'),
  (3, N'SUS', N'SUS', N'Suspended')
) 
AS Source (EmployerTrainingProviderStatusId, CodeName, ShortName, FullName)
ON Target.EmployerTrainingProviderStatusId = Source.EmployerTrainingProviderStatusId
-- update matched rows 
WHEN MATCHED THEN 
UPDATE SET CodeName = Source.CodeName, ShortName = Source.ShortName, FullName = Source.FullName
-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN 
INSERT (EmployerTrainingProviderStatusId, CodeName, ShortName, FullName) 
VALUES (EmployerTrainingProviderStatusId, CodeName, ShortName, FullName) 
-- delete rows that are in the target but not the source 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[EmployerTrainingProviderStatus] OFF
GO
