create PROCEDURE [dbo].[uspCandidateSelectByReferralPoints]                      
(                
@Threshold INT=3,                
@RegionId int,                
@PageIndex int =  1,            
@PageSize int = 20,            
@IsSortAsc bit= 1,            
@SortByField nvarchar(100) = 'CandidateName',  
@RowCount INT OUT                
)                
AS                  
BEGIN         
IF @RegionId = 0 SET @RegionId = null    
      
                
SET NOCOUNT ON                  
/*********Set Page Number**********************************************/            
declare @StartRowNo int            
declare @EndRowNo int            
set @StartRowNo =((@PageIndex-1)* @PageSize)+1             
set @EndRowNo =(@PageIndex * @PageSize)                
/***********************************************************************/            
            
/**************Total Number of Rows*************************************/            
declare @TotalRows int            
SELECT @TotalRows=COUNT(1)  
FROM     
  (SELECT Candidate.CandidateId     
 FROM Candidate  
                  INNER JOIN dbo.LocalAuthority LA ON Candidate.LocalAuthorityId = LA.LocalAuthorityId
				  INNER JOIN dbo.LocalAuthorityGroupMembership LAGM ON LA.LocalAuthorityId = LAGM.LocalAuthorityID
				  INNER JOIN dbo.LocalAuthorityGroup LAG ON LAGM.LocalAuthorityGroupID = LAG.LocalAuthorityGroupID
				  INNER JOIN dbo.LocalAuthorityGroupType LAGT ON LAG.LocalAuthorityGroupTypeID = LAGT.LocalAuthorityGroupTypeID
				  AND LocalAuthorityGroupTypeName = N'Region'
				  INNER join person ON dbo.Person.PersonId= candidate.PersonId    
				  INNER JOIN [APPLICATION] ON dbo.[Application].CandidateId = candidate.CandidateId  
				  inner join ApplicationUnsuccessfulReasonType
					on ApplicationUnsuccessfulReasonType.ApplicationUnsuccessfulReasonTypeId = [APPLICATION].UnsuccessfulReasonId
					and ApplicationUnsuccessfulReasonType.ReferralPoints != 0
					LEFT outer JOIN dbo.ApplicationHistory ON   
				  dbo.[Application].ApplicationId = dbo.ApplicationHistory.ApplicationId   
				  AND ApplicationHistoryEventTypeId = 1      -- Status Change  
				  AND ApplicationHistoryEventSubTypeId =  5  -- ApplicationstatusType.Rejected 
			 WHERE Candidate.ReferralPoints >= @Threshold    
				AND (LAG.LocalAuthorityGroupID = @RegionId OR @RegionId IS NULL)
				AND Candidate.CandidateId NOT IN (  
					 SELECT CandidateId  
					 FROM [APPLICATION]		
					 INNER JOIN [dbo].[ApplicationStatusType] ON [ApplicationStatusType].ApplicationStatusTypeId = [Application].ApplicationStatusTypeId  
					 WHERE dbo.ApplicationStatusType.FullName = 'Successful'  
					 GROUP BY CandidateId)  
			 GROUP BY Candidate.CandidateId,Person.FirstName + ' ' + Person.Surname,
				dateofbirth,Candidate.ReferralPoints,  
			  Case WHEN DATEdiff(hh,getdate(),Candidate.LockedForSupportUntil)<=24 AND DATEdiff(hh,getdate(),Candidate.LockedForSupportUntil) >=1/60      
			  THEN Candidate.BeingSupportedBy ELSE '' END,Candidate.LockedForSupportUntil   
)AS total   
/***********************************************************************/            
			SET @RowCount = @TotalRows         

