CREATE TABLE [Reference].[Standard]
(
	[StandardId] INT IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [StandardSectorId] INT NOT NULL, 
    [LarsCode] INT NOT NULL, 
    [FullName] NVARCHAR(MAX) NOT NULL,
	[EducationLevelId] INT NOT NULL,
	[ApprenticeshipFrameworkStatusTypeId] [int] NOT NULL CONSTRAINT [DF_Standard_ApprenticeshipFrameworkStatusTypeId]  DEFAULT ((1)),
    CONSTRAINT [PK_Reference_Standard] PRIMARY KEY ([StandardId]), 
    CONSTRAINT [FK_Reference_Standard_StandardSectorId] FOREIGN KEY ([StandardSectorId]) REFERENCES [Reference].[StandardSector]([StandardSectorId]),
    CONSTRAINT [FK_Reference_Standard_EducationLevelId] FOREIGN KEY ([EducationLevelId]) REFERENCES [Reference].[EducationLevel]([EducationLevelId]),
	CONSTRAINT [FK_Standard_ApprenticeshipFrameworkStatusType] FOREIGN KEY([ApprenticeshipFrameworkStatusTypeId]) REFERENCES [dbo].[ApprenticeshipFrameworkStatusType] ([ApprenticeshipFrameworkStatusTypeId])
)