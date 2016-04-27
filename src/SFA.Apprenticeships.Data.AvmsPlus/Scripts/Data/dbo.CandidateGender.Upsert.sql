SET IDENTITY_INSERT [dbo].[CandidateGender] ON
GO

MERGE INTO [dbo].[CandidateGender] AS Target 
USING (VALUES 
	(0, N'', N'', N'Please Select...'),
	(1, N'M', N'M', N'Male'),
	(2, N'F', N'F', N'Female'),
	(3, N'U', N'U', N'Prefer not to say')
) 
AS Source (CandidateGenderId, CodeName, ShortName, FullName) 
ON Target.CandidateGenderId = Source.CandidateGenderId 
-- update matched rows 
WHEN MATCHED THEN 
UPDATE SET CodeName = Source.CodeName, ShortName = Source.ShortName, FullName = Source.FullName
-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN 
INSERT (CandidateGenderId, CodeName, ShortName, FullName) 
VALUES (CandidateGenderId, CodeName, ShortName, FullName) 
-- delete rows that are in the target but not the source 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[CandidateGender] OFF
GO