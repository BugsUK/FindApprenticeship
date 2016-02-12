CREATE TABLE [UserProfile].[AgencyUser]
(
	[AgencyUserId] INT IDENTITY(1,1), 
    [DateCreated] DATETIME2 NOT NULL, 
    [DateUpdated] DATETIME2 NULL, 
    [Username] NVARCHAR(MAX) NOT NULL, 
    CONSTRAINT PK_UserProfile_AgencyUser PRIMARY KEY (AgencyUserId)
)
