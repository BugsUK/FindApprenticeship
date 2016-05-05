SET IDENTITY_INSERT [dbo].[VacancyOwnerRelationship] ON

DECLARE @vacancyOwnerRelationshipId1 INT = 60000001
DECLARE @vacancyOwnerRelationshipId2 INT = 60000002

DECLARE @employerId1 INT = 50000001
DECLARE @employerId2 INT = 50000002

DECLARE @providerSiteId1 INT = 20000001

MERGE INTO [dbo].[VacancyOwnerRelationship] AS TARGET 
USING (VALUES (
	@vacancyOwnerRelationshipId1, -- VacancyOwnerRelationshipId
	@employerId1, -- EmployerId
	@providerSiteId1, -- ProviderSiteID
	0, -- ContractHolderIsEmployer
	0, -- ManagerIsEmployer
	4, -- StatusTypeId: Live
	0 -- NationWideAllowed
  ), (
	@vacancyOwnerRelationshipId2, -- VacancyOwnerRelationshipId
	@employerId2, -- EmployerId
	@providerSiteId1, -- ProviderSiteID
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
