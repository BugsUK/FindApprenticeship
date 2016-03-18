CREATE TABLE [dbo].[Message] (
    [MessageId]         INT             IDENTITY (-1, -1) NOT FOR REPLICATION NOT NULL,
    [Sender]            INT             NOT NULL,
    [SenderType]        INT             NOT NULL,
    [Recipient]         INT             NOT NULL,
    [RecipientType]     INT             NOT NULL,
    [MessageDate]       DATETIME        NOT NULL,
    [MessageEventId]    INT             NOT NULL,
    [Text]              NVARCHAR (MAX)  NULL,
    [Title]             NVARCHAR (1000) NULL,
    [IsRead]            BIT             CONSTRAINT [DF_Message_Read] DEFAULT ((0)) NOT NULL,
    [IsDeleted]         BIT             CONSTRAINT [DF_Message_Deleted] DEFAULT ((0)) NOT NULL,
    [MessageCategoryID] INT             NULL,
    [ReadDate]          DATETIME        NULL,
    [DeletedBy]         NVARCHAR (250)  NULL,
    [ReadByFirst]       NVARCHAR (250)  NULL,
    [DeletedDate]       DATETIME        NULL,
    CONSTRAINT [PK_Message] PRIMARY KEY CLUSTERED ([MessageId] ASC),
    CONSTRAINT [FK_Message_MessageCategory] FOREIGN KEY ([MessageCategoryID]) REFERENCES [dbo].[MessageCategory] ([MessageCategoryId]),
    CONSTRAINT [FK_Message_MessageEvent1] FOREIGN KEY ([MessageEventId]) REFERENCES [dbo].[MessageEvent] ([MessageEventId]),
    CONSTRAINT [FK_Message_UserTypeRecipient] FOREIGN KEY ([RecipientType]) REFERENCES [dbo].[UserType] ([UserTypeId]),
    CONSTRAINT [FK_Message_UserTypeSender] FOREIGN KEY ([SenderType]) REFERENCES [dbo].[UserType] ([UserTypeId])
);


GO
CREATE NONCLUSTERED INDEX [idx_Message_Recipient_RecipientType_IsDeleted]
    ON [dbo].[Message]([Recipient] ASC, [RecipientType] ASC, [IsDeleted] ASC)
    INCLUDE([IsRead], [MessageId]);

