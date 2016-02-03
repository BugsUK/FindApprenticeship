CREATE TABLE [dbo].[AttachedDocument] (
    [AttachedDocumentId] INT             IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Title]              NVARCHAR (50)   COLLATE Latin1_General_CI_AS NULL,
    [Attachment]         VARBINARY (MAX) NOT NULL,
    [MIMEType]           INT             NOT NULL,
    CONSTRAINT [PK_AttachedDocument_1] PRIMARY KEY CLUSTERED ([AttachedDocumentId] ASC),
    CONSTRAINT [FK_AttachedDocument_MIMEType] FOREIGN KEY ([MIMEType]) REFERENCES [dbo].[MIMEType] ([MIMETypeId]) NOT FOR REPLICATION
);



