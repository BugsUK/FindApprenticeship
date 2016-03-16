CREATE TABLE [dbo].[ApplicationWithdrawnOrDeclinedReasonType] (
    [ApplicationWithdrawnOrDeclinedReasonTypeId] INT            IDENTITY (-1, -1) NOT FOR REPLICATION NOT NULL,
    [CodeName]                                   NVARCHAR (3)   NOT NULL,
    [ShortName]                                  NVARCHAR (50)  NOT NULL,
    [FullName]                                   NVARCHAR (100) NOT NULL,
    CONSTRAINT [PK_ApplicationWithdrawnOrDeclinedReasonType] PRIMARY KEY CLUSTERED ([ApplicationWithdrawnOrDeclinedReasonTypeId] ASC),
    CONSTRAINT [uq_idx_ApplicationWithdrawnOrDeclinedReasonType] UNIQUE NONCLUSTERED ([FullName] ASC)
);

