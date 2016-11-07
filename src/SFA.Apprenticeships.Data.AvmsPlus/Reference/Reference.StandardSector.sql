CREATE TABLE [Reference].[StandardSector]
(
	[StandardSectorId] INT IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [FullName] NVARCHAR(MAX) NOT NULL, 
	[ApprenticeshipOccupationId] INT NOT NULL DEFAULT 1, 
    CONSTRAINT [PK_StandardSector] PRIMARY KEY ([StandardSectorId]),
    CONSTRAINT [FK_StandardSector_ApprenticeshipOccupation] FOREIGN KEY ([ApprenticeshipOccupationId]) REFERENCES [dbo].[ApprenticeshipOccupation] ([ApprenticeshipOccupationId])
)
