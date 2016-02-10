CREATE TABLE [UserProfile].[AgencyUserTeam]
(
	[AgencyUserTeamId] INT NOT NULL,
	[CodeName] NVARCHAR(3) NOT NULL,
    [Name] NVARCHAR(MAX) NOT NULL, 
    [IsDefault] BIT NOT NULL DEFAULT 0,
	CONSTRAINT PK_UserProfile_AgencyUserTeam PRIMARY KEY ([AgencyUserTeamId]),
	CONSTRAINT UNIQUE_UserProfile_AgencyUserTeam_CodeName UNIQUE (CodeName)
)
