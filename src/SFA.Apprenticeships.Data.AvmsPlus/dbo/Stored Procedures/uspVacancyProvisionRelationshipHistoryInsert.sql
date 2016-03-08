
CREATE PROCEDURE [dbo].[uspVacancyProvisionRelationshipHistoryInsert]  
@vacancyProvisionRelationshipId INT,
@userName NVARCHAR(50),
@historyDate DATETIME,
@eventTypeId INT,
@eventSubTypeId INT,
@Comments NVARCHAR(4000)
AS  
BEGIN  
    SET NOCOUNT ON
   
    BEGIN TRY  
        INSERT INTO
            [dbo].[VacancyOwnerRelationshipHistory]
            (
                [VacancyOwnerRelationshipId],
                [UserName],
                [Date],
                [EventTypeId],
                [EventSubTypeId],
                [Comments]
            )
        VALUES
            (
                @vacancyProvisionRelationshipId,
                @userName,
                @historyDate,
                @eventTypeId,
                @eventSubTypeId,
                @Comments
            )
    END TRY  
    BEGIN CATCH  
        EXEC RethrowError;  
    END CATCH  

    SET NOCOUNT OFF  
END