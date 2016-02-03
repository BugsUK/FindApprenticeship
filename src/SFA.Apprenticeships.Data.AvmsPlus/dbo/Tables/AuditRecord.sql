CREATE TABLE [dbo].[AuditRecord] (
    [AuditRecordId]      INT           IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Author]             NVARCHAR (50) COLLATE Latin1_General_CI_AS NOT NULL,
    [ChangeDate]         DATETIME      NOT NULL,
    [AttachedtoItem]     INT           NOT NULL,
    [AttachedtoItemType] INT           NOT NULL,
    CONSTRAINT [PK_AuditRecord] PRIMARY KEY CLUSTERED ([AuditRecordId] ASC),
    CONSTRAINT [FK_AuditRecord_AttachedtoItemType] FOREIGN KEY ([AttachedtoItemType]) REFERENCES [dbo].[AttachedtoItemType] ([AttachedtoItemTypeId]) NOT FOR REPLICATION
);



