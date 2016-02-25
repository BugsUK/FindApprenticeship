CREATE PROCEDURE [dbo].[uspVacancySelectApproachingClosingDateWithNoApps]
	@EmployerId Int,
	@TrainingProviderId Int,
	@vacancyManagerId int = null,
	@SortByField NVarchar(20),
	@IsSortAsc Bit,
	@PageNumber Int,
	@PageSize Int,
	@ClosingDateWithinNumberOfDaysBeforeVacancyClosure INT,
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

if @SortByField = 'VacancyTitle'
BEGIN  
  SET @SortByField = 'Title'
END
if @SortByField = 'EmployerName'
BEGIN  
  SET @SortByField = 'FullName'
END

if @IsSortAsc = 1 BEGIN set  @SortByField = @SortByField + ' Asc' END            
if @IsSortAsc = 0 BEGIN  set  @SortByField = @SortByField + ' desc' END       
/***********************************************************************/            
 
 --Print @SortByField
 
 IF ISNULL(@TrainingProviderId, 0) <> 0  
 BEGIN  
   
  /*---------> Get the total number of vacancies for this query */  
  SELECT @TotalRows = COUNT(*)  
   FROM dbo.[ProviderSite]   
    INNER JOIN dbo.[VacancyOwnerRelationship] ON dbo.[ProviderSite].ProviderSiteID = dbo.[VacancyOwnerRelationship].[ProviderSiteID]   
    INNER JOIN dbo.Vacancy ON dbo.[VacancyOwnerRelationship].[VacancyOwnerRelationshipId] = dbo.Vacancy.[VacancyOwnerRelationshipId]  
   WHERE (dbo.[VacancyOwnerRelationship].ManagerIsEmployer = 0)  
    AND dbo.[ProviderSite].ProviderSiteID = @TrainingProviderId   
    AND ApplyOutsideNAVMS = 0  
    AND VacancyStatusId IN (SELECT dbo.VacancyStatusType.VacancyStatusTypeId   
           FROM dbo.VacancyStatusType  
           WHERE dbo.VacancyStatusType.CodeName = N'Lve')  
    AND DATEDIFF(dd,dbo.fnx_RemoveTime(GETDATE()),Vacancy.ApplicationClosingDate) <= @ClosingDateWithinNumberOfDaysBeforeVacancyClosure  
    AND DATEDIFF(dd,dbo.fnx_RemoveTime(GETDATE()),Vacancy.ApplicationClosingDate) >= 0  
    AND (SELECT COUNT(*) FROM dbo.[Application] WHERE dbo.[Application].VacancyId = dbo.Vacancy.VacancyId) = 0  
	AND (
		(@vacancyManagerId IS NULL) OR
		(Vacancy.VacancyManagerId = @vacancyManagerId)
	)
  /*---------> End */  
  
 
      
  /*---------> Get the desired page of data for this query */  
  SELECT *  
   FROM (  
    SELECT ROW_NUMBER() OVER  
      (   
       ORDER BY  
--       Case When @IsSortAsc = 1 Then @SortByField End Asc,   
--       Case When @IsSortAsc = 0 Then @SortByField End Desc  
        Case when @SortByField='Title Asc'  then  dbo.Vacancy.Title   End ASC,            
			  Case when @SortByField='Title desc'  then  dbo.Vacancy.Title End DESC,
        Case when @SortByField='FullName Asc'  then  dbo.Employer.FullName   End ASC,            
			  Case when @SortByField='FullName desc'  then  dbo.Employer.FullName End DESC

      ) AS 'RowNumber',  
      dbo.Vacancy.VacancyId,  
      dbo.Vacancy.ApprenticeshipType AS VacancyType,  
      dbo.Vacancy.Title,   
      dbo.Vacancy.VacancyStatusId,   
      --dbo.TrainingProvider.FullName,  
            dbo.Employer.FullName,    
      dbo.Vacancy.NumberofPositions,  
      dbo.Vacancy.ApplicationClosingDate,  
      dbo.ApprenticeshipFramework.ApprenticeshipFrameworkId,  
      dbo.ApprenticeshipFramework.ApprenticeshipOccupationId,  
      dbo.ApprenticeshipFramework.FullName AS FrameworkName,  
      (SELECT COUNT(*) FROM dbo.[Application] WHERE dbo.[Application].VacancyId = dbo.Vacancy.VacancyId) AS NumberOfApplications  
     FROM dbo.Vacancy   
      LEFT JOIN dbo.ApprenticeshipFramework ON dbo.Vacancy.ApprenticeshipFrameworkId = dbo.ApprenticeshipFramework.ApprenticeshipFrameworkId   
      INNER JOIN dbo.[VacancyOwnerRelationship] ON dbo.Vacancy.[VacancyOwnerRelationshipId] = dbo.[VacancyOwnerRelationship].[VacancyOwnerRelationshipId]   
      INNER JOIN dbo.[ProviderSite] ON dbo.[VacancyOwnerRelationship].[ProviderSiteID] = dbo.[ProviderSite].ProviderSiteID  
            INNER JOIN dbo.Employer ON dbo.Employer.EmployerId = dbo.[VacancyOwnerRelationship].EmployerId  
     WHERE (dbo.[VacancyOwnerRelationship].ManagerIsEmployer = 0)  
      AND VacancyStatusId IN (SELECT dbo.VacancyStatusType.VacancyStatusTypeId   
             FROM dbo.VacancyStatusType  
             WHERE dbo.VacancyStatusType.CodeName = N'Lve')  
      AND dbo.[ProviderSite].ProviderSiteID = @TrainingProviderId   
      AND ApplyOutsideNAVMS = 0  
      AND DATEDIFF(dd,dbo.fnx_RemoveTime(getDate()),Vacancy.ApplicationClosingDate) <= @ClosingDateWithinNumberOfDaysBeforeVacancyClosure  
      AND DATEDIFF(dd,dbo.fnx_RemoveTime(getDate()),Vacancy.ApplicationClosingDate) >= 0  
      AND (SELECT COUNT(*) FROM dbo.[Application] WHERE dbo.[Application].VacancyId = dbo.Vacancy.VacancyId) = 0  
	  AND (
		(@vacancyManagerId IS NULL) OR
		(Vacancy.VacancyManagerId = @vacancyManagerId)
		)
    ) X1  
    WHERE (RowNumber > @FirstRecord) AND (RowNumber <= @LastRecord)  
  /*---------> End */  
    
 END  
 ELSE  
 BEGIN  
  
  /* Vacancies with no applications approaching their closing date */  
  SELECT @TotalRows = COUNT(*)  
   FROM dbo.Employer  
    INNER JOIN dbo.[VacancyOwnerRelationship] ON dbo.Employer.EmployerId = dbo.[VacancyOwnerRelationship].EmployerId   
    INNER JOIN dbo.Vacancy ON dbo.[VacancyOwnerRelationship].[VacancyOwnerRelationshipId] = dbo.Vacancy.[VacancyOwnerRelationshipId]  
   WHERE (dbo.[VacancyOwnerRelationship].ManagerIsEmployer = 1)  
    AND dbo.Employer.EmployerId = @EmployerId   
    AND ApplyOutsideNAVMS = 0  
    AND VacancyStatusId IN (SELECT dbo.VacancyStatusType.VacancyStatusTypeId   
           FROM dbo.VacancyStatusType  
           WHERE dbo.VacancyStatusType.CodeName = N'Lve')  
    AND DATEDIFF(dd,dbo.fnx_RemoveTime(GETDATE()), Vacancy.ApplicationClosingDate) <= @ClosingDateWithinNumberOfDaysBeforeVacancyClosure  
    AND DATEDIFF(dd,dbo.fnx_RemoveTime(GETDATE()), Vacancy.ApplicationClosingDate) >= 0  
    AND (SELECT COUNT(*) FROM dbo.[Application] WHERE dbo.[Application].VacancyId = dbo.Vacancy.VacancyId) = 0  
  /*---------> End */  
  
  
      
  /*---------> Get the desired page of data for this query */  
  SELECT *  
   FROM (  
    SELECT ROW_NUMBER() OVER  
      (   
       ORDER BY  
       Case When @IsSortAsc = 1 Then @SortByField End Asc,   
       Case When @IsSortAsc = 0 Then @SortByField End Desc  
      ) AS 'RowNumber',  
      dbo.Vacancy.VacancyId,  
      dbo.Vacancy.ApprenticeshipType AS VacancyType,  
      dbo.Vacancy.Title,   
      dbo.Vacancy.VacancyStatusId,   
      dbo.Employer.FullName,  
      dbo.Vacancy.NumberofPositions,  
      dbo.Vacancy.ApplicationClosingDate,  
      dbo.ApprenticeshipFramework.ApprenticeshipFrameworkId,  
      dbo.ApprenticeshipFramework.ApprenticeshipOccupationId,  
      dbo.ApprenticeshipFramework.FullName AS FrameworkName,  
      (SELECT COUNT(*) FROM dbo.[Application] WHERE dbo.[Application].VacancyId = dbo.Vacancy.VacancyId) AS NumberOfApplications  
     FROM dbo.Vacancy   
      LEFT JOIN dbo.ApprenticeshipFramework ON dbo.Vacancy.ApprenticeshipFrameworkId = dbo.ApprenticeshipFramework.ApprenticeshipFrameworkId   
      INNER JOIN dbo.[VacancyOwnerRelationship] ON dbo.Vacancy.[VacancyOwnerRelationshipId] = dbo.[VacancyOwnerRelationship].[VacancyOwnerRelationshipId]   
      INNER JOIN dbo.Employer ON dbo.[VacancyOwnerRelationship].EmployerId = dbo.Employer.EmployerId  
     WHERE (dbo.[VacancyOwnerRelationship].ManagerIsEmployer = 1)  
      AND dbo.Employer.EmployerId = @EmployerId   
      AND ApplyOutsideNAVMS = 0  
      AND VacancyStatusId IN (SELECT dbo.VacancyStatusType.VacancyStatusTypeId   
             FROM dbo.VacancyStatusType  
             WHERE dbo.VacancyStatusType.CodeName = N'Lve')  
      AND DATEDIFF(dd,dbo.fnx_RemoveTime(GETDATE()), Vacancy.ApplicationClosingDate) <= @ClosingDateWithinNumberOfDaysBeforeVacancyClosure   
      AND DATEDIFF(dd,dbo.fnx_RemoveTime(GETDATE()), Vacancy.ApplicationClosingDate) >= 0  
      AND (SELECT COUNT(*) FROM dbo.[Application] WHERE dbo.[Application].VacancyId = dbo.Vacancy.VacancyId) = 0  
    ) X1  
    WHERE (RowNumber > @FirstRecord) AND (RowNumber <= @LastRecord)  
  /*---------> End */  
    
 END  
END