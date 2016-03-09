CREATE PROCEDURE [dbo].[uspGetEmployersWithoutTrainingPartnersCount]
(
	@ManagingAreaId int = 0,
	@NoDays int = -1,
	@count int output
)
AS
BEGIN
SET NOCOUNT ON
	
	DECLARE @CodeName as char(3)
	Set @CodeName = 'CRE'
	Declare @DelCodeName as char(3)
	set @DelCodeName = 'DEL';
	
	WITH EmployersAndManagingAreas AS 
	(SELECT EmployerID, ISNULL(ManagingAreaID, (SELECT ManagingAreaID FROM dbo.vwManagingAreas 
	WHERE ManagingAreaCodeName = 'NAC')) AS ManagingArea
	FROM employer LEFT JOIN vwManagingAreaAndLocalAuthority MALA
	ON dbo.Employer.LocalAuthorityId = MALA.LocalAuthorityID)
	
		select	@count = Count(emp.EmployerId)
		from	EmployersAndManagingAreas ema
				JOIN Employer emp ON ema.EmployerID = emp.EmployerId
				inner join EmployerHistory eh
					on emp.employerid = eh.employerid
				inner join EmployerHistoryEventType ehet
					on eh.[event] = ehet.employerhistoryeventtypeid
		where	emp.employerid not in	(
											select	vpr.employerid 
											from	[VacancyOwnerRelationship] vpr
													inner join vacancyprovisionrelationshipStatustype vprst
														on vprst.vacancyprovisionrelationshipStatustypeID = vpr.StatusTypeID
											where	UPPER(vprst.CodeName) <> @DelCodeName
										)			
                and DATEADD(d, @NoDays, eh.[date]) <  getdate()
				and Upper(ehet.codename) = @CodeName		
				and ema.ManagingArea = @ManagingAreaId
				and emp.EmployerStatusTypeId = 1
				and eh.Date = (SELECT MAX(eh1.Date) FROM EmployerHistory eh1 WHERE eh1.EmployerId = emp.EmployerId  AND eh1.Event=1 AND DATEADD(d, @NoDays, eh1.[date]) <  getdate());
     
	SET NOCOUNT OFF

END