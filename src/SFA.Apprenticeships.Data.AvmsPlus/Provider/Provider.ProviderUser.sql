CREATE TABLE [Provider].[ProviderUser]
(
	[ProviderUserId] INT NOT NULL PRIMARY KEY, 
	[ProviderUserGuid] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(), 
    [ProviderUserStatusId] INT NOT NULL,
    [ProviderId] INT NOT NULL, 
    [Username] NVARCHAR(MAX) NOT NULL, 
    [Fullname] NVARCHAR(MAX) NOT NULL, 
    [PreferredSiteErn] INT NULL, 
    [Email] NVARCHAR(MAX) NOT NULL, 
    [EmailVerificationCode] CHAR(6) NULL, 
    [EmailVerifiedDateTime] DATETIME2 NULL, 
    [PhoneNumber] NVARCHAR(MAX) NOT NULL
)