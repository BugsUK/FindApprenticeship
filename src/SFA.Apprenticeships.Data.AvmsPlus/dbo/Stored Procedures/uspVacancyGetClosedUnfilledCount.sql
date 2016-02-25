CREATE  PROCEDURE [dbo].[uspVacancyGetClosedUnfilledCount]
    (
      @ManagingAreaId INT ,
      @daysFromClosingDateForVacancyNotFilled INT                  
               
    )
AS 
    BEGIN                          
                      
        SET NOCOUNT ON      
    
        DECLARE @totalRows INT                        
    
        IF ( @ManagingAreaId = 0 ) 
            SET @ManagingAreaId = NULL                   
        
        SELECT  @totalRows = COUNT(1)
        FROM    Vacancy vac
                INNER JOIN [VacancyOwnerRelationship] vpr ON vac.[VacancyOwnerRelationshipId] = vpr.[VacancyOwnerRelationshipId]
                INNER JOIN Employer emp ON vpr.EmployerId = emp.EmployerId
                JOIN [ProviderSite] tp ON vpr.[ProviderSiteID] = tp.ProviderSiteID
                INNER JOIN dbo.VacancyStatusType VST ON vac.VacancyStatusId = VST.VacancyStatusTypeId
        WHERE   VST.CodeName IN ( 'Lve', 'Cld' )
                AND DATEDIFF(dd, ApplicationClosingDate, GETDATE()) > @daysFromClosingDateForVacancyNotFilled
                AND vac.NumberofPositions != ( SELECT   COUNT(*)
                                               FROM     dbo.[Application]
                                                        INNER JOIN dbo.ApplicationStatusType ON dbo.ApplicationStatusType.ApplicationStatusTypeId = dbo.[Application].ApplicationStatusTypeId
                                               WHERE    dbo.ApplicationStatusType.FullName = 'Successful'
                                                        AND [Application].VacancyId = vac.VacancyId
                                             )
                AND tp.ManagingAreaID = @ManagingAreaId           
 
/***********************************************************************/                  
          
        RETURN @totalRows    
        
        SET NOCOUNT OFF                    
                       
    END