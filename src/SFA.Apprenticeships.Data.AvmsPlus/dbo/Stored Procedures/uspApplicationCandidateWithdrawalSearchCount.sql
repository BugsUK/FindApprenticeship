CREATE PROCEDURE [dbo].[uspApplicationCandidateWithdrawalSearchCount]
		@count INT OUTPUT, 
		@providerId int,
		@VacancyManagerId int = null
AS
	BEGIN
		SET NOCOUNT ON;

		select @count = count(app.ApplicationId)
		from [application] app  
		inner join vacancy v on app.VacancyId=v.VacancyId
		inner join [VacancyOwnerRelationship] vpr on vpr.[VacancyOwnerRelationshipId]=v.[VacancyOwnerRelationshipId]
		inner join [ProviderSite] prov on prov.ProviderSiteID=vpr.[ProviderSiteID]
		inner join Candidate c on c.CandidateId=app.CandidateId
		inner join Person p on c.PersonId=p.PersonId
		inner join Employer e on e.EmployerId=vpr.EmployerId
		inner join ApprenticeshipFramework af on af.ApprenticeshipFrameworkId=v.ApprenticeshipFrameworkId
		where 
		app.ApplicationStatusTypeId=4 
		AND
		(
			@VacancyManagerId IS NULL OR v.VacancyManagerId = @VacancyManagerId
		)
		and prov.ProviderSiteID=@providerId
		-- NOTE: CHECK IF ACK = 0 OR 1
		and app.WithdrawalAcknowledged=0

	SET NOCOUNT OFF
	END