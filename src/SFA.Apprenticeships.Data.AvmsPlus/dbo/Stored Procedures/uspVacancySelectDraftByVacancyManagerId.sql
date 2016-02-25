CREATE PROCEDURE [dbo].[uspVacancySelectDraftByVacancyManagerId]  
 @RelationshipId Int,  
 @VacancyManagerId int = null,  
 @SortByField NVarchar(20),  
 @IsSortAsc Bit,  
 @PageNumber Int,  
 @PageSize Int,  
 @Impersonated Bit,
 @TotalRows Int OUTPUT  
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
  AND (  
   (@VacancyManagerId IS null) OR (dbo.Vacancy.VacancyManagerId = @VacancyManagerId)  
  ) AND  
   dbo.Vacancy.VacancyStatusId IN  
      (Select VacancyStatusTypeId   
       From VacancyStatusType (NOLOCK)  
       Where CodeName = N'Dft' Or CodeName = N'Ref' Or CodeName = N'Sub')  
  
 /*---------> End */  
  
  
  
 /*---------> Get the desired page of data for this query */  
 If @SortByField = N'VacancyTitle'      
 Begin  
  If @IsSortAsc = 1  
   Set @SortByField = 'Title Asc'  
  Else  
   Set @SortByField = 'Title Desc'  
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
      dbo.Vacancy.Town Asc  
     ) AS 'RowNumber',   
     dbo.Vacancy.VacancyId, dbo.Vacancy.Title,  
     dbo.Vacancy.VacancyStatusId  
    FROM dbo.[VacancyOwnerRelationship] (NOLOCK) INNER JOIN  
     dbo.Vacancy ON dbo.[VacancyOwnerRelationship].[VacancyOwnerRelationshipId] = dbo.Vacancy.[VacancyOwnerRelationshipId]  
     INNER JOIN ProviderSite PS ON PS.ProviderSiteId = dbo.[VacancyOwnerRelationship].ProviderSiteId  
     LEFT JOIN PROVIDER CO ON Vacancy.contractownerid = CO.ProviderID  
    WHERE (dbo.Vacancy.[VacancyOwnerRelationshipId] = @RelationshipID)  
    AND (@Impersonated = 1 OR (PS.TrainingProviderStatusTypeId != 3 AND (co.ProviderStatusTypeID IS NULL OR co.ProviderStatusTypeID != 3))) 
    AND (  
   (@VacancyManagerId IS null) OR (dbo.Vacancy.VacancyManagerId = @VacancyManagerId)  
  ) AND  
     dbo.Vacancy.VacancyStatusId IN  
      (Select VacancyStatusTypeId   
       From VacancyStatusType (NOLOCK)  
       Where CodeName = N'Dft' Or CodeName = N'Ref' Or CodeName = N'Sub')  
  ) X  
  WHERE (RowNumber > @FirstRecord) AND (RowNumber <= @LastRecord)  
   
 /*---------> End */  
END