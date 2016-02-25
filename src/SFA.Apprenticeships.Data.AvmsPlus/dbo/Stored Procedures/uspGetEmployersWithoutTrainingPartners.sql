CREATE PROCEDURE [dbo].[uspGetEmployersWithoutTrainingPartners]    
(    
 @ManagingArea int = 0,    
 @NoDays int = -1,    
 @PageIndex int =  1,          
 @PageSize int = 20,          
 @IsSortAsc bit= 1,          
 @SortByField nvarchar(100) = 'DateRegistered'      
)    
AS    
BEGIN    
    
 SET NOCOUNT ON    
    
 /*********Set Page Number**********************************************/          
 declare @StartRowNo int          
 declare @EndRowNo int          
 set @StartRowNo =((@PageIndex-1)* @PageSize)+1           
 set @EndRowNo =(@PageIndex * @PageSize)              
 /***********************************************************************/          
           
 /**************Total Number of Rows*************************************/          
 declare @TotalRows int      
 /* call existing sp to retrieve the total number of rows */    
 exec uspGetEmployersWithoutTrainingPartnersCount @ManagingArea, @NoDays, @TotalRows output    
 /***********************************************************************/          
           
 /*********set the order by**********************************************/          
 declare @OrderBywithSort varchar(500)          
           
  if @IsSortAsc = 1 BEGIN set  @SortByField = @SortByField + ' Asc' END          
  if @IsSortAsc = 0 BEGIN set  @SortByField = @SortByField + ' desc' END     
 /***********************************************************************/       
     
 DECLARE @CodeName as char(3)    
 Set @CodeName = 'CRE'    
 Declare @DelCodeName as char(3)    
 set @DelCodeName = 'DEL'   ; 
    
 -- create a temporary table and populate with the Employer ids that     
 -- have status created and in the relevent region    
 declare @employertemp table(EmployerID int, LSCRegion int);    
   
   	WITH EmployersAndManagingAreas AS 
	(SELECT EmployerID, ISNULL(ManagingAreaID, (SELECT ManagingAreaID FROM dbo.vwManagingAreas 
	WHERE ManagingAreaCodeName = 'NAC')) AS ManagingArea
	FROM employer LEFT JOIN vwManagingAreaAndLocalAuthority MALA
	ON dbo.Employer.LocalAuthorityId = MALA.LocalAuthorityID)
	
	 
   insert into @employertemp( EmployerID, LSCRegion)    
  select ema.EmployerId as EmployerID,    
    ema.ManagingArea as RegionId    
  from EmployersAndManagingAreas ema 
  INNER JOIN employer emp ON ema.EmployerID = emp.EmployerId   

    inner join EmployerHistory eh    
     on emp.employerid = eh.employerid    
    inner join EmployerHistoryEventType ehet    
     on eh.[event] = ehet.employerhistoryeventtypeid    
  where emp.employerid not in (    
           select vpr.employerid     
           from [VacancyOwnerRelationship] vpr    
             inner join vacancyprovisionrelationshipStatustype vprst    
              on vprst.vacancyprovisionrelationshipStatustypeID = vpr.StatusTypeID    
           where UPPER(vprst.CodeName) <> @DelCodeName    
          )       
    and DATEADD(d, @NoDays, eh.[date]) <  getdate()    
    and Upper(ehet.codename) = @CodeName      
    and ema.ManagingArea = @ManagingArea    
    and emp.EmployerStatusTypeId = 1
	and eh.Date = (SELECT MAX(eh1.Date) FROM EmployerHistory eh1 WHERE eh1.EmployerId = emp.EmployerId  AND eh1.Event=1  AND DATEADD(d, @NoDays, eh1.[date]) <  getdate())      
    
    
  -- return data    
  select *     
  from (    
     select *,    
       ROW_NUMBER() OVER( ORDER BY           
        Case when @SortByField='DateRegistered Asc'  then DateRegistered  End ASC,          
        Case when @SortByField='DateRegistered desc'  then DateRegistered  End DESC,          
        Case when @SortByField='FullName Asc'  then FullName  End ASC,          
        Case when @SortByField='FullName desc'  then FullName  End DESC,          
        Case when @SortByField='Town Asc'  then Town  End ASC,          
        Case when @SortByField='Town desc'  then Town  End DESC,          
        Case when @SortByField='OpenedBy Asc'  then OpenedBy  End ASC,          
        Case when @SortByField='OpenedBy desc'  then OpenedBy  End DESC          
       ) as RowNum,    
       @TotalRows  as 'TotalRows'     
    
     from      
       (    
           
       select eh.[date] as DateRegistered,     
         emp.fullname as FullName,    
         emp.town as Town,    
         emp.employerid as EmployerID,    
         (Case WHEN emp.LockedForSupportUntil > getdate() THEN emp.BeingSupportedBy ELSE '' END) as OpenedBy    
           
       from employer emp    
         inner join @employertemp et    
          on emp.employerid = et.employerid    
         inner join EmployerHistory eh    
          on emp.employerid = eh.employerid    
         inner join EmployerHistoryEventType ehet    
          on eh.[event] = ehet.employerhistoryeventtypeid and CodeName = 'CRE'    
        WHERE eh.Date = (SELECT MAX(eh1.Date) FROM EmployerHistory eh1 WHERE eh1.EmployerId = emp.EmployerId AND eh1.Event=1  AND DATEADD(d, @NoDays, eh1.[date]) <  getdate())    
        ) x     
       ) z    
  WHERE RowNum BETWEEN @StartRowNo AND @EndRowNo;     
    
         
 SET NOCOUNT OFF    
    
END