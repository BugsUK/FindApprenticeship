SET IDENTITY_INSERT [dbo].[MessageStatus] ON
GO

MERGE INTO [dbo].[MessageStatus] AS TARGET 
USING (VALUES
	( 1, 'Received' ),
	( 2, 'Processed' )
) AS SOURCE (MessageStatusId,Description)
ON TARGET.MessageStatusId=SOURCE.MessageStatusId
WHEN MATCHED THEN 
UPDATE SET MessageStatusId = SOURCE.MessageStatusId, 
			Description = SOURCE.Description
-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN 
INSERT (MessageStatusId, Description) 
VALUES (SOURCE.MessageStatusId, SOURCE.Description) 
-- delete rows that are in the target but not the source 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [Reference].[EducationLevel] OFF
GO
        