MERGE INTO [dbo].[LocalAuthorityGroupPurpose] AS Target 
USING (VALUES 
	(1, N'Management'),
	(2, N'Aliases'),
	(3, N'Local Government')
) 
AS Source (LocalAuthorityGroupPurposeID, LocalAuthorityGroupPurposeName) 
ON Target.LocalAuthorityGroupPurposeID = Source.LocalAuthorityGroupPurposeID 
-- update matched rows 
WHEN MATCHED THEN 
UPDATE SET LocalAuthorityGroupPurposeName = Source.LocalAuthorityGroupPurposeName
-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN 
INSERT (LocalAuthorityGroupPurposeID, LocalAuthorityGroupPurposeName) 
VALUES (LocalAuthorityGroupPurposeID, LocalAuthorityGroupPurposeName) 
-- delete rows that are in the target but not the source 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;