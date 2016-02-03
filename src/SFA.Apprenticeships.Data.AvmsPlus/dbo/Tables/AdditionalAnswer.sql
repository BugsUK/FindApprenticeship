CREATE TABLE [dbo].[AdditionalAnswer] (
    [AdditionalAnswerId]   INT             IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ApplicationId]        INT             NOT NULL,
    [AdditionalQuestionId] INT             NOT NULL,
    [Answer]               NVARCHAR (4000) NOT NULL,
    CONSTRAINT [PK_AdditionalAnswer] PRIMARY KEY CLUSTERED ([AdditionalAnswerId] ASC) WITH (FILLFACTOR = 90) ON [PRIMARY],
    CONSTRAINT [FK_AdditionalAnswer_AdditionalQuestion] FOREIGN KEY ([AdditionalQuestionId]) REFERENCES [dbo].[AdditionalQuestion] ([AdditionalQuestionId]),
    CONSTRAINT [FK_AdditionalAnswer_Application] FOREIGN KEY ([ApplicationId]) REFERENCES [dbo].[Application] ([ApplicationId]),
    CONSTRAINT [uq_idx_additionalAnswer] UNIQUE NONCLUSTERED ([ApplicationId] ASC, [AdditionalQuestionId] ASC) WITH (FILLFACTOR = 90) ON [Index]
);

