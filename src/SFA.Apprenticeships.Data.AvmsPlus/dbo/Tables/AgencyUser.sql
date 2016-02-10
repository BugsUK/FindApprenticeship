CREATE TABLE [UserProfile].[AgencyUser]
(
	[AgencyUserId] INT IDENTITY(1,1), 
    [DateCreated] DATETIME2 NOT NULL, 
    [DateUpdated] DATETIME2 NULL, 
    [Username] NVARCHAR(MAX) NOT NULL, 
    [TeamId] INT NULL, 
    [RoleId] INT NULL,
	CONSTRAINT PK_UserProfile_AgencyUser PRIMARY KEY (AgencyUserId),
	CONSTRAINT FK_UserProfile_AgencyUser_AgencyUserTeam FOREIGN KEY (TeamId) REFERENCES [UserProfile].[AgencyUserTeam](AgencyUserTeamId),
	CONSTRAINT FK_UserProfile_AgencyUser_AgencyUserRole FOREIGN KEY (RoleId) REFERENCES [UserProfile].[AgencyUserRole](AgencyUserRoleId)
)
