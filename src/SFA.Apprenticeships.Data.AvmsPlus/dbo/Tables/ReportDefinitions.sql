CREATE TABLE [dbo].[ReportDefinitions] (
    [RoleTypeID]             INT            NOT NULL,
    [DisplayName]            NVARCHAR (100) NOT NULL,
    [HTMLVersion]            NVARCHAR (100) NULL,
    [CSVVersion]             NVARCHAR (100) NULL,
    [SummaryVersion]         NVARCHAR (100) NULL,
    [Description]            NVARCHAR (MAX) NULL,
    [GeographicSectionName]  NVARCHAR (100) NULL,
    [DateSectionName]        NVARCHAR (100) NULL,
    [ApplicationSectionName] NVARCHAR (100) NULL,
    [Flags]                  NVARCHAR (255) NULL,
    CONSTRAINT [PK_ReportDefinitions] PRIMARY KEY CLUSTERED ([RoleTypeID] ASC, [DisplayName] ASC),
    CONSTRAINT [FK_ReportDefinitions_RoleTYpe] FOREIGN KEY ([RoleTypeID]) REFERENCES [dbo].[RoleType] ([RoleTypeId])
);

