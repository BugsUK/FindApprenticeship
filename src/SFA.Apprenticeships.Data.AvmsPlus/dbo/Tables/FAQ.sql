CREATE TABLE [dbo].[FAQ] (
    [FAQId]      INT             IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [SortOrder]  INT             NOT NULL,
    [UserTypeId] INT             NOT NULL,
    [Title]      NVARCHAR (100)  NOT NULL,
    [Content]    NVARCHAR (2000) NOT NULL,
    CONSTRAINT [PK_FAQ] PRIMARY KEY CLUSTERED ([FAQId] ASC) WITH (FILLFACTOR = 90) ON [PRIMARY],
    CONSTRAINT [FK_FAQ_OwnerType] FOREIGN KEY ([UserTypeId]) REFERENCES [dbo].[UserType] ([UserTypeId]),
    CONSTRAINT [uq_idx_FAQ_sort] UNIQUE NONCLUSTERED ([UserTypeId] ASC, [SortOrder] ASC) WITH (FILLFACTOR = 90) ON [Index],
    CONSTRAINT [uq_idx_FAQ_title] UNIQUE NONCLUSTERED ([UserTypeId] ASC, [Title] ASC) WITH (FILLFACTOR = 90) ON [Index]
);

