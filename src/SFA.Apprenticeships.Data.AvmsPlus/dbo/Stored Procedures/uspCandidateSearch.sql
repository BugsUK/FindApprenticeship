CREATE PROCEDURE [dbo].[uspCandidateSearch]
@FirstName VARCHAR (70), @SurName VARCHAR (70), @DateOfBirth DATETIME, @GeographicRegionCodeName VARCHAR(3), @PostCode VARCHAR (50), @PageIndex INT=1, @PageSize INT=20, @IsSortAsc BIT=1, @SortByField NVARCHAR (100)='surname'
AS
BEGIN         
IF @GeographicRegionCodeName = '' SET @GeographicRegionCodeName = null    
if @FirstName = '' SET @FirstName = null
if @SurName = '' set @SurName = null
if @PostCode = '' set @PostCode = null
if @DateOfBirth = '' set @DateOfBirth = null

      
                
SET NOCOUNT ON                  
/*********Set Page Number**********************************************/            
declare @StartRowNo int            
declare @EndRowNo int            
set @StartRowNo =((@PageIndex-1)* @PageSize)+1             
set @EndRowNo =(@PageIndex * @PageSize)                
/***********************************************************************/            
             
		   
/*********set the order by**********************************************/            
			            
declare @OrderBywithSort varchar(500)            
			            
if @IsSortAsc = 1 BEGIN set  @SortByField = @SortByField + ' Asc' END            
if @IsSortAsc = 0 BEGIN  set  @SortByField = @SortByField + ' desc' END       
/***********************************************************************/            


SELECT MyTable.*  FROM            
	( 
	select 
	TotalRows = count(1) over(),
	ROW_NUMBER() OVER( ORDER BY             
			  Case when @SortByField='surname Asc'  then p.SurName   End ASC,            
			  Case when @SortByField='surname desc'  then p.SurName End DESC,
			  Case when @SortByField='surname Asc'  then p.FirstName End ASC,            
			  Case when @SortByField='surname desc'  then p.FirstName End DESC,
			 
			  Case when @SortByField='FirstName Asc'  then p.FirstName   End ASC,            
			  Case when @SortByField='FirstName desc'  then p.FirstName End DESC,
			  Case when @SortByField='DateOfBirth Asc'  then c.Dateofbirth   End ASC,            
			  Case when @SortByField='DateOfBirth desc'  then c.Dateofbirth End DESC,
			  Case when @SortByField='PostCode Asc'  then c.Postcode   End ASC,            
			  Case when @SortByField='PostCode desc'  then c.Postcode End DESC,
			  Case when @SortByField='Region Asc'  then vwRLA.GeographicFullName   End ASC,            
			  Case when @SortByField='Region desc'  then vwRLA.GeographicFullName End DESC,
			  Case when @SortByField='AppsMade Asc'  then COUNT(a.ApplicationId)   End ASC,            
			  Case when @SortByField='AppsMade desc'  then COUNT(a.ApplicationId) End DESC,
			  Case when @SortByField='UnsucceApps Asc'  then ISNULL((SELECT COUNT(ApplicationId) FROM APPLICATION app LEFT OUTER JOIN ApplicationStatusType ast ON app.ApplicationStatusTypeId= ast.ApplicationStatusTypeId WHERE  app.CandidateId = c.candidateId AND 	ast.CodeName = 'Rej' GROUP BY app.candidateId),0)   End ASC,            
			  Case when @SortByField='UnsucceApps desc'  then ISNULL((SELECT COUNT(ApplicationId) FROM APPLICATION app LEFT OUTER JOIN ApplicationStatusType ast ON app.ApplicationStatusTypeId= ast.ApplicationStatusTypeId WHERE  app.CandidateId = c.candidateId AND 	ast.CodeName = 'Rej' GROUP BY app.candidateId),0) End DESC,
	 		  c.CandidateID
			  ) as RowNum,                 
			c.CandidateId,
			ISNULL(p.FirstName,'') AS 'FirstName',
			ISNULL(p.SurName,'') AS 'SurName',
			c.Dateofbirth,
			ISNULL(c.Postcode,'') AS 'PostCode',
			vwRLA.GeographicRegionID,
			vwRLA.GeographicCodeName,
			vwRLA.GeographicShortName,
			vwRLA.GeographicFullName,
			COUNT(a.ApplicationId) AS 'NoOfAppsMade',	
			ISNULL((SELECT COUNT(ApplicationId) FROM APPLICATION app LEFT OUTER JOIN ApplicationStatusType ast ON app.ApplicationStatusTypeId= ast.ApplicationStatusTypeId WHERE  app.CandidateId = c.candidateId AND 	ast.CodeName = 'Rej' GROUP BY app.candidateId),0)
			AS UnSuccessAPP 
			FROM Candidate c
			INNER JOIN dbo.LocalAuthority LA ON c.LocalAuthorityId = LA.LocalAuthorityId
			JOIN vwRegionsAndLocalAuthority vwRLA on vwRLA.LocalAuthorityId = LA.LocalAuthorityId
			 --INNER JOIN dbo.LocalAuthority LA ON c.LocalAuthorityId = LA.LocalAuthorityId
			 --INNER JOIN dbo.LocalAuthorityGroupMembership LAGM ON LA.LocalAuthorityId = LAGM.LocalAuthorityID
			 --INNER JOIN dbo.LocalAuthorityGroup LAG ON LAGM.LocalAuthorityGroupID = LAG.LocalAuthorityGroupID
			 --INNER JOIN dbo.LocalAuthorityGroupType LAGT ON LAG.LocalAuthorityGroupTypeID = LAGT.LocalAuthorityGroupTypeID
			 --AND LocalAuthorityGroupTypeName = N'Region' 
			INNER join person p ON p.PersonId= c.PersonId    
			--INNER JOIN LSCRegion R ON c.LSCRegionId = r.LSCRegionId
			LEFT OUTER JOIN [APPLICATION] a ON c.CandidateId  = a.CandidateId 
			WHERE 
				(p.FirstName LIKE  @FirstName + '%' OR @FirstName IS NULL)
			AND (p.SurName LIKE  @SurName + '%' OR @SurName IS NULL)
			AND (convert(varchar(20),DateOfBirth,105) = convert(varchar(20),@DateOfBirth,105) OR @DateOfBirth IS NULL)
			AND (vwRLA.GeographicCodeName = @GeographicRegionCodeName OR @GeographicRegionCodeName IS NULL)
			AND (c.PostCode LIKE  @PostCode + '%' OR @PostCode IS NULL)
			AND (c.CandidateStatusTypeId = 2 or c.CandidateStatusTypeId = 1)
		
			GROUP BY c.CandidateId,p.FirstName,p.SurName,c.dateofbirth,c.Postcode,vwRLA.GeographicRegionID,vwRLA.GeographicCodeName,vwRLA.GeographicShortName,vwRLA.GeographicFullName					  
	) as MyTable            
			   
			WHERE RowNum BETWEEN @StartRowNo AND @EndRowNo            
            		ORDER BY RowNum
             
                  
SET NOCOUNT OFF                  
END