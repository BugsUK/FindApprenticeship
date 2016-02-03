CREATE TABLE [dbo].[VacancyOwnerRelationshipHistory] (
    [VacancyOwnerRelationshipHistoryId] INT            IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [VacancyOwnerRelationshipId]        INT            NOT NULL,
    [UserName]                          NVARCHAR (50)  NULL,
    [Date]                              DATETIME       NOT NULL,
    [EventTypeId]                       INT            NOT NULL,
    [EventSubTypeId]                    INT            NULL,
    [Comments]                          VARCHAR (4000) NULL,
    CONSTRAINT [PK_VacancyProvisionRelationshipHistory] PRIMARY KEY CLUSTERED ([VacancyOwnerRelationshipHistoryId] ASC) WITH (FILLFACTOR = 90) ON [PRIMARY],
    CONSTRAINT [FK_VacancyProvisionRelationshipHistory_VacancyProvisionRelationship] FOREIGN KEY ([VacancyOwnerRelationshipId]) REFERENCES [dbo].[VacancyOwnerRelationship] ([VacancyOwnerRelationshipId]),
    CONSTRAINT [FK_VacancyProvisionRelationshipHistory_VacancyProvisionRelationshipHistoryEventType] FOREIGN KEY ([EventTypeId]) REFERENCES [dbo].[VacancyProvisionRelationshipHistoryEventType] ([VacancyProvisionRelationshipHistoryEventTypeId])
);

