CREATE TABLE [dbo].[BackgroundSearchLog] (
    [BackgroundSearchLogId]       INT      IDENTITY (-1, -1) NOT FOR REPLICATION NOT NULL,
    [Date]                        DATETIME NOT NULL,
    [NumberOfVacancies]           INT      NOT NULL,
    [NumberOfCandidatesProcessed] INT      NOT NULL,
    [NumberOfFailures]            INT      NOT NULL,
    CONSTRAINT [PK_BackgroundSearchLog] PRIMARY KEY CLUSTERED ([BackgroundSearchLogId] ASC)
);