IF @PageSize != -1
BEGIN
                
		   
			/*********set the order by**********************************************/            
			            
			declare @OrderBywithSort varchar(500)            
			            
			 if @IsSortAsc = 1 BEGIN set  @SortByField = @SortByField + ' Asc' END            
			 if @IsSortAsc = 0 BEGIN  set  @SortByField = @SortByField + ' desc' END       
			/***********************************************************************/            


			            
			SELECT *,@TotalRows  as 'TotalRows'   
			FROM            
			(             
			 SELECT  ROW_NUMBER() OVER( ORDER BY             
			  Case when @SortByField='CandidateName Asc'  then  Person.FirstName + ' ' + Person.Surname  End ASC,            
			  Case when @SortByField='CandidateName desc'  then  Person.FirstName + ' ' + Person.Surname  End DESC, 
			  
			  Case when @SortByField='LastDateOfRejection Asc'  then MAX(ApplicationHistoryEventDate)  End ASC,            
			  Case when @SortByField='LastDateOfRejection desc'  then MAX(ApplicationHistoryEventDate)  End DESC, 
			  
			  Case when @SortByField='Age Asc'  then datediff(yy,dateofbirth ,getdate())  End ASC,            
			  Case when @SortByField='Age desc'  then datediff(yy,dateofbirth ,getdate())  End DESC,
			  
			   Case when @SortByField='Category Asc'  then dbo.fnx_GetCategory(Candidate.CandidateId)  End ASC,            
			  Case when @SortByField='Category desc'  then dbo.fnx_GetCategory(Candidate.CandidateId)  End DESC, 
			  
			   Case when @SortByField='BeingSupportBy Asc'  then Case WHEN DATEdiff(hh,getdate(),Candidate.LockedForSupportUntil)<=24 AND DATEdiff(hh,getdate(),Candidate.LockedForSupportUntil) >=1/60      
			  THEN Candidate.BeingSupportedBy ELSE '' END  End ASC,            
			  Case when @SortByField='BeingSupportBy desc'  then Case WHEN DATEdiff(hh,getdate(),Candidate.LockedForSupportUntil)<=24 AND DATEdiff(hh,getdate(),Candidate.LockedForSupportUntil) >=1/60      
			  THEN Candidate.BeingSupportedBy ELSE '' END  End DESC 
			             
			  ) as RowNum,                 
			 Candidate.CandidateId,
			 datediff(yy,dateofbirth ,getdate()) as Age,
			 MAX(ApplicationHistoryEventDate) AS 'LastDateOfRejection' , 
			 ISNULL(Person.FirstName + ' ' + Person.Surname,'') AS CandidateName,      
			 (Case WHEN DATEdiff(hh,getdate(),Candidate.LockedForSupportUntil)<=24 AND DATEdiff(hh,getdate(),Candidate.LockedForSupportUntil) >=1/60      
			  THEN Candidate.BeingSupportedBy ELSE '' END) as OpenedBy,      
			 Candidate.LockedForSupportUntil,
			dbo.fnx_GetCategory(Candidate.CandidateId) as 'Category'
			,Candidate.ReferralPoints
			 FROM Candidate
			     INNER JOIN dbo.LocalAuthority LA ON Candidate.LocalAuthorityId = LA.LocalAuthorityId
				 INNER JOIN dbo.LocalAuthorityGroupMembership LAGM ON LA.LocalAuthorityId = LAGM.LocalAuthorityID
				 INNER JOIN dbo.LocalAuthorityGroup LAG ON LAGM.LocalAuthorityGroupID = LAG.LocalAuthorityGroupID
				 INNER JOIN dbo.LocalAuthorityGroupType LAGT ON LAG.LocalAuthorityGroupTypeID = LAGT.LocalAuthorityGroupTypeID
				 AND LocalAuthorityGroupTypeName = N'Region'  
				  INNER join person ON dbo.Person.PersonId= candidate.PersonId    
				  INNER JOIN [APPLICATION] ON dbo.[Application].CandidateId = candidate.CandidateId  
				  inner join ApplicationUnsuccessfulReasonType
					on ApplicationUnsuccessfulReasonType.ApplicationUnsuccessfulReasonTypeId = [APPLICATION].UnsuccessfulReasonId
					and ApplicationUnsuccessfulReasonType.ReferralPoints != 0
					LEFT outer JOIN dbo.ApplicationHistory ON   
				  dbo.[Application].ApplicationId = dbo.ApplicationHistory.ApplicationId   
				  AND ApplicationHistoryEventTypeId = 1      -- Status Change  
				  AND ApplicationHistoryEventSubTypeId =  5  -- ApplicationstatusType.Rejected 
			 WHERE Candidate.ReferralPoints >= @Threshold    
				AND (LAG.LocalAuthorityGroupID = @RegionId OR @RegionId IS NULL)
				AND Candidate.CandidateId NOT IN (  
					 SELECT CandidateId  
					 FROM [APPLICATION]		
					 INNER JOIN [dbo].[ApplicationStatusType] ON [ApplicationStatusType].ApplicationStatusTypeId = [Application].ApplicationStatusTypeId  
					 WHERE dbo.ApplicationStatusType.FullName = 'Successful'  
					 GROUP BY CandidateId)  
			 GROUP BY Candidate.CandidateId,Person.FirstName + ' ' + Person.Surname,
				dateofbirth,Candidate.ReferralPoints,  
			  Case WHEN DATEdiff(hh,getdate(),Candidate.LockedForSupportUntil)<=24 AND DATEdiff(hh,getdate(),Candidate.LockedForSupportUntil) >=1/60      
			  THEN Candidate.BeingSupportedBy ELSE '' END,Candidate.LockedForSupportUntil
			  
			 ) as DerivedTable            
			   
			WHERE RowNum BETWEEN @StartRowNo AND @EndRowNo            
            
END              
                  
SET NOCOUNT OFF                  
END