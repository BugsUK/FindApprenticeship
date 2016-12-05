CREATE TABLE [RaaApi].[User]
(
	[PrimaryApiKey] UNIQUEIDENTIFIER NOT NULL,
	[SecondaryApiKey] UNIQUEIDENTIFIER NOT NULL,
	[UserTypeId] INT NOT NULL, 
	[ReferencedEntityId] INT NOT NULL, 
	[ReferencedEntityGuid] UNIQUEIDENTIFIER NULL, 
	[ReferencedEntitySurrogateId] INT NOT NULL, 
	CONSTRAINT FK_RaaApiUser_RaaApiUserType FOREIGN KEY ([UserTypeId]) REFERENCES [RaaApi].[UserType]([UserTypeId])
)

GO
