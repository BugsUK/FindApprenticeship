CREATE TABLE [Reference].[Framework]
(
	[FrameworkId] INT NOT NULL, 
    [CodeName] NCHAR(3) NOT NULL, 
    [ShortName] NVARCHAR(MAX) NOT NULL, 
    [FullName] NVARCHAR(MAX) NOT NULL, 
    [FrameworkStatusId] INT NOT NULL, 
    [OccupationId] INT NOT NULL, 
    [PreviousOccupationId] INT NULL, 
    [ClosedDate] DATETIME2 NULL, 
    CONSTRAINT [PK_Reference_Framework] PRIMARY KEY ([FrameworkId]), 
    CONSTRAINT [FK_Reference_Framework_FrameworkStatusId_Reference_FrameworkStatus_FrameworkStatusId] FOREIGN KEY ([FrameworkStatusId]) REFERENCES [Reference].[FrameworkStatus]([FrameworkStatusId]), 
    CONSTRAINT [FK_Reference_Framework_OccupationId_Reference_Occupation_OccupationId] FOREIGN KEY ([OccupationId]) REFERENCES [Reference].[Occupation]([OccupationId]), 
    CONSTRAINT [FK_Reference_Framework_PreviousOccupationId_Reference_Occupation_OccupationId] FOREIGN KEY ([PreviousOccupationId]) REFERENCES [Reference].[Occupation]([OccupationId])
)
