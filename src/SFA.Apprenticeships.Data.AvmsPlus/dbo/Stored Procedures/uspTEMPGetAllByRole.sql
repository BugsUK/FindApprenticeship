CREATE PROCEDURE [dbo].[uspTEMPGetAllByRole]
	@RoleType NVARCHAR (40)
AS
BEGIN  
	SET NOCOUNT ON;  
   
	IF @RoleType = 'Candidate'  
	BEGIN  
		SELECT  
			[candidate].[CandidateId] AS 'UserId',  
			[candidate].[PersonId] AS 'PersonId',  
			[Person].[FirstName] + ' ' + [Person].[Surname] +' ('+ CAST([candidate].[CandidateId] AS varchar(20)) +')' AS 'FirstName'  
		FROM [dbo].[Candidate] [candidate]  
			INNER JOIN  [dbo].[Person] [Person] ON  
				[candidate].[PersonId] = [Person].[PersonId]  
		WHERE [Person].[FirstName] like 'Test%'
		ORDER BY [Person].[FirstName], [Person].[Surname]
	END  

	IF @RoleType = 'RecruitmentAgency'
	BEGIN
		SELECT  
			[ProviderSite].ProviderSiteID AS 'UserId',  
			[ProviderSite].[FullName] AS 'FirstName'  
		FROM [dbo].[ProviderSite]
		WHERE [ProviderSite].IsRecruitmentAgency = 1
		ORDER BY [ProviderSite].[FullName]
	END
  
	IF @RoleType = 'TrainingProvider'  
	BEGIN  
		SELECT  
			[ProviderSite].ProviderSiteID AS 'UserId',  
			[ProviderSite].[TradingName] + ' (' + CAST([ProviderSite].EDSURN as varchar(20))+ ')'  AS 'FirstName'  
		FROM [dbo].[ProviderSite]
		ORDER BY [ProviderSite].[TradingName]
	END  

	IF @RoleType = 'ProfileManager'  
	BEGIN  
		SELECT  
			[ProviderSite].ProviderSiteID AS 'UserId',  
			[ProviderSite].[FullName] AS 'FirstName'  
		FROM [dbo].[ProviderSite]
		ORDER BY [ProviderSite].[FullName]
	END  
  
	IF @RoleType = 'TrainingProviderSuperUser'  
	BEGIN  
		SELECT  
			[ProviderSite].ProviderSiteID AS 'UserId',  
			[ProviderSite].[FullName] +' ('+ CAST([ProviderSite].ProviderSiteID AS varchar(20)) +')' AS 'FirstName'  
		FROM [dbo].[ProviderSite]
		ORDER BY [ProviderSite].[FullName]
	END 
	
	IF @RoleType = 'TrainingProviderInterfaceAdmin'  
	BEGIN  
		SELECT  
			[ProviderSite].ProviderSiteID AS 'UserId',  
			[ProviderSite].[FullName] AS 'FirstName'  
		FROM [dbo].[ProviderSite]
		ORDER BY [ProviderSite].[FullName]
	END  
  
	IF @RoleType = 'Employer'  
	BEGIN  
		SELECT TOP 5000 
			[Employer].[EmployerId] AS 'UserId',  
			[Employer].[FullName] +' ('+ CAST([Employer].[EmployerId] AS varchar(20)) +')' AS 'FirstName'  
		FROM [dbo].[Employer] [Employer]
		ORDER BY [Employer].[FullName]
	END  
  
	IF @RoleType = 'EmployerSuperUser'  
	BEGIN  
		SELECT  TOP 5000  
			[Employer].[EmployerId] AS 'UserId',  
			[Employer].[FullName] AS 'FirstName'  
		FROM [dbo].[Employer] [Employer]
		ORDER BY [Employer].[FullName]
	END
	
	IF @RoleType = 'EmployerInterfaceAdmin'  
	BEGIN  
		SELECT   TOP 5000
			[Employer].[EmployerId] AS 'UserId',  
			[Employer].[FullName] AS 'FirstName'  
		FROM [dbo].[Employer] [Employer]
		ORDER BY [Employer].[FullName]
	END
	
	IF @RoleType = N'Stakeholder'
	BEGIN
		SELECT  dbo.StakeHolder.StakeHolderID AS UserId, 
				dbo.StakeHolder.PersonId AS PersonId,
				dbo.Person.FirstName + ' ' + dbo.Person.Surname +' ('+ CAST(StakeHolder.StakeHolderID AS varchar(20)) +')' AS FirstName
			FROM dbo.StakeHolder 
				INNER JOIN dbo.Person ON dbo.StakeHolder.PersonId = dbo.Person.PersonId
			ORDER BY [Person].[FirstName], [Person].[SurName]
	END
  
	IF @RoleType = 'NasSupport'  
	BEGIN  
		SELECT   
			99999 AS 'CandidateId',  
			99999 AS 'PersonId',  
			'NAS Support User' AS 'FirstName'  
	END

	IF @RoleType = 'LSCNationalManagementInformation'  
	BEGIN  
		SELECT   
			99999 AS 'CandidateId',  
			99999 AS 'PersonId',
			'National Management Information User' AS 'FirstName'  
	END

	IF @RoleType = 'LSCNationalLearnerSupport'  
	BEGIN  
		SELECT   
			99999 AS 'CandidateId',  
			99999 AS 'PersonId',
			'National Learner Support User' AS 'FirstName'  
	END

	IF @RoleType = 'LSCNationalEmployerSupport'  
	BEGIN  
		SELECT   
			99999 AS 'CandidateId',  
			99999 AS 'PersonId',
			'National Employer Support User' AS 'FirstName'  
	END

	IF @RoleType = 'LSCNationalVMSSupport'  
	BEGIN  
		SELECT   
			99999 AS 'CandidateId',  
			99999 AS 'PersonId',
			'National VMS Support User' AS 'FirstName'  
	END

	IF @RoleType = 'LSCRegionalManagementInformation'  
	BEGIN  
		SELECT   
			99999 AS 'CandidateId',  
			99999 AS 'PersonId',
			'Regional Management Information User' AS 'FirstName'  
	END

	IF @RoleType = 'LSCRegionalLearnerSupport'  
	BEGIN  
		SELECT   
			99999 AS 'CandidateId',  
			99999 AS 'PersonId',
			'Regional Learner Support User' AS 'FirstName'  
	END

	IF @RoleType = 'LSCRegionalEmployerSupport'  
	BEGIN  
		SELECT   
			99999 AS 'CandidateId',  
			99999 AS 'PersonId',
			'Regional Employer Support User' AS 'FirstName'  
	END

	IF @RoleType = 'LSCRegionalVMSSupport'  
	BEGIN  
		SELECT   
			99999 AS 'CandidateId',  
			99999 AS 'PersonId',
			'Regional VMS Support User' AS 'FirstName'  
	END
  
	IF @RoleType = 'LSCNationalSystemAdmin'  
	BEGIN  
		SELECT   
			99999 AS 'CandidateId',  
			99999 AS 'PersonId',
			'National System Admin' AS 'FirstName'  
	END

	IF @RoleType = 'LSCNationalTransferSupport'  
	BEGIN  
		SELECT   
			99999 AS 'CandidateId',  
			99999 AS 'PersonId',
			'National Transfer Support User' AS 'FirstName'  
	END
	
	IF @RoleType = 'LSCRegionalTransferSupport'  
	BEGIN  
		SELECT   
			99999 AS 'CandidateId',  
			99999 AS 'PersonId',
			'Regional Transfer Support User' AS 'FirstName'  
	END
	
	IF @RoleType = 'NasInterfaceAdmin'  
	BEGIN  
		SELECT   
			99999 AS 'CandidateId',  
			99999 AS 'PersonId',
			'NAS Interface Admin' AS 'FirstName'  
	END
	
	SET NOCOUNT OFF;  
END