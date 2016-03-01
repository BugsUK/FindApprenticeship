CREATE TABLE [UserProfile].[AgencyUser]
(
	[AgencyUserId] INT IDENTITY(1,1),
	[AgencyUserGuid] UNIQUEIDENTIFIER NOT NULL, 
    [CreatedDateTime] DATETIME2 NOT NULL, 
    [UpdatedDateTime] DATETIME2 NULL, 
    [Username] NVARCHAR(100) NOT NULL, 
    [RoleId] NVARCHAR(20) NULL, 
    CONSTRAINT PK_UserProfile_AgencyUser PRIMARY KEY (AgencyUserId)
)

GO


CREATE UNIQUE INDEX [IX_AgencyUser_Username] ON [UserProfile].[AgencyUser] ([Username])
