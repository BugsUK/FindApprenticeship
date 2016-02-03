CREATE TABLE [dbo].[EmployerHistory] (
    [EmployerHistoryId] INT            IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [EmployerId]        INT            NOT NULL,
    [UserName]          NVARCHAR (50)  COLLATE Latin1_General_CI_AS NOT NULL,
    [Date]              DATETIME       NOT NULL,
    [Event]             INT            NOT NULL,
    [Comment]           VARCHAR (4000) COLLATE Latin1_General_CI_AS NULL,
    CONSTRAINT [PK_EmployerHistory_1] PRIMARY KEY CLUSTERED ([EmployerHistoryId] ASC),
    CONSTRAINT [FK_Employer_History_Employer] FOREIGN KEY ([EmployerId]) REFERENCES [dbo].[Employer] ([EmployerId]) NOT FOR REPLICATION,
    CONSTRAINT [FK_EmployerHistory_EmployerHistoryEventType] FOREIGN KEY ([Event]) REFERENCES [dbo].[EmployerHistoryEventType] ([EmployerHistoryEventTypeId]) NOT FOR REPLICATION
);




GO
CREATE NONCLUSTERED INDEX [idx_EmployerHistory_Event]
    ON [dbo].[EmployerHistory]([Event] ASC)
    INCLUDE([EmployerId], [Date])
    ON [Index];

