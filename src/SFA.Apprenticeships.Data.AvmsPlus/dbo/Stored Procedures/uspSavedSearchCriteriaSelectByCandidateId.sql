CREATE PROCEDURE [dbo].[uspSavedSearchCriteriaSelectByCandidateId]  
(  
@CandidateId int,  
@PageIndex int =  1,        
@PageSize int = 20,        
@IsSortAsc bit= 1,        
@SortByField nvarchar(100) = 'Name'  
)  
AS  
BEGIN  
-- SET NOCOUNT ON added to prevent extra result sets from  
-- interfering with SELECT statements.  
SET NOCOUNT ON;  
  
/*********Set Page Number**********************************************/  
declare @StartRowNo int  
declare @EndRowNo int  
set @StartRowNo =((@PageIndex-1)* @PageSize)+1   
set @EndRowNo =(@PageIndex * @PageSize)      
/***********************************************************************/  
  
/**************Total Number of Rows*************************************/  
declare @TotalRows int  
select @TotalRows= count(1)   
FROM   
 SavedSearchCriteria   
 --LEFT JOIN ApprenticeshipFramework ON [ApprenticeshipFramework].[ApprenticeshipFrameworkId] = SavedSearchCriteria.[ApprenticeshipFrameworkId]  
 --LEFT JOIN ApprenticeshipOccupation ON [ApprenticeshipOccupation].[ApprenticeshipOccupationId] = SavedSearchCriteria.[ApprenticeshipOccupationId]  
  
  
WHERE   
 CandidateId=@CandidateId   
/***********************************************************************/  
  
/*********set the order by**********************************************/  
declare @OrderBywithSort varchar(500)  
if @SortByField <> ''  
Begin   
 if @IsSortAsc = 1 BEGIN set  @SortByField = @SortByField + ' Asc' END                
 if @IsSortAsc = 0 BEGIN  set  @SortByField = @SortByField + ' desc' END       
End  
/***********************************************************************/  
SELECT *,@TotalRows as TotalRows FROM  
(   
SELECT  ROW_NUMBER() OVER( ORDER BY   
 Case when @SortByField='Name Asc'  then [Name]  End ASC,  
    Case when @SortByField='Name Desc'  then [Name]  End DESC  
 ) as RowNum,        
  SavedSearchCriteria.[SavedSearchCriteriaId],  
  SavedSearchCriteria.[CandidateId],  
  SavedSearchCriteria.[Name],  
  SavedSearchCriteria.[SearchType],  
  --SavedSearchCriteria.[Region],  
  SavedSearchCriteria.[Postcode],  
  SavedSearchCriteria.[Longitude],  
  SavedSearchCriteria.[Latitude],  
  SavedSearchCriteria.[GeocodeEasting],  
  SavedSearchCriteria.[GeocodeNorthing],  
  SavedSearchCriteria.[DistanceFromPostcode],  
  SavedSearchCriteria.[MinWages],  
  SavedSearchCriteria.[MaxWages],  
  SavedSearchCriteria.[VacancyReferenceNumber],  
  0 as EmployerId,  
  SavedSearchCriteria.[Employer] as EmployerName,  
  SavedSearchCriteria.[TrainingProvider],  
  SavedSearchCriteria.[Keywords],  
  SavedSearchCriteria.[DateSearched],  
  SavedSearchCriteria.[BackgroundSearch],  
  SavedSearchCriteria.[AlertSent],  
  SavedSearchCriteria.[CountBackgroundMatches],  
  VacancyPostedSince,  
  SavedSearchCriteria.[ApprenticeshipTypeId]   
 FROM   
 -- left joins should be inner joins when there is info in savedSearchCriteria.Employer  
 SavedSearchCriteria 
 WHERE  
   CandidateId=@CandidateId ) as DerivedTable  
 
 WHERE RowNum BETWEEN @StartRowNo AND @EndRowNo  
END