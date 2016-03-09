MERGE INTO [dbo].[ProviderSiteRelationshipType] AS Target 
USING (VALUES 
  (1, N'Owner'),
  (2, N'Subcontractor'),
  (3, N'Recruitment Agent')
) 
AS Source (ProviderSiteRelationshipTypeId, ProviderSiteRelationshipTypeName) 
ON Target.ProviderSiteRelationshipTypeId = Source.ProviderSiteRelationshipTypeId 
-- update matched rows 
WHEN MATCHED THEN 
UPDATE SET ProviderSiteRelationshipTypeName = Source.ProviderSiteRelationshipTypeName
-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN 
INSERT (ProviderSiteRelationshipTypeId, ProviderSiteRelationshipTypeName) 
VALUES (ProviderSiteRelationshipTypeId, ProviderSiteRelationshipTypeName) 
-- delete rows that are in the target but not the source 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;
