CREATE TABLE [dbo].[LocalAuthorityAudit] (
    [LocalAuthorityAuditId] INT          IDENTITY (-1, -1) NOT NULL,
    [ItemName]              VARCHAR (50) NOT NULL,
    [ItemKey]               INT          NOT NULL,
    [OldAuthorityId]        INT          NULL,
    [NewAuthorityId]        INT          NULL,
    [TransactionType]       VARCHAR (50) NULL,
    CONSTRAINT [PK_LocalAuthorityAudit] PRIMARY KEY CLUSTERED ([LocalAuthorityAuditId] ASC)
);

