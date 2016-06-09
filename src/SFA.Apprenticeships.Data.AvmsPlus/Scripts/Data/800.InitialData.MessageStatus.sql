MERGE INTO [dbo].[MessageStatus] AS TARGET 
USING (VALUES
	( 1, 'Received' ),
	( 2, 'Processed' )
) AS SOURCE (MessageStatusId,Description)
ON TARGET.MessageStatusId=SOURCE.MessageStatusId
WHEN MATCHED THEN 
UPDATE SET Description = SOURCE.Description
-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN 
INSERT (MessageStatusId, Description) 
VALUES (SOURCE.MessageStatusId, SOURCE.Description) 
-- delete rows that are in the target but not the source 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;
        