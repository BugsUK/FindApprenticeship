-- =============================================
-- uspVacancyGetExpired
-- =============================================

Create Procedure [dbo].[uspVacancyGetExpired]
	@expiryDate DateTime = GetDate
As
Begin
	Select VacancyId From Vacancy v
	Inner Join VacancyStatusType t On v.VacancyStatusId = t.VacancyStatusTypeId
	Where ApplicationClosingDate < @expiryDate
	And   t.Codename = 'Lve'
End