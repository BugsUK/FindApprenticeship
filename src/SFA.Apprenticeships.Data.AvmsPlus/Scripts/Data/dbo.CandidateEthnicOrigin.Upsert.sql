SET IDENTITY_INSERT [dbo].[CandidateEthnicOrigin] ON
GO

MERGE INTO [dbo].[CandidateEthnicOrigin] AS Target 
USING (VALUES 
	(0, N'', N'Please Select', N'Select a group first'),
	(1, N'', N'Asian or Asian British', N'Please Select'),
	(2, N'', N'Asian or Asian British', N'Bangladeshi'),
	(3, N'', N'Asian or Asian British', N'Indian'),
	(4, N'', N'Asian or Asian British', N'Pakistani'),
	(5, N'', N'Asian or Asian British', N'Other Asian Background'),
	(6, N'', N'Black or black British', N'Please Select'),
	(7, N'', N'Black or black British', N'African'),
	(8, N'', N'Black or black British', N'Caribbean'),
	(9, N'', N'Black or black British', N'Other black background'),
	(10, N'', N'Mixed', N'Please Select'),
	(11, N'', N'Mixed', N'White Asian'),
	(12, N'', N'Mixed', N'White and black African'),
	(13, N'', N'Mixed', N'White and black Caribbean'),
	(14, N'', N'Mixed', N'Other mixed background'),
	(15, N'', N'White', N'Please Select'),
	(16, N'', N'White', N'British'),
	(17, N'', N'White', N'Irish'),
	(18, N'', N'White', N'Other white background'),
	(19, N'', N'Chinese', N'Chinese'),
	(20, N'', N'Other (please specify)', N'Not applicable'),
	(21, N'', N'Don''t know or don''t want to say', N'Not applicable')
) 
AS Source (CandidateEthnicOriginId, CodeName, ShortName, FullName) 
ON Target.CandidateEthnicOriginId = Source.CandidateEthnicOriginId 
-- update matched rows 
WHEN MATCHED THEN 
UPDATE SET CodeName = Source.CodeName, ShortName = Source.ShortName, FullName = Source.FullName
-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN 
INSERT (CandidateEthnicOriginId, CodeName, ShortName, FullName) 
VALUES (CandidateEthnicOriginId, CodeName, ShortName, FullName) 
-- delete rows that are in the target but not the source 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[CandidateEthnicOrigin] OFF
GO