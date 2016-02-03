CREATE TABLE [dbo].[ReportDefinitions] (
    [RoleTypeID]             INT            NOT NULL,
    [DisplayName]            NVARCHAR (100) COLLATE Latin1_General_CI_AS NOT NULL,
    [HTMLVersion]            NVARCHAR (100) COLLATE Latin1_General_CI_AS NULL,
    [CSVVersion]             NVARCHAR (100) COLLATE Latin1_General_CI_AS NULL,
    [SummaryVersion]         NVARCHAR (100) COLLATE Latin1_General_CI_AS NULL,
    [Description]            NVARCHAR (MAX) COLLATE Latin1_General_CI_AS NULL,
    [GeographicSectionName]  NVARCHAR (100) COLLATE Latin1_General_CI_AS NULL,
    [DateSectionName]        NVARCHAR (100) COLLATE Latin1_General_CI_AS NULL,
    [ApplicationSectionName] NVARCHAR (100) COLLATE Latin1_General_CI_AS NULL,
    [Flags]                  NVARCHAR (255) COLLATE Latin1_General_CI_AS NULL,
    CONSTRAINT [PK_ReportDefinitions] PRIMARY KEY CLUSTERED ([RoleTypeID] ASC, [DisplayName] ASC),
    CONSTRAINT [FK_ReportDefinitions_RoleTYpe] FOREIGN KEY ([RoleTypeID]) REFERENCES [dbo].[RoleType] ([RoleTypeId]) NOT FOR REPLICATION
);



