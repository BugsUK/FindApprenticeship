CREATE PROCEDURE [dbo].[uspMessageGetSummaryByRecipientAndRecipientType]
    @recipientType int,
    @recipient int
AS
BEGIN
    SET NOCOUNT ON

    BEGIN TRY
        BEGIN

            SELECT
                CASE
                    WHEN s.FullName IS NULL THEN 'Unknown'
                    ELSE s.FullName
                END AS FullName,
                SUM(CONVERT(INT,m.ISRead)) AS 'ReadCount', s.TotalCount
            FROM
                [Message] m
                     JOIN 
                    (
                        SELECT
                            CASE
                                WHEN mc.FullName IS NULL THEN 'Unknown'
                                ELSE mc.FullName
                            END AS FullName
                            ,
                            CASE
                                WHEN mc.MessageCategoryId IS NULL THEN -1
                                ELSE mc.MessageCategoryId
                            END AS MessageCategoryId
                            , COUNT(m.MessageId) AS 'TotalCount'
                        FROM
                            [Message] m LEFT OUTER JOIN MessageCategory mc ON m.MessageCategoryID = mc.MessageCategoryId
                        WHERE
                            m.Recipient = @recipient AND
                            m.RecipientType = @recipientType AND
                            m.IsDeleted = 0 -- 0 = false
                        GROUP BY
                            mc.MessageCategoryId, mc.FullName
                    ) s ON (s.MessageCategoryId = m.MessageCategoryId OR (s.MessageCategoryId = -1 AND m.MessageCategoryId IS NULL))
            WHERE
                m.Recipient = @recipient AND
                m.RecipientType = @recipientType AND
                 m.IsDeleted = 0 -- 0 = false
            GROUP BY
                m.MessageCategoryId, s.FullName, s.TotalCount

        END
    END TRY
    BEGIN CATCH
        EXEC RethrowError;
    END CATCH

    SET NOCOUNT OFF
END