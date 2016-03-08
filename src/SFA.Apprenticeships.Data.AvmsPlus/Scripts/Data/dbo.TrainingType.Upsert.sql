SET IDENTITY_INSERT [dbo].[TrainingType] ON
GO

MERGE INTO [dbo].[TrainingType] AS Target 
USING (VALUES 
  (0, 'UNK', 'UNK', 'Unknown'),
  (1, 'FWS', 'FWS', 'Frameworks'),
  (2, 'STD', 'STD', 'Standards')
) 

AS Source (TrainingTypeId, CodeName, ShortName, FullName) 
ON Target.TrainingTypeId = Source.TrainingTypeId 
-- update matched rows 
WHEN MATCHED THEN 
UPDATE SET CodeName = Source.CodeName, ShortName = Source.ShortName, FullName = Source.FullName
-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN 
INSERT (TrainingTypeId, CodeName, ShortName, FullName) 
VALUES (TrainingTypeId, CodeName, ShortName, FullName) 
-- delete rows that are in the target but not the source 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[TrainingType] OFF
GO
