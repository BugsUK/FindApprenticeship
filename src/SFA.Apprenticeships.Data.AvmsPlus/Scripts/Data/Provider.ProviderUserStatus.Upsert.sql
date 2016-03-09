MERGE INTO [Provider].[ProviderUserStatus] AS Target 
USING (VALUES 
  (10, N'REG', N'Registered', N'Registered'),
  (20, N'VER', N'Email verified', N'Email verified')
) 
AS Source (ProviderUserStatusId, CodeName, ShortName, FullName)
ON Target.ProviderUserStatusId = Source.ProviderUserStatusId
-- update matched rows 
WHEN MATCHED THEN 
UPDATE SET CodeName = Source.CodeName, ShortName = Source.ShortName, FullName = Source.FullName
-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN 
INSERT (ProviderUserStatusId, CodeName, ShortName, FullName) 
VALUES (ProviderUserStatusId, CodeName, ShortName, FullName) 
-- delete rows that are in the target but not the source 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;
