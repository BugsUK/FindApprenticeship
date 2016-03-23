CREATE TABLE [dbo].[ExternalSystem] (
    [ID]               INT              IDENTITY (-1, -1) NOT FOR REPLICATION NOT NULL,
    [SystemCode]       UNIQUEIDENTIFIER NOT NULL,
    [SystemName]       NVARCHAR (200)   NULL,
    [OrganisationId]   INT              NULL,
    [OrganisationType] INT              NULL,
    [ContactName]      NVARCHAR (150)   NULL,
    [ContactEmail]     NVARCHAR (200)   NULL,
    [ContactNumber]    NVARCHAR (32)    NULL,
    [IsNasDisabled]    BIT              NULL,
    [IsUserEnabled]    BIT              NULL,
    CONSTRAINT [PK_ExternalSystem] PRIMARY KEY CLUSTERED ([ID] ASC)
);

