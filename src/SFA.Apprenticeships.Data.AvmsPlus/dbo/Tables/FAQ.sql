CREATE TABLE [dbo].[FAQ] (
    [FAQId]      INT             IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [SortOrder]  INT             NOT NULL,
    [UserTypeId] INT             NOT NULL,
    [Title]      NVARCHAR (100)  COLLATE Latin1_General_CI_AS NOT NULL,
    [Content]    NVARCHAR (2000) COLLATE Latin1_General_CI_AS NOT NULL,
    CONSTRAINT [PK_FAQ] PRIMARY KEY CLUSTERED ([FAQId] ASC),
    CONSTRAINT [FK_FAQ_OwnerType] FOREIGN KEY ([UserTypeId]) REFERENCES [dbo].[UserType] ([UserTypeId]) NOT FOR REPLICATION,
    CONSTRAINT [uq_idx_FAQ_sort] UNIQUE NONCLUSTERED ([UserTypeId] ASC, [SortOrder] ASC),
    CONSTRAINT [uq_idx_FAQ_title] UNIQUE NONCLUSTERED ([UserTypeId] ASC, [Title] ASC)
);



