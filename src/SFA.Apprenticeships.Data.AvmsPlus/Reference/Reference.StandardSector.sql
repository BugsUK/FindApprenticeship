CREATE TABLE [Reference].[StandardSector]
(
	[StandardSectorId] INT NOT NULL, 
    [FullName] NVARCHAR(MAX) NOT NULL, 
	[ApprenticeshipOccupationId] INT NOT NULL DEFAULT 1, 
    CONSTRAINT [PK_StandardSector] PRIMARY KEY ([StandardSectorId]),
    CONSTRAINT [FK_StandardSector_ApprenticeshipOccupation] FOREIGN KEY ([ApprenticeshipOccupationId]) REFERENCES [dbo].[ApprenticeshipOccupation] ([ApprenticeshipOccupationId])
)
