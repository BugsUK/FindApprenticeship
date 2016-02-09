CREATE TABLE [UserProfile].[Team]
(
	[TeamId] INT NOT NULL,
	[CodeName] NVARCHAR(3) NOT NULL,
    [Name] NVARCHAR(MAX) NOT NULL, 
    [IsDefault] BIT NOT NULL DEFAULT 0,
	CONSTRAINT PK_UserProfile_Team PRIMARY KEY (TeamId),
	CONSTRAINT UNIQUE_UserProfile_Team_CodeName UNIQUE (CodeName)
)
