CREATE TABLE [UserProfile].[Role]
(
	[RoleId] INT NOT NULL, 
	[CodeName] NVARCHAR(3) NOT NULL,
	[Name] NVARCHAR(MAX) NOT NULL, 
    [IsDefault] BIT NOT NULL DEFAULT 0,
	CONSTRAINT PK_UserProfile_Role PRIMARY KEY (RoleId),
	CONSTRAINT UNIQUE_UserProfile_Role_CodeName UNIQUE (CodeName)
)
