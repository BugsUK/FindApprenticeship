CREATE TABLE [dbo].[SearchAuditRecord] (
    [SearchAuditRecordId] INT            IDENTITY (0, 1) NOT FOR REPLICATION NOT NULL,
    [CandidateId]         INT            NOT NULL,
    [RunDate]             DATETIME       NOT NULL,
    [SearchCriteria]      NVARCHAR (500) NULL,
    [RunTime]             DATETIME       NULL,
    [RecordCount]         INT            NULL,
    CONSTRAINT [PK_SearchAuditRecord_1] PRIMARY KEY CLUSTERED ([SearchAuditRecordId] ASC)
);

