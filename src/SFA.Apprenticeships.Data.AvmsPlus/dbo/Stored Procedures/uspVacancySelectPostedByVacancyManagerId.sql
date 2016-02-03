CREATE PROCEDURE [dbo].[uspVacancySelectPostedByVacancyManagerId]    
@RelationshipId INT, 
@VacancyManagerId int = null, 
@SortByField NVARCHAR (20), 
@IsSortAsc BIT, @PageNumber INT, 
@PageSize INT,
@Impersonated Bit, 
@TotalRows INT OUTPUT    
AS    
BEGIN    
 -- SET NOCOUNT ON added to prevent extra result sets from    
 -- interfering with SELECT statements.    
 SET NOCOUNT ON;    
    
    
 /*---------> Initialisation */    
 Declare @FirstRecord Int    
 Declare @LastRecord Int    
    
 /* Ensure we have a valid page number */    
 If @PageNumber < 1    
  Set @PageNumber = 1    
    
 /* Ensure we have a valid page size */    
 If @PageSize < 1    
  Set @PageSize = 10    
    
 /* Get the start and end row number for the     
  requested page */    
 Set @FirstRecord = (@PageNumber - 1) * @PageSize    
 Set @LastRecord = @FirstRecord + @PageSize    
 /*---------> End */    
    
    
 /*---------> Get the total number of vacancies for this query */    
 SELECT @TotalRows = Count(*)    
  FROM dbo.[VacancyOwnerRelationship] (NOLOCK) INNER JOIN    
   dbo.Vacancy ON dbo.[VacancyOwnerRelationship].[VacancyOwnerRelationshipId] = dbo.Vacancy.[VacancyOwnerRelationshipId]    
  INNER JOIN ProviderSite PS ON PS.ProviderSiteId = dbo.[VacancyOwnerRelationship].ProviderSiteId  
     LEFT JOIN PROVIDER CO ON Vacancy.contractownerid = CO.ProviderID  
  WHERE (dbo.Vacancy.[VacancyOwnerRelationshipId] = @RelationshipID)   
 AND (@Impersonated=1 OR (PS.TrainingProviderStatusTypeId != 3 AND (co.ProviderStatusTypeID IS NULL OR co.ProviderStatusTypeID != 3)))
 AND  
  (  
   (@vacancyManagerId IS NULL) OR  
   (Vacancy.VacancyManagerId = @vacancyManagerId)  
  ) AND   
   dbo.Vacancy.VacancyStatusId IN    
      (Select VacancyStatusTypeId     
       From VacancyStatusType (NOLOCK)    
       Where CodeName = N'Lve' Or CodeName = N'Com' Or CodeName = N'Cld' Or CodeName = N'Wdr')    
    
 /*---------> End */    
    
    
 /*---------> Get the desired page of data for this query */    
 If @SortByField = N'VacancyTitle'        
 Begin    
  If @IsSortAsc = 1    
   Set @SortByField = 'Title Asc'    
  Else    
   Set @SortByField = 'Title Desc'    
 End       
 Else    
 If @SortByField = N'Location'    
 Begin         
  If @IsSortAsc = 1     
   Set  @SortByField = 'Town Asc'    
  Else    
   Set  @SortByField = 'Town Desc'    
 End    
    
 SELECT *    
  FROM (    
   SELECT ROW_NUMBER() OVER    
     (     
      ORDER BY    
      Case When @SortByField = 'Title Asc' Then dbo.Vacancy.Title End Asc,     
      Case When @SortByField = 'Title Desc' Then dbo.Vacancy.Title End Desc,    
      Case When @SortByField = 'Town Asc' Then dbo.Vacancy.Town End Asc,    
      Case When @SortByField = 'Town Desc' Then dbo.Vacancy.Town End Desc,    
      dbo.Vacancy.ApplicationClosingDate Asc    
     ) AS 'RowNumber',     
     dbo.Vacancy.VacancyId, dbo.Vacancy.Title,    
     dbo.Vacancy.Town, dbo.Vacancy.ApplicationClosingDate,     
     dbo.Vacancy.VacancyStatusId,     
     dbo.Vacancy.NumberofPositions,   
     dbo.Vacancy.VacancyLocationTypeId,  
    
     /* Calculate unfilled positions for this vacancy */    
     (Select CASE     
        WHEN dbo.Vacancy.NumberofPositions - (Count(*) + ISNULL(dbo.Vacancy.NoOfOfflineApplicants,0)) >= 0 THEN dbo.Vacancy.NumberofPositions - (Count(*) + ISNULL(dbo.Vacancy.NoOfOfflineApplicants,0))   
        ELSE 0    
       END    
       From dbo.Application (NOLOCK) INNER JOIN    
        dbo.ApplicationStatusType ON dbo.Application.ApplicationStatusTypeId = dbo.ApplicationStatusType.ApplicationStatusTypeId    
       Where (dbo.ApplicationStatusType.CodeName = N'SUC') AND (dbo.Application.VacancyId = dbo.Vacancy.VacancyId)) UnfilledPositions,    
    
  
     /* Get number of applications for this vacancy */    
     (Select Count(*)     
       From dbo.[Application] (NOLOCK)     
       Where dbo.[Application].VacancyId = dbo.Vacancy.VacancyId     
       And dbo.[Application].ApplicationStatusTypeId NOT IN     
        (Select ApplicationStatusTypeId    
         From dbo.ApplicationStatusType (NOLOCK)    
         Where dbo.ApplicationStatusType.Codename = 'DRF')) Applications    
   
    
    FROM dbo.[VacancyOwnerRelationship] (NOLOCK) INNER JOIN    
     dbo.Vacancy ON dbo.[VacancyOwnerRelationship].[VacancyOwnerRelationshipId] = dbo.Vacancy.[VacancyOwnerRelationshipId]    
  INNER JOIN ProviderSite PS ON PS.ProviderSiteId = dbo.[VacancyOwnerRelationship].ProviderSiteId  
  LEFT JOIN PROVIDER CO ON Vacancy.contractownerid = CO.ProviderID  
    WHERE (dbo.Vacancy.[VacancyOwnerRelationshipId] = @RelationshipID)   
 AND (@Impersonated=1 OR (PS.TrainingProviderStatusTypeId != 3 AND (co.ProviderStatusTypeID IS NULL OR co.ProviderStatusTypeID != 3)))
 AND (  
  (@vacancyManagerId IS NULL) OR  
  (dbo.Vacancy.VacancyManagerId = @vacancyManagerId)  
 ) AND    
     dbo.Vacancy.VacancyStatusId IN    
      (Select VacancyStatusTypeId     
       From VacancyStatusType (NOLOCK)    
       Where CodeName = N'Lve' Or CodeName = N'Com' Or CodeName = N'Cld' Or CodeName = N'Wdr')    
  ) X    
  WHERE (RowNumber > @FirstRecord) AND (RowNumber <= @LastRecord)    
  
 /*---------> End */    
END