MERGE INTO [RaaApi].[UserType] AS Target 
USING (VALUES 
  (0, 'UNK', 'UNK', 'Unknown'),
  (1, 'PVD', 'PVD', 'Provider'),
  (2, 'EMP', 'EMP', 'Employer')
) 

AS Source (UserTypeId, CodeName, ShortName, FullName) 
ON Target.UserTypeId = Source.UserTypeId 
-- update matched rows 
WHEN MATCHED THEN 
UPDATE SET CodeName = Source.CodeName, 
	ShortName = Source.ShortName, 
	FullName = Source.FullName
-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN 
INSERT (UserTypeId, CodeName, ShortName, FullName) 
VALUES (UserTypeId, CodeName, ShortName, FullName) 
-- delete rows that are in the target but not the source 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;