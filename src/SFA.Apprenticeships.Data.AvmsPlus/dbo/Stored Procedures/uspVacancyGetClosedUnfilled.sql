CREATE PROCEDURE [dbo].[uspVacancyGetClosedUnfilled]
    (
      @ManagingArea INT ,
      @daysFromClosingDateForVacancyNotFilled INT ,
      @daysFromClosingDateFor0ApplicationVacancies INT ,
      @numberOfDaysForFilledVacanciesWithOpenApplications INT ,
      @noApplicationsOnly BIT = 1 ,
      @PageIndex INT = 1 ,
      @PageSize INT = 20 ,
      @IsSortAsc BIT = 1 ,
      @SortByField NVARCHAR(100) = 'VacancyManager'                     
    )
AS 
    BEGIN                        
            
       
/*********Set Page Number**********************************************/                
        DECLARE @StartRowNo INT                
        DECLARE @EndRowNo INT                
        SET @StartRowNo = ( ( @PageIndex - 1 ) * @PageSize ) + 1                 
        SET @EndRowNo = ( @PageIndex * @PageSize )                    
/***********************************************************************/       
/*********set the order by**********************************************/                
                
        DECLARE @OrderBywithSort VARCHAR(500)                
                
        IF @IsSortAsc = 1 
            BEGIN
                SET @SortByField = @SortByField + ' Asc'
            END                
        IF @IsSortAsc = 0 
            BEGIN
                SET @SortByField = @SortByField + ' desc'
            END           
/***********************************************************************/                
      
        IF ( @ManagingArea = 0 ) 
            SET @ManagingArea = NULL            
      
        
        SELECT  MyTable.*
        FROM    ( SELECT    TotalRows = COUNT(1) OVER ( ) ,
                                                        ROW_NUMBER() OVER ( ORDER BY CASE
                                                              WHEN @SortByField = 'ClosingDate Asc'
                                                              THEN ApplicationClosingDate
                                                              END ASC, CASE
                                                              WHEN @SortByField = 'ClosingDate desc'
                                                              THEN ApplicationClosingDate
                                                              END DESC, CASE
                                                              WHEN @SortByField = 'VacancyManager Asc'
                                                              THEN ( CASE
                                                              WHEN vpr.ManagerIsEmployer = 1
                                                              THEN emp.FullName
                                                              ELSE tp.FullName
                                                              END )
                                                              END ASC, CASE
                                                              WHEN @SortByField = 'VacancyManager desc'
                                                              THEN ( CASE
                                                              WHEN vpr.ManagerIsEmployer = 1
                                                              THEN emp.FullName
                                                              ELSE tp.FullName
                                                              END )
                                                              END DESC, CASE
                                                              WHEN @SortByField = 'VacancyPartner Asc'
                                                              THEN ( CASE
                                                              WHEN vpr.ManagerIsEmployer = 0
                                                              THEN emp.FullName
                                                              ELSE tp.FullName
                                                              END )
                                                              END ASC, CASE
                                                              WHEN @SortByField = 'VacancyPartner desc'
                                                              THEN ( CASE
                                                              WHEN vpr.ManagerIsEmployer = 0
                                                              THEN emp.FullName
                                                              ELSE tp.FullName
                                                              END )
                                                              END DESC, CASE
                                                              WHEN @SortByField = 'VacancyTitle Asc'
                                                              THEN Title
                                                              END ASC, CASE
                                                              WHEN @SortByField = 'VacancyTitle desc'
                                                              THEN Title
                                                              END DESC, CASE
                                                              WHEN @SortByField = 'OpenedBy Asc'
                                                              THEN ( CASE
                                                              WHEN vac.LockedForSupportUntil > GETDATE()
                                                              THEN vac.BeingSupportedBy
                                                              ELSE ''
                                                              END )
                                                              END ASC, CASE
                                                              WHEN @SortByField = 'OpenedBy desc'
                                                              THEN ( CASE
                                                              WHEN vac.LockedForSupportUntil > GETDATE()
                                                              THEN vac.BeingSupportedBy
                                                              ELSE ''
                                                              END )
                                                              END DESC ) AS RowNum ,
                                                        emp.EmployerId ,
                                                        tp.ProviderSiteID ,
                                                        ( CASE
                                                              WHEN vpr.ManagerIsEmployer = 1
                                                              THEN emp.FullName
                                                              ELSE tp.FullName
                                                          END ) AS VacancyManager ,
                                                        ( CASE
                                                              WHEN vpr.ManagerIsEmployer = 0
                                                              THEN emp.FullName
                                                              ELSE tp.FullName
                                                          END ) AS VacancyPartner ,
                                                        vac.Title ,
                                                        vac.ApplicationClosingDate ,
                                                        vac.VacancyId ,
                                                        ( CASE
                                                              WHEN vac.LockedForSupportUntil > GETDATE()
                                                              THEN vac.BeingSupportedBy
                                                              ELSE ''
                                                          END ) AS OpenedBy
                  FROM                                  Vacancy vac
                                                        INNER JOIN [VacancyOwnerRelationship] vpr ON vac.[VacancyOwnerRelationshipId] = vpr.[VacancyOwnerRelationshipId]
                                                        INNER JOIN Employer emp ON vpr.EmployerId = emp.EmployerId
                                                        INNER JOIN [ProviderSite] tp ON vpr.[ProviderSiteID] = tp.ProviderSiteID
                                                        INNER JOIN dbo.VacancyStatusType VST ON vac.VacancyStatusId = VST.VacancyStatusTypeId                  
                      
   --WHERE VST.FullName='Closed'     
                  WHERE                                 VST.CodeName IN (
                                                        'Lve', 'Cld' )
                                                        AND DATEDIFF(dd,
                                                              ApplicationClosingDate,
                                                              GETDATE()) > @daysFromClosingDateForVacancyNotFilled
                                                        AND vac.NumberofPositions != ( SELECT
                                                              COUNT(*)
                                                              FROM
                                                              dbo.[Application]
                                                              INNER JOIN dbo.ApplicationStatusType ON dbo.ApplicationStatusType.ApplicationStatusTypeId = dbo.[Application].ApplicationStatusTypeId
                                                              WHERE
                                                              dbo.ApplicationStatusType.FullName = 'Successful'
                                                              AND [Application].VacancyId = vac.VacancyId
                                                              )
                                                        AND tp.ManagingAreaID = @ManagingArea
                ) AS MyTable
        WHERE   RowNum BETWEEN @StartRowNo AND @EndRowNo                
                            
      
        SET NOCOUNT OFF                  
                     
    END