CREATE PROCEDURE [dbo].[ProofConceptMethod2GIS]
    @Latitude decimal(30,15),
    @Longitude decimal(30,15),
    @Eastin decimal(30,15),
    @Northin decimal(30,15),
    @Distance int

--SET @Latitude = 52.477818110072185
--SET @Longitude = -1.9028135272556475
--SET @Eastin = 406600
--SET @Northin = 286700
AS
BEGIN
	SET NOCOUNT ON

BEGIN TRY

SET @Distance = @Distance * 1609.344

        Select 
        (
          (
            SQRT(SQUARE(ABS((VACANCY.GEOCODEEASTING - @Eastin))) + 
	        SQUARE(ABS((VACANCY.GEOCODENORTHING - @Northin))))
          ) /1609.344
         ) 
        as 'Distance',
        Postcode,GeocodeEasting,GeocodeNorthing
        --,dbo.fnx_CalcDistance(@Latitude,@Longitude, Vacancy.Latitude,Vacancy.Longitude) as DistanceLatLon
        ,VacancyId,VacancyReferenceNumber
        from
        Vacancy
        WHERE
--		(
--            SQRT(SQUARE(ABS(((VACANCY.GEOCODEEASTING/1609) - (@Eastin/1609)))) + 
--	        SQUARE(ABS(((VACANCY.GEOCODENORTHING/1609) - (@Northin/1609)))))
--          ) /1609 < 10
        ABS(VACANCY.GEOCODEEASTING - @Eastin) < @Distance
        AND
        ABS(VACANCY.GEOCODENORTHING - @Northin) < @Distance
Order By VacancyId

END TRY

    BEGIN CATCH
		EXEC RethrowError;
	END CATCH
    
    SET NOCOUNT OFF
END