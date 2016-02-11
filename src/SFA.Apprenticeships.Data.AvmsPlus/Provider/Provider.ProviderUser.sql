CREATE TABLE [Provider].[ProviderUser]
(
	[ProviderUserId] INT NOT NULL PRIMARY KEY IDENTITY, 
	[ProviderUserGuid] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(), 
    [ProviderUserStatusId] INT NOT NULL,
    [ProviderId] INT NOT NULL, 
    [Username] NVARCHAR(100) NOT NULL, 
    [Fullname] NVARCHAR(MAX) NOT NULL, 
    [PreferredSiteErn] INT NULL, 
    [Email] NVARCHAR(MAX) NOT NULL, 
    [EmailVerificationCode] CHAR(6) NULL, 
    [EmailVerifiedDateTime] DATETIME2 NULL, 
    [PhoneNumber] NVARCHAR(MAX) NOT NULL, 
    [DateCreated] DATETIME2 NOT NULL, 
    [DateUpdated] DATETIME2 NULL
)
GO

CREATE UNIQUE INDEX [IX_ProviderUser_Username] ON [Provider].[ProviderUser] ([Username])
GO

CREATE INDEX [IX_ProviderUser_ProviderUserGuid] ON [Provider].[ProviderUser] ([ProviderUserGuid])
GO
