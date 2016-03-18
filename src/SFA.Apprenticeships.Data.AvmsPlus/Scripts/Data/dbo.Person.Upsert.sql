SET IDENTITY_INSERT [dbo].[Person] ON
GO

MERGE INTO [dbo].[Person] AS Target 
--Inserting unspecified person records to easily identify missing data
USING (VALUES 
  (-1, 0, 'Unspecified', 'Candidate', 1),
  (-2, 0, 'Unspecified', 'Employer Contact', 2),
  (-3, 0, 'Unspecified', 'StakeHolder', 3)
) 
AS Source (PersonId, Title, FirstName, Surname, PersonTypeId) 
ON Target.PersonId = Source.PersonId 
-- update matched rows 
WHEN MATCHED THEN 
UPDATE SET Title = Source.Title, FirstName = Source.FirstName, Surname = Source.Surname, PersonTypeId = Source.PersonTypeId
-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN 
INSERT (PersonId, Title, FirstName, Surname, PersonTypeId) 
VALUES (PersonId, Title, FirstName, Surname, PersonTypeId);

SET IDENTITY_INSERT [dbo].[Person] OFF
GO