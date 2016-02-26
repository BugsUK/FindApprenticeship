CREATE TABLE [UserProfile].[AgencyUser]
(
	[AgencyUserId] INT IDENTITY(1,1), 
    [DateCreated] DATETIME2 NOT NULL, 
    [DateUpdated] DATETIME2 NULL, 
    [Username] NVARCHAR(100) NOT NULL, 
    CONSTRAINT PK_UserProfile_AgencyUser PRIMARY KEY (AgencyUserId)
)

GO


CREATE UNIQUE INDEX [IX_AgencyUser_Username] ON [UserProfile].[AgencyUser] ([Username])
