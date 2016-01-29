MERGE INTO [Reference].[Region] AS Target 
USING (VALUES 
  (0, N'NUL', N'NUL', N'Unspecified'),
  (1001, N'EM', N'EM', N'East Midlands'),
  (1002, N'EE', N'EE', N'East of England'),
  (1003, N'LON', N'LON', N'London'),
  (1004, N'NE', N'NE', N'North East'),
  (1005, N'NW', N'NW', N'North West'),
  (1006, N'SE', N'SE', N'South East'),
  (1007, N'SW', N'SW', N'South West'),
  (1008, N'WM', N'WM', N'West Midlands'),
  (1009, N'YH', N'YH', N'Yorkshire and The Humber')
) 
AS Source (RegionId, CodeName, ShortName, FullName) 
ON Target.RegionId = Source.RegionId 
-- update matched rows 
WHEN MATCHED THEN 
UPDATE SET CodeName = Source.CodeName, ShortName = Source.ShortName, FullName = Source.FullName
-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN 
INSERT (RegionId, CodeName, ShortName, FullName) 
VALUES (RegionId, CodeName, ShortName, FullName) 
-- delete rows that are in the target but not the source 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;