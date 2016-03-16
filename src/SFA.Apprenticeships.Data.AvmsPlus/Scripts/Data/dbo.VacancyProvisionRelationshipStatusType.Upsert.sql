SET IDENTITY_INSERT [dbo].[VacancyProvisionRelationshipStatusType] ON
GO

MERGE INTO [dbo].[VacancyProvisionRelationshipStatusType] AS Target 
USING (VALUES 
	(1, 'PEN', 'Pen', 'Pending'),
	(2, 'ACT', 'Act', 'Active'),
	(3, 'DEL', 'Del', 'Deleted'),
	(4, 'LIV', 'Liv', 'Live')
) 
AS Source (VacancyProvisionRelationshipStatusTypeId, CodeName, ShortName, FullName) 
ON Target.VacancyProvisionRelationshipStatusTypeId = Source.VacancyProvisionRelationshipStatusTypeId 
-- update matched rows 
WHEN MATCHED THEN 
UPDATE SET CodeName = Source.CodeName, ShortName = Source.ShortName, FullName = Source.FullName
-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN 
INSERT (VacancyProvisionRelationshipStatusTypeId, CodeName, ShortName, FullName) 
VALUES (VacancyProvisionRelationshipStatusTypeId, CodeName, ShortName, FullName) 
-- delete rows that are in the target but not the source 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SELECT * FROM [dbo].[VacancyProvisionRelationshipStatusType]

SET IDENTITY_INSERT [dbo].[VacancyProvisionRelationshipStatusType] OFF
GO
