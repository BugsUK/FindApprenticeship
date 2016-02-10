CREATE TABLE [UserProfile].[AgencyUserRole]
(
	[AgencyUserRoleId] INT NOT NULL, 
	[CodeName] NVARCHAR(3) NOT NULL,
	[Name] NVARCHAR(MAX) NOT NULL, 
    [IsDefault] BIT NOT NULL DEFAULT 0,
	CONSTRAINT PK_UserProfile_AgencyUserRole PRIMARY KEY ([AgencyUserRoleId]),
	CONSTRAINT UNIQUE_UserProfile_AgencyUserRole_CodeName UNIQUE (CodeName)
)
