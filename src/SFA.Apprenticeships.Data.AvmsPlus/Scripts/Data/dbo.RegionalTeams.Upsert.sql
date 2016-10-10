SET IDENTITY_INSERT [dbo].[RegionalTeams] ON
GO

MERGE INTO [dbo].[RegionalTeams] AS Target 
USING (VALUES 
	(1, 'North'),
	(2, 'NorthWest'),
	(3, 'YorkshireAndHumberside'),
	(4, 'EastMidlands'),
	(5, 'WestMidlands'),
	(6, 'EastAnglia'),
	(7, 'SouthEast'),
	(8, 'SouthWest')
) 

AS Source (Id, TeamName) 
ON Target.Id = Source.Id 
-- update matched rows 
WHEN MATCHED THEN 
UPDATE SET TeamName = Source.TeamName
-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN 
INSERT (Id, TeamName) 
VALUES (Id, TeamName) 
-- delete rows that are in the target but not the source 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[RegionalTeams] OFF
GO
