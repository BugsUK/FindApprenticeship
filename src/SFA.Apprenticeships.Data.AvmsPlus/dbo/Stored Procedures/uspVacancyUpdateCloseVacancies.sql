Create Procedure [dbo].[uspVacancyUpdateCloseVacancies]
As
Begin
	Declare @cnt int
	Declare @VacStatusID int
	Declare @vId int
	Declare @vstId int
	Declare @acDate DateTime
	Declare @TEMPVacsTable Table
	(
		vacId int,
		vacStatusTypeId int,
		appClosingDate DateTime
	)

	set @cnt = 0
	Select @VacStatusID=VacancyStatusTypeId From VacancyStatusType Where CodeName ='Cld'

	Declare curVac Cursor For 
		Select	VacancyId, VacancyStatusId, ApplicationClosingDate from Vacancy 
		Where	ApplicationClosingDate <= GetDate()
		And		VacancyStatusId <> @VacStatusID
	Open curVac
	Fetch Next From curVac Into @vId, @vstId, @acDate
	
	While @@FETCH_STATUS = 0
	Begin
		
		Update	Vacancy
		Set		VacancyStatusId = @VacStatusID
		Where	VacancyId = @vId
		set @cnt = @cnt + 1
		Fetch Next From curVac Into @vId, @vstId, @acDate
	End
	Close curVac
	Deallocate curVac
	Return @cnt
End