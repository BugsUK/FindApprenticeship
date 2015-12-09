CREATE TABLE [Reference].[Occupation]
(
	[OccupationId] INT NOT NULL, 
	[OccupationStatusId] INT NOT NULL, 
    [CodeName] NCHAR(3) NOT NULL, 
    [ShortName] NVARCHAR(MAX) NOT NULL, 
    [FullName] NVARCHAR(MAX) NOT NULL, 
    [ClosedDate] DATETIME2 NULL, 
    CONSTRAINT [PK_Reference_Occupation] PRIMARY KEY ([OccupationId]), 
    CONSTRAINT [FK_Reference_Occupation_OccupationStatusId_Reference_OccupationStatus] FOREIGN KEY ([OccupationStatusId]) REFERENCES [Reference].[OccupationStatus]([OccupationStatusId])  
)
