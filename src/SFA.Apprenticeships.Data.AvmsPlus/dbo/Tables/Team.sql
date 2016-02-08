CREATE TABLE [UserProfile].[Team]
(
	[TeamId] INT IDENTITY(1,1), 
    [Name] NVARCHAR(MAX) NOT NULL, 
    [IsDefault] BIT NOT NULL DEFAULT 0,
	CONSTRAINT PK_UserProfile_Team PRIMARY KEY (TeamId)
)
