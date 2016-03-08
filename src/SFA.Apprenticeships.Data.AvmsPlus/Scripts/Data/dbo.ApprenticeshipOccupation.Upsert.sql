SET IDENTITY_INSERT [dbo].[ApprenticeshipOccupation] ON
GO

MERGE INTO [dbo].[ApprenticeshipOccupation] AS Target 
USING (VALUES 
	(1, 'AHR', '15', 'Business, Administration and Law', 1,	NULL),
	(2, 'ALB', '03', 'Agriculture, Horticulture and Animal Care', 1, NULL),
	(3, 'ACC', '09', 'Arts, Media and Publishing', 1, NULL),
	(7, 'CST', '05', 'Construction, Planning and the Built Environment', 1, NULL),
	(13, 'HBY', '07', 'Retail and Commercial Enterprise', 1, NULL),
	(14, 'HTL', '08', 'Leisure, Travel and Tourism', 1, NULL),
	(15, 'ITC', '06', 'Information and Communication Technology', 1, NULL),
	(17, 'MFP', '04', 'Engineering and Manufacturing Technologies',	1, NULL),
	(20, 'PUB', '01', 'Health, Public Services and Care', 1, NULL),
	(22, '13', '13', 'Education and Training', 1, NULL),
	(24, '02', '02', 'Science and Mathematics',	1, NULL),
	(99, '99', '99', 'Traineeship',	1, NULL),
	(100, '00', '00', 'Standards', 1, NULL)
) 

AS Source (ApprenticeshipOccupationId, CodeName, ShortName, FullName, ApprenticeshipOccupationStatusTypeId, ClosedDate) 
ON Target.ApprenticeshipOccupationId = Source.ApprenticeshipOccupationId 
-- update matched rows 
WHEN MATCHED THEN 
UPDATE SET CodeName = Source.CodeName, ShortName = Source.ShortName, FullName = Source.FullName, ApprenticeshipOccupationStatusTypeId = Source.ApprenticeshipOccupationStatusTypeId, ClosedDate = Source.ClosedDate
-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN 
INSERT (ApprenticeshipOccupationId, CodeName, ShortName, FullName, ApprenticeshipOccupationStatusTypeId, ClosedDate) 
VALUES (ApprenticeshipOccupationId, CodeName, ShortName, FullName, ApprenticeshipOccupationStatusTypeId, ClosedDate) 
-- delete rows that are in the target but not the source 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[ApprenticeshipOccupation] OFF
GO
