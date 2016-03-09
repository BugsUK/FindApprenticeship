CREATE PROCEDURE [dbo].[uspVacancyGeocodeUpdate]
	@VacancyId INT, @LocalAuthorityId INT, @GridEastM INT, @GridNorthM INT, @Longitude DECIMAL(13,10), @Latitude DECIMAL(13,10)
AS
BEGIN

	UPDATE
		dbo.Vacancy
	SET
		LocalAuthorityId = @LocalAuthorityId,
		GeocodeEasting = @GridEastM,
		GeocodeNorthing = @GridNorthM,
		Longitude = @Longitude,
		Latitude = @Latitude
	WHERE
		VacancyId = @VacancyId

END