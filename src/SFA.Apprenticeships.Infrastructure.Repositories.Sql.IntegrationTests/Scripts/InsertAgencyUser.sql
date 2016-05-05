SET IDENTITY_INSERT UserProfile.AgencyUser ON

MERGE INTO UserProfile.AgencyUser AS TARGET
USING (VALUES (
		1, -- AgencyUserId
		NEWID(), -- AgencyUserGuid
		'jane.agency@sfa.bis.gov.uk', -- Username
		'QA', -- RoleId
		GETUTCDATE(), -- CreatedDateTime
		GETUTCDATE() -- UpdatedDateTime
	)
) 
AS SOURCE (
	AgencyUserId,
	AgencyUserGuid,
	Username,
	RoleId,
	CreatedDateTime,
	UpdatedDateTime
) 
ON TARGET.AgencyUserId = SOURCE.AgencyUserId 
WHEN MATCHED THEN 
UPDATE SET
	AgencyUserGuid = SOURCE.AgencyUserGuid,
	Username = SOURCE.Username,
	RoleId = SOURCE.RoleId,
	CreatedDateTime = SOURCE.CreatedDateTime,
	UpdatedDateTime = SOURCE.UpdatedDateTime
WHEN NOT MATCHED BY TARGET THEN 
INSERT (
	AgencyUserId,
	AgencyUserGuid,
	Username,
	RoleId,
	CreatedDateTime,
	UpdatedDateTime
)
VALUES (
	AgencyUserId,
	AgencyUserGuid,
	Username,
	RoleId,
	CreatedDateTime,
	UpdatedDateTime
)
;

SELECT * FROM UserProfile.AgencyUser
WHERE AgencyUserId = 1

SET IDENTITY_INSERT UserProfile.AgencyUser OFF
