CREATE TABLE [Reference].[OccupationStatus]
(
	[OccupationStatusId] INT NOT NULL, 
    [CodeName] NCHAR(3) NOT NULL, 
    [ShortName] NVARCHAR(MAX) NOT NULL, 
    [FullName] NVARCHAR(MAX) NOT NULL, 
    CONSTRAINT [PK_Reference_OccupationStatus] PRIMARY KEY ([OccupationStatusId]) 
)
