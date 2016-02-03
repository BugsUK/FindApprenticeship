CREATE TABLE [dbo].[ApprenticeshipOccupation] (
    [ApprenticeshipOccupationId]           INT            IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Codename]                             NVARCHAR (3)   COLLATE Latin1_General_CI_AS NOT NULL,
    [ShortName]                            NVARCHAR (50)  COLLATE Latin1_General_CI_AS NOT NULL,
    [FullName]                             NVARCHAR (100) COLLATE Latin1_General_CI_AS NOT NULL,
    [ApprenticeshipOccupationStatusTypeId] INT            NOT NULL,
    [ClosedDate]                           DATETIME       NULL,
    CONSTRAINT [PK_ApprenticeshipOccupation_1] PRIMARY KEY CLUSTERED ([ApprenticeshipOccupationId] ASC),
    CONSTRAINT [FK_ApprenticeshipOccupation_ApprenticeshipOccupationStatusType] FOREIGN KEY ([ApprenticeshipOccupationStatusTypeId]) REFERENCES [dbo].[ApprenticeshipOccupationStatusType] ([ApprenticeshipOccupationStatusTypeId]) NOT FOR REPLICATION,
    CONSTRAINT [FK_ApprenticeshipOccupationStatusType_ApprenticeshipOccupationStatusTypeID] FOREIGN KEY ([ApprenticeshipOccupationStatusTypeId]) REFERENCES [dbo].[ApprenticeshipOccupationStatusType] ([ApprenticeshipOccupationStatusTypeId]) NOT FOR REPLICATION,
    CONSTRAINT [uq_idx_ApprenticeshipOccupation] UNIQUE NONCLUSTERED ([FullName] ASC)
);



