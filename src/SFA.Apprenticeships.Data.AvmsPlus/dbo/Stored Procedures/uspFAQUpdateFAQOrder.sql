CREATE PROCEDURE [dbo].[uspFAQUpdateFAQOrder]
@faqId1 INT, @faqId2 INT
AS
BEGIN
	SET NOCOUNT ON
	
	BEGIN TRY

	BEGIN TRAN

		UPDATE FAQ
		SET [SortOrder] = -1 * [SortOrder]
		WHERE
		FaqId IN (@faqId1, @faqId2)

		UPDATE F1
		SET F1.[SortOrder] = -1 * F2.[SortOrder]
		FROM FAQ F1
		INNER JOIN FAQ F2 ON F2.FaqId =
          CASE
               WHEN F1.FaqId = @faqId1 THEN @faqId2
               WHEN F1.FaqId = @faqId2 THEN @faqId1
               ELSE NULL
          END
		WHERE
		F1.FaqId IN (@faqId1, @faqId2)

		
		
	COMMIT TRAN

	END TRY

    BEGIN CATCH
	ROLLBACK TRAN
		EXEC RethrowError;
	END CATCH
    
    SET NOCOUNT OFF
END