CREATE TABLE [Vacancy].[TrainingType]
(
	[TrainingTypeCode] CHAR NOT NULL, 
    [FullName] NVARCHAR(MAX) NOT NULL, 
    CONSTRAINT [PK_TrainingType] PRIMARY KEY ([TrainingTypeCode]) 
)
