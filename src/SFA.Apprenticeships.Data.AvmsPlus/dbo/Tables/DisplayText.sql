CREATE TABLE [dbo].[DisplayText] (
    [DisplayTextId] INT            IDENTITY (-1, -1) NOT FOR REPLICATION NOT NULL,
    [Type]          NVARCHAR (250) NOT NULL,
    [Id]            INT            NOT NULL,
    [StandardText]  NVARCHAR (250) NOT NULL,
    CONSTRAINT [PK_BoilerplateText] PRIMARY KEY CLUSTERED ([DisplayTextId] ASC),
    CONSTRAINT [uq_idx_DisplayText] UNIQUE NONCLUSTERED ([Type] ASC)
);

