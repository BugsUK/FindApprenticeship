CREATE TABLE [dbo].[CountyAudit] (
    [CountyAuditId]   INT          IDENTITY (1, 1) NOT NULL,
    [AuditDate]       DATETIME     NOT NULL,
    [ItemName]        VARCHAR (50) NOT NULL,
    [ItemKey]         INT          NOT NULL,
    [OldCountyId]     INT          NULL,
    [NewCountyId]     INT          NULL,
    [TransactionType] VARCHAR (50) NULL,
    CONSTRAINT [PK_LCountyAudit] PRIMARY KEY CLUSTERED ([CountyAuditId] ASC)
);

