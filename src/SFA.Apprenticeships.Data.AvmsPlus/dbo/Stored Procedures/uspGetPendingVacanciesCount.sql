CREATE PROCEDURE [dbo].[uspGetPendingVacanciesCount]
    @ManagingAreaId INT = 0 ,
    @count INT OUTPUT
AS 
    BEGIN

        SET NOCOUNT ON
	
        DECLARE @Status AS CHAR(3)
        SET @Status = 'SUB'

        SELECT  @count = COUNT(VacancyId)
        FROM    Vacancy vac
                INNER JOIN [VacancyOwnerRelationship] vpr ON vac.[VacancyOwnerRelationshipId] = vpr.[VacancyOwnerRelationshipId]
                INNER JOIN [ProviderSite] tp ON vpr.[ProviderSiteID] = tp.ProviderSiteID
                INNER JOIN VacancyStatusType vst ON vac.VacancyStatusId = vst.VacancyStatusTypeId
        WHERE   UPPER(vst.CodeName) = @Status
                AND tp.ManagingAreaId = @ManagingAreaId

        SET NOCOUNT OFF
    END