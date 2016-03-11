SET IDENTITY_INSERT Provider.ProviderUser ON

DECLARE @providerUserId INT = -1

MERGE INTO Provider.ProviderUser AS TARGET
USING (VALUES (
		@providerUserId , -- ProviderUserId
		NEWID(), -- ProviderUserGuid
		20, -- ProviderUserStatusId: Email verified
		(SELECT ProviderId FROM dbo.Provider WHERE UKPRN = 10000000), -- ProviderId
		'jane.doe@example.com', --Username
		'Jane Doe', -- Fullname
		(SELECT TOP 1 ProviderSiteId FROM dbo.ProviderSite WHERE EDSURN = 100339794), -- PreferredProviderSiteId
		'jane.doe@example.com', -- Email
		'XYZ123', -- EmailVerificationCode
		GETUTCDATE(), -- EmailVerifiedDateTime
		'0161555123', -- PhoneNumber
		GETUTCDATE(), -- CreatedDateTime
		GETUTCDATE() -- UpdatedDateTime
	)
) 
AS SOURCE (
	ProviderUserId,
	ProviderUserGuid,
	ProviderUserStatusId,
	ProviderId,
	Username,
	Fullname,
	PreferredProviderSiteId,
	Email,
	EmailVerificationCode,
	EmailVerifiedDateTime,
	PhoneNumber,
	CreatedDateTime,
	UpdatedDateTime
) 
ON TARGET.ProviderUserId = SOURCE.ProviderUserId 
WHEN MATCHED THEN 
UPDATE SET
	ProviderUserGuid = SOURCE.ProviderUserGuid,
	ProviderUserStatusId = SOURCE.ProviderUserStatusId,
	ProviderId = SOURCE.ProviderId,
	Username = SOURCE.Username,
	Fullname = SOURCE.Fullname,
	PreferredProviderSiteId = SOURCE.PreferredProviderSiteId,
	Email = SOURCE.Email,
	EmailVerificationCode = SOURCE.EmailVerificationCode,
	EmailVerifiedDateTime = SOURCE.EmailVerifiedDateTime,
	PhoneNumber = SOURCE.PhoneNumber,
	CreatedDateTime = SOURCE.CreatedDateTime,
	UpdatedDateTime = SOURCE.UpdatedDateTime
WHEN NOT MATCHED BY TARGET THEN 
INSERT (
	ProviderUserId,
	ProviderUserGuid,
	ProviderUserStatusId,
	ProviderId,
	Username,
	Fullname,
	PreferredProviderSiteId,
	Email,
	EmailVerificationCode,
	EmailVerifiedDateTime,
	PhoneNumber,
	CreatedDateTime,
	UpdatedDateTime
)
VALUES (
	ProviderUserId,
	ProviderUserGuid,
	ProviderUserStatusId,
	ProviderId,
	Username,
	Fullname,
	PreferredProviderSiteId,
	Email,
	EmailVerificationCode,
	EmailVerifiedDateTime,
	PhoneNumber,
	CreatedDateTime,
	UpdatedDateTime
)
;

SELECT * FROM Provider.ProviderUser
WHERE ProviderUserId = @providerUserId

SET IDENTITY_INSERT Provider.ProviderUser OFF
