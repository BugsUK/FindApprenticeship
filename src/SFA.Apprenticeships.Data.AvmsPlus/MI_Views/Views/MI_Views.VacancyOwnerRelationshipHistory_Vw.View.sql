CREATE VIEW [MI_Views].[VacancyOwnerRelationshipHistory_Vw]
AS
     SELECT VacancyOwnerRelationshipHistoryId,
            VacancyOwnerRelationshipId,
            UserName,
            Date,
            EventTypeId,
            EventSubTypeId,
            Comments
     FROM dbo.VacancyOwnerRelationshipHistory;