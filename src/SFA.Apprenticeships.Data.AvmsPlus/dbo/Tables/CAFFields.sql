CREATE TABLE [dbo].[CAFFields] (
    [CAFFieldsId]   INT             IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CandidateId]   INT             NOT NULL,
    [ApplicationId] INT             NULL,
    [Field]         SMALLINT        NOT NULL,
    [Value]         NVARCHAR (4000) COLLATE Latin1_General_CI_AS NULL,
    CONSTRAINT [PK_CAFFields_1] PRIMARY KEY CLUSTERED ([CAFFieldsId] ASC),
    CONSTRAINT [FK_CAFFields_Application] FOREIGN KEY ([ApplicationId]) REFERENCES [dbo].[Application] ([ApplicationId]) NOT FOR REPLICATION,
    CONSTRAINT [FK_CAFFields_CAFFieldsFieldType] FOREIGN KEY ([Field]) REFERENCES [dbo].[CAFFieldsFieldType] ([CAFFieldsFieldTypeId]) NOT FOR REPLICATION,
    CONSTRAINT [FK_CAFFields_Candidate] FOREIGN KEY ([CandidateId]) REFERENCES [dbo].[Candidate] ([CandidateId]) NOT FOR REPLICATION,
    CONSTRAINT [uq_idx_CAFFields] UNIQUE NONCLUSTERED ([CandidateId] ASC, [ApplicationId] ASC, [Field] ASC)
);




GO


