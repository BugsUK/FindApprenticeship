SET IDENTITY_INSERT [dbo].[PersonType] ON
GO

MERGE INTO [dbo].[PersonType] AS TARGET 
USING (VALUES 
  (1, N'CAN', N'CAN', N'Candidate'),
  (2, N'EPC', N'EMP', N'Employer Contact'),
  (3, N'STA', N'STA', N'StakeHolder')
) 
AS SOURCE (PersonTypeId, CodeName, ShortName, FullName) 
ON TARGET.PersonTypeId = SOURCE.PersonTypeId 

WHEN MATCHED THEN 
UPDATE SET CodeName = SOURCE.CodeName, ShortName = SOURCE.ShortName, FullName = SOURCE.FullName

WHEN NOT MATCHED BY TARGET THEN 
INSERT (PersonTypeId, CodeName, ShortName, FullName) 
VALUES (PersonTypeId, CodeName, ShortName, FullName) 

WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SELECT * FROM [dbo].[PersonType]

SET IDENTITY_INSERT [dbo].[PersonType] OFF
GO
