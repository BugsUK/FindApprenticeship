SET IDENTITY_INSERT [dbo].[VacancyOwnerRelationship] ON

MERGE INTO [dbo].[VacancyOwnerRelationship] AS TARGET 
USING (VALUES (
	1, -- VacancyOwnerRelationshipId
	1, -- EmployerId
	1, -- ProviderSiteID
	0, -- ContractHolderIsEmployer
	0, -- ManagerIsEmployer
	4, -- StatusTypeId: Live
	0 -- NationWideAllowed
  ), (
	2, -- VacancyOwnerRelationshipId
	2, -- EmployerId
	1, -- ProviderSiteID
	0, -- ContractHolderIsEmployer
	0, -- ManagerIsEmployer
	4, -- StatusTypeId: Live
	0 -- NationWideAllowed
  )
) 
AS SOURCE (
	VacancyOwnerRelationshipId,
	EmployerId,
	ProviderSiteID,
	ContractHolderIsEmployer,
	ManagerIsEmployer,
	StatusTypeId,
	NationWideAllowed
) 
ON TARGET.VacancyOwnerRelationshipId = SOURCE.VacancyOwnerRelationshipId

WHEN MATCHED THEN 
UPDATE SET
	EmployerId = SOURCE.EmployerId,
	ProviderSiteID = SOURCE.EmployerId,
	ContractHolderIsEmployer = SOURCE.EmployerId,
	ManagerIsEmployer = SOURCE.EmployerId,
	StatusTypeId = SOURCE.EmployerId,
	NationWideAllowed = SOURCE.EmployerId

WHEN NOT MATCHED BY TARGET THEN 
INSERT (
	VacancyOwnerRelationshipId,
	EmployerId,
	ProviderSiteID,
	ContractHolderIsEmployer,
	ManagerIsEmployer,
	StatusTypeId,
	NationWideAllowed
) 
VALUES (
	VacancyOwnerRelationshipId,
	EmployerId,
	ProviderSiteID,
	ContractHolderIsEmployer,
	ManagerIsEmployer,
	StatusTypeId,
	NationWideAllowed
) 
;

SELECT * FROM [dbo].[VacancyOwnerRelationship]
WHERE VacancyOwnerRelationshipId IN (
	1
)

SET IDENTITY_INSERT [dbo].[VacancyOwnerRelationship] OFF
