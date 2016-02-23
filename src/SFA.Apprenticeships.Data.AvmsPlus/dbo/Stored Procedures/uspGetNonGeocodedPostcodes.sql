CREATE PROCEDURE [dbo].[uspGetNonGeocodedPostcodes]
AS
BEGIN
		SELECT
			VacancyId,
			PostCode,
			LocalAuthorityId			
		FROM
			dbo.Vacancy
		WHERE	
			(
				VacancyStatusId = (SELECT VacancyStatusTypeId FROM dbo.VacancyStatusType WHERE CodeName = 'Lve')
				OR
				VacancyStatusId = (SELECT VacancyStatusTypeId FROM dbo.VacancyStatusType WHERE CodeName = 'Ref')
				OR
				VacancyStatusId = (SELECT VacancyStatusTypeId FROM dbo.VacancyStatusType WHERE CodeName = 'Sub')
			)	
			AND (ISNULL(PostCode,'') <> '')		
			AND (
				GeocodeEasting IS NULL		OR GeocodeEasting = 0
				OR GeocodeNorthing IS NULL	OR GeocodeNorthing = 0
				OR Latitude IS NULL			OR Latitude = 0
				OR Longitude IS NULL		OR Longitude = 0		
			   	OR LocalAuthorityId IS NULL)
END