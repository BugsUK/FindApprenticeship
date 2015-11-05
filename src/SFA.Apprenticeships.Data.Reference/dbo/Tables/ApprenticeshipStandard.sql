CREATE TABLE [dbo].[ApprenticeshipStandard]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [ApprenticeshipSectorId] INT NOT NULL, 
    [Name] NVARCHAR(100) NOT NULL, 
    [ApprenticeshipLevel] INT NOT NULL, 
    CONSTRAINT [FK_ApprenticeshipStandard_ApprenticeshipSector] FOREIGN KEY ([ApprenticeshipSectorId]) REFERENCES [dbo].[ApprenticeshipSector]([Id])
)
