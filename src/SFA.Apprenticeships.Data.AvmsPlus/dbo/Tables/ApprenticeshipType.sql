CREATE TABLE [dbo].[ApprenticeshipType] (
    [ApprenticeshipTypeId] INT            IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CodeName]             NVARCHAR (3)   NOT NULL,
    [ShortName]            NVARCHAR (100) NOT NULL,
    [FullName]             NVARCHAR (200) NOT NULL,
	[EducationLevel]	   INT NOT NULL
    CONSTRAINT [PK_ApprenticeshipType] PRIMARY KEY CLUSTERED ([ApprenticeshipTypeId] ASC),
    CONSTRAINT [uq_idx_ApprenticeshipType] UNIQUE NONCLUSTERED ([FullName] ASC)
);

