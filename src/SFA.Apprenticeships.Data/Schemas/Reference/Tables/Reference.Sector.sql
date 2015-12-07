CREATE TABLE [Reference].[Sector]
(
	[SectorId] INT NOT NULL, 
    [FullName] NVARCHAR(MAX) NOT NULL, 
    [LevelCode] CHAR NOT NULL, 
    CONSTRAINT [PK_Sector] PRIMARY KEY ([SectorId]), 
    CONSTRAINT [FK_Reference_Sector_LevelCode] FOREIGN KEY ([LevelCode]) REFERENCES [Reference].[Level]([LevelCode])
)
