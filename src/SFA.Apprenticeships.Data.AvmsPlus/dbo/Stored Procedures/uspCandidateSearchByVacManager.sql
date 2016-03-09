CREATE PROCEDURE [dbo].[uspCandidateSearchByVacManager]
@FirstName VARCHAR (70), 
@SurName VARCHAR (70), 
@ProviderSiteId INT, 
@RecruitmentAgencyId int,   
@PageIndex INT=1, 
@PageSize INT=20, 
@IsSortAsc BIT=1, 
@SortByField NVARCHAR (100)='surname'
AS
BEGIN  


DECLARE @NewStatusId int
DECLARE @AppliedStatusId int
DECLARE @SuccessStatusId int
DECLARE @DraftStatusId int


SELECT @NewStatusId = applicationStatustypeiD 
FROM applicationStatustype 
WHERE CODENAME = 'NEW'


SELECT @AppliedStatusId = applicationStatustypeiD 
FROM applicationStatustype 
WHERE CODENAME = 'APP'


SELECT @SuccessStatusId = applicationStatustypeiD 
FROM applicationStatustype 
WHERE CODENAME = 'SUC'

SELECT @DraftStatusId = applicationStatustypeiD 
FROM applicationStatustype 
WHERE CODENAME = 'DRF'
       
IF @ProviderSiteId = 0 SET @ProviderSiteId = null 
IF @RecruitmentAgencyId = 0 SET @RecruitmentAgencyId = null       
if @FirstName = '' SET @FirstName = null
if @SurName = '' set @SurName = null


      
                
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
		SELECT 
			TotalRows = count(1) over(),
			ROW_NUMBER() OVER( ORDER BY             
					Case when @SortByField='surname Asc'  then p.SurName   End ASC,            
					Case when @SortByField='surname desc'  then p.SurName End DESC,
					Case when @SortByField='surname Asc'  then p.FirstName End ASC,            
					Case when @SortByField='surname desc'  then p.FirstName End DESC,
					Case when @SortByField='FirstName Asc'  then p.FirstName   End ASC,            
					Case when @SortByField='FirstName desc'  then p.FirstName End DESC,
					Case when @SortByField='PostCode Asc'  then c.Postcode   End ASC,            
					Case when @SortByField='PostCode desc'  then c.Postcode End DESC,
					Case when @SortByField='AppsMade Asc'  then ISNULL((SELECT COUNT(ApplicationId)FROM APPLICATION app LEFT OUTER JOIN ApplicationStatusType ast ON app.ApplicationStatusTypeId= ast.ApplicationStatusTypeId  INNER JOIN [VACANCY] Vapp ON Vapp.VACANCYID = app.VACANCYID INNER JOIN [VacancyOwnerRelationship] VPRapp ON VPRapp.[VacancyOwnerRelationshipId] = Vapp.[VacancyOwnerRelationshipId] WHERE  app.CandidateId = c.candidateId AND 	ast.ApplicationStatusTypeId NOT IN  (@DraftStatusId) and VPRapp.[ProviderSiteID] = @ProviderSiteId GROUP BY app.candidateId),0)   End ASC,            
					Case when @SortByField='AppsMade Asc'  then ISNULL((SELECT COUNT(ApplicationId)FROM APPLICATION app LEFT OUTER JOIN ApplicationStatusType ast ON app.ApplicationStatusTypeId= ast.ApplicationStatusTypeId  INNER JOIN [VACANCY] Vapp ON Vapp.VACANCYID = app.VACANCYID INNER JOIN [VacancyOwnerRelationship] VPRapp ON VPRapp.[VacancyOwnerRelationshipId] = Vapp.[VacancyOwnerRelationshipId] WHERE  app.CandidateId = c.candidateId AND 	ast.ApplicationStatusTypeId NOT IN  (@DraftStatusId) and VPRapp.[ProviderSiteID] = @ProviderSiteId GROUP BY app.candidateId),0)   End ASC,            
					Case when @SortByField='AppsMade desc'  then COUNT(a.ApplicationId) End DESC,
					Case when @SortByField='UnsucceApps Asc'  then ISNULL((SELECT COUNT(ApplicationId) FROM APPLICATION app LEFT OUTER JOIN ApplicationStatusType ast ON app.ApplicationStatusTypeId= ast.ApplicationStatusTypeId INNER JOIN [VACANCY] Vapp ON Vapp.VACANCYID = app.VACANCYID INNER JOIN [VacancyOwnerRelationship] VPRapp ON VPRapp.[VacancyOwnerRelationshipId] = Vapp.[VacancyOwnerRelationshipId] where  app.CandidateId = c.candidateId AND 	ast.ApplicationStatusTypeId IN  (@SuccessStatusId) and VPRapp.[ProviderSiteID] = @ProviderSiteId GROUP BY app.candidateId),0)   End ASC,            
					Case when @SortByField='UnsucceApps desc'  then ISNULL((SELECT COUNT(ApplicationId) FROM APPLICATION app LEFT OUTER JOIN ApplicationStatusType ast ON app.ApplicationStatusTypeId= ast.ApplicationStatusTypeId INNER JOIN [VACANCY] Vapp ON Vapp.VACANCYID = app.VACANCYID INNER JOIN [VacancyOwnerRelationship] VPRapp ON VPRapp.[VacancyOwnerRelationshipId] = Vapp.[VacancyOwnerRelationshipId] where  app.CandidateId = c.candidateId AND 	ast.ApplicationStatusTypeId IN  (@SuccessStatusId) and VPRapp.[ProviderSiteID] = @ProviderSiteId GROUP BY app.candidateId),0) End DESC
			  ) as RowNum,                 
				c.CandidateId,
				ISNULL(p.FirstName,'') AS 'FirstName',
				ISNULL(p.SurName,'') AS 'SurName',
				c.Dateofbirth,
				ISNULL(c.Postcode,'') AS 'PostCode',
				ISNULL(LAG.FullName,'') AS 'RegionName',
				--COUNT(a.ApplicationId) AS 'NoOfAppsMade',	
				ISNULL((SELECT COUNT(ApplicationId)FROM APPLICATION app LEFT OUTER JOIN ApplicationStatusType ast ON app.ApplicationStatusTypeId= ast.ApplicationStatusTypeId  INNER JOIN [VACANCY] Vapp ON Vapp.VACANCYID = app.VACANCYID INNER JOIN [VacancyOwnerRelationship] VPRapp ON VPRapp.[VacancyOwnerRelationshipId] = Vapp.[VacancyOwnerRelationshipId] WHERE  app.CandidateId = c.candidateId AND 	ast.ApplicationStatusTypeId NOT IN  (@DraftStatusId) and VPRapp.[ProviderSiteID] = @ProviderSiteId GROUP BY app.candidateId),0)
				AS NoOfAppsMade ,
				--VPR.TrainingProviderId,
				ISNULL((SELECT COUNT(ApplicationId) FROM APPLICATION app LEFT OUTER JOIN ApplicationStatusType ast ON app.ApplicationStatusTypeId= ast.ApplicationStatusTypeId INNER JOIN [VACANCY] Vapp ON Vapp.VACANCYID = app.VACANCYID INNER JOIN [VacancyOwnerRelationship] VPRapp ON VPRapp.[VacancyOwnerRelationshipId] = Vapp.[VacancyOwnerRelationshipId] where  app.CandidateId = c.candidateId AND 	ast.ApplicationStatusTypeId IN  (@SuccessStatusId) and VPRapp.[ProviderSiteID] = @ProviderSiteId GROUP BY app.candidateId),0)
				AS UnSuccessAPP 
		FROM Candidate c 
			INNER JOIN dbo.LocalAuthority LA ON c.LocalAuthorityId = LA.LocalAuthorityId
			INNER JOIN dbo.LocalAuthorityGroupMembership LAGM ON LA.LocalAuthorityId = LAGM.LocalAuthorityID
			INNER JOIN dbo.LocalAuthorityGroup LAG ON LAGM.LocalAuthorityGroupID = LAG.LocalAuthorityGroupID
			INNER JOIN dbo.LocalAuthorityGroupType LAGT ON LAG.LocalAuthorityGroupTypeID = LAGT.LocalAuthorityGroupTypeID AND LocalAuthorityGroupTypeName = N'Region'
			INNER join person p ON p.PersonId= c.PersonId    
			LEFT OUTER JOIN [APPLICATION] a ON c.CandidateId  = a.CandidateId  and a.ApplicationStatusTypeId <> 1        
			INNER JOIN [VACANCY] V ON V.VACANCYID = A.VACANCYID
			INNER JOIN [VacancyOwnerRelationship] VPR ON VPR.[VacancyOwnerRelationshipId] = V.[VacancyOwnerRelationshipId]
		WHERE 
			 VPR.[ProviderSiteID] = @ProviderSiteId
			AND (@RecruitmentAgencyId IS NULL OR V.VacancyManagerID = @RecruitmentAgencyId)
			AND (p.FirstName LIKE  @FirstName + '%' OR @FirstName IS NULL)
			AND (p.SurName LIKE  @SurName + '%' OR @SurName IS NULL)
			AND (VPR.ManagerIsEmployer=0)
			AND (c.CandidateStatusTypeId = 2 )        			
		GROUP BY c.CandidateId,p.FirstName,p.SurName,c.dateofbirth,c.Postcode,LAG.FullName --,a.ApplicationStatusTypeId	--,VPR.TrainingProviderId	
		) as MyTable            
	WHERE RowNum BETWEEN @StartRowNo AND @EndRowNo            
            
             
                  
SET NOCOUNT OFF                  
END