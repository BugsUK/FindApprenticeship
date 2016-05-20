CREATE TABLE [dbo].[ExternalServiceSystemRelationship] (
    [ID]                INT IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ExternalServiceId] INT NOT NULL,
    [ExternalSystemId]  INT NOT NULL,
    [IsNasDisabled]     BIT NULL,
    [IsUserEnabled]     BIT NULL,
    CONSTRAINT [PK_ExternalServiceSystemRelationship] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ExternalServiceSystemRelationship_ExternalSystem] FOREIGN KEY ([ExternalSystemId]) REFERENCES [dbo].[ExternalSystem] ([ID])
);



