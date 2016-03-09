
CREATE Procedure [dbo].[uspGetWebInterfaceGenericDetails]
As
Begin

	SET NOCOUNT ON

		select	
			s.ParameterValue as 'ApprenticeshipVacanciesURL',
			s.Description as 'ApprenticeshipVacanciesDescription'
		from 
			SystemParameters s 
		where 
			s.ParameterName = 'AV Description'

	SET NOCOUNT OFF

End