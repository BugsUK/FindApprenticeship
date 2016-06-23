MERGE INTO [dbo].[LocalAuthorityGroupType] AS Target 
USING (VALUES 
	(1, N'Division'),
	(2, N'Managing Area'),
	(3, N'Alias'),
	(4, N'Region')
) 

AS Source (LocalAuthorityGroupTypeID, LocalAuthorityGroupTypeName) 
ON Target.LocalAuthorityGroupTypeID = Source.LocalAuthorityGroupTypeID 
-- update matched rows 
WHEN MATCHED THEN 
UPDATE SET LocalAuthorityGroupTypeName = Source.LocalAuthorityGroupTypeName
-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN 
INSERT (LocalAuthorityGroupTypeID, LocalAuthorityGroupTypeName) 
VALUES (LocalAuthorityGroupTypeID, LocalAuthorityGroupTypeName) 
-- delete rows that are in the target but not the source 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;