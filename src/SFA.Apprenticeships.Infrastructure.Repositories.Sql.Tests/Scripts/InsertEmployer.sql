SET IDENTITY_INSERT [dbo].[Employer] ON

DECLARE @employerId1 INT = 50000001
DECLARE @employerId2 INT = 50000002

DECLARE @edsurn1 INT = 21000002
DECLARE @edsurn2 INT = 21000003

MERGE INTO [dbo].[Employer] AS TARGET 
USING (VALUES (
	@employerId1, -- EmployerId
	1, -- EmployerStatusTypeId: Activated
	@edsurn1, -- EdsUrn
	'Acme Corp', -- FullName
	'Acme Corp', -- TradingName
	'14 Acacia Avenue', -- AddressLine1
	'Coventry', -- Town
	41, -- CountyId: West Midlands
	'CV1 2WT', -- PostCode
	1, -- PrimaryContact
	1, -- DisableAllowed
	1 -- TrackingAllowed
  ), (
	@employerId2, -- EmployerId
	1, -- EmployerStatusTypeId: Activated
	@edsurn2, -- EdsUrn
	'Awesome Inc', -- FullName
	'Awesome Inc', -- TradingName
	'Big Building', -- AddressLine1
	'London', -- Town
	25, -- CountyId: London
	'EC14 4AA', -- PostCode
	1, -- PrimaryContact
	1, -- DisableAllowed
	1 -- TrackingAllowed
  )
) 
AS SOURCE (
	EmployerId,
	EmployerStatusTypeId,
	EdsUrn,
	FullName,
	TradingName,
	AddressLine1,
	Town,
	CountyId,
	PostCode,
	PrimaryContact,
	DisableAllowed,
	TrackingAllowed
) 
ON TARGET.EmployerId = SOURCE.EmployerId

WHEN MATCHED THEN 
UPDATE SET
	EmployerStatusTypeId = SOURCE.EmployerStatusTypeId,
	EdsUrn = SOURCE.EdsUrn,
	FullName = SOURCE.FullName,
	TradingName = SOURCE.TradingName,
	AddressLine1 = SOURCE.AddressLine1,
	Town = SOURCE.Town,
	CountyId = SOURCE.CountyId,
	PostCode = SOURCE.PostCode,
	PrimaryContact = SOURCE.PrimaryContact,
	DisableAllowed = SOURCE.DisableAllowed,
	TrackingAllowed = SOURCE.TrackingAllowed
WHEN NOT MATCHED BY TARGET THEN 
INSERT (
	EmployerId,
	EmployerStatusTypeId,
	EdsUrn,
	FullName,
	TradingName,
	AddressLine1,
	Town,
	CountyId,
	PostCode,
	PrimaryContact,
	DisableAllowed,
	TrackingAllowed
) 
VALUES (
	EmployerId,
	EmployerStatusTypeId,
	EdsUrn,
	FullName,
	TradingName,
	AddressLine1,
	Town,
	CountyId,
	PostCode,
	PrimaryContact,
	DisableAllowed,
	TrackingAllowed
) 
;

SELECT * FROM [dbo].[Employer]
WHERE EmployerId IN (
	@employerId1,
	@employerId2
)

SET IDENTITY_INSERT [dbo].[Employer] OFF
