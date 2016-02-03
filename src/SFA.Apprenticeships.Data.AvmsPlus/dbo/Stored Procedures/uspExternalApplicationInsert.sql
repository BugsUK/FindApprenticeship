CREATE PROCEDURE [dbo].[uspExternalApplicationInsert]
    @vacancyId int,
    @candidateId int,
    @insertedId int OUT,
    @externalTrackingId UNIQUEIDENTIFIER OUT
AS  

BEGIN 
    DECLARE @extAppId INT

    SET @externalTrackingId = NEWID()
 
    SELECT
        @extAppId = ExternalApplicationId
    FROM
        ExternalApplication
    WHERE
        CandidateId = @candidateId
        AND VacancyId = @vacancyId

    -- insert if entry doesn't already exist
    IF @extAppId IS NULL
        BEGIN
            INSERT INTO
                EXTERNALAPPLICATION  
            VALUES
                (@candidateId, @vacancyId, GETDATE(), @externalTrackingId)  

            SET @insertedId = SCOPE_IDENTITY()
        END
    -- otherwise do an update on existing entry
    ELSE
        BEGIN
            UPDATE
                ExternalApplication
            SET
                CandidateId = @candidateId,
                VacancyId = @vacancyId,
                ClickthroughDate = GETDATE(),
                ExternalTrackingId = @externalTrackingId
            WHERE
                ExternalApplicationId = @extAppId

            SET @insertedId = @extAppId
        END

    SET NOCOUNT OFF  
END