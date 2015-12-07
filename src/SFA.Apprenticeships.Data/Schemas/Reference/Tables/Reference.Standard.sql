CREATE TABLE [Reference].[Standard]
(
	[StandardId] INT NOT NULL, 
    [FullName] NVARCHAR(MAX) NOT NULL,
    [SectorId] INT NOT NULL, 
	[LevelCode] CHAR NOT NULL,
    CONSTRAINT [PK_Reference_Standard] PRIMARY KEY ([StandardId]), 
    CONSTRAINT [FK_Standard_SectorId_Sector_SectorId] FOREIGN KEY ([SectorId]) REFERENCES [Reference].[Sector]([SectorId]) 
)
