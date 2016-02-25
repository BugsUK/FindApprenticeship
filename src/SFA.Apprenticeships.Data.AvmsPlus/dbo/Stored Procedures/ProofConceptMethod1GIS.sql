CREATE PROCEDURE [dbo].[ProofConceptMethod1GIS]
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
        Select dbo.fnx_CalcDistance
        (@Latitude,@Longitude, 
        v.Latitude,v.Longitude) as Distance,
        v.Postcode,V.Latitude,V.Longitude
        ,VacancyId, VacancyReferenceNumber 
        from Vacancy v
        wHERE   dbo.fnx_CalcDistance
        (@Latitude,@Longitude, 
        v.Latitude,v.Longitude) < @Distance
        Order By VacancyId
END TRY

    BEGIN CATCH
		EXEC RethrowError;
	END CATCH
    
    SET NOCOUNT OFF
END