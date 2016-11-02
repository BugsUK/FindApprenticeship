CREATE TABLE [dbo].[ApprenticeshipFramework] (
    [ApprenticeshipFrameworkId]           INT            IDENTITY (-1, -1) NOT FOR REPLICATION NOT NULL,
    [ApprenticeshipOccupationId]          INT            NOT NULL,
    [CodeName]                            NVARCHAR (3)   NOT NULL,
    [ShortName]                           NVARCHAR (100) NOT NULL,
    [FullName]                            NVARCHAR (200) NOT NULL,
    [ApprenticeshipFrameworkStatusTypeId] INT            NOT NULL,
    [ClosedDate]                          DATETIME       NULL,
    [PreviousApprenticeshipOccupationId]  INT            NULL,
    [StandardId] INT NULL, 
    CONSTRAINT [PK_Apprenticeship_Framework] PRIMARY KEY CLUSTERED ([ApprenticeshipFrameworkId] ASC),
    CONSTRAINT [FK_ApprenticeshipFramework_ApprenticeshipFrameworkStatusType] FOREIGN KEY ([ApprenticeshipFrameworkStatusTypeId]) REFERENCES [dbo].[ApprenticeshipFrameworkStatusType] ([ApprenticeshipFrameworkStatusTypeId]),
    CONSTRAINT [FK_ApprenticeshipFramework_ApprenticeshipOccupation] FOREIGN KEY ([ApprenticeshipOccupationId]) REFERENCES [dbo].[ApprenticeshipOccupation] ([ApprenticeshipOccupationId]),
    CONSTRAINT [FK_ApprenticeshipFramework_PreviousApprenticeshipOccupation] FOREIGN KEY ([PreviousApprenticeshipOccupationId]) REFERENCES [dbo].[ApprenticeshipOccupation] ([ApprenticeshipOccupationId]),
    CONSTRAINT [uq_idx_ApprenticeshipFramework] UNIQUE NONCLUSTERED ([FullName] ASC)
);

