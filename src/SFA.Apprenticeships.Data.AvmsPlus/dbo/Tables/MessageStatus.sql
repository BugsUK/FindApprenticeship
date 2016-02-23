CREATE TABLE [dbo].[MessageStatus] (
    [MessageStatusId] INT           NOT NULL,
    [Description]     NVARCHAR (64) NULL,
    CONSTRAINT [PK_MessageStatus] PRIMARY KEY CLUSTERED ([MessageStatusId] ASC)
);

