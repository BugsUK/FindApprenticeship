SET IDENTITY_INSERT [dbo].[County] ON
GO

MERGE INTO [dbo].[County] AS Target 
USING (VALUES 
  (0, N'NA', N'NA', N'Please Select...'),
  (1, N'BED', N'BED', N'Bedfordshire'),
  (2, N'BER', N'BER', N'Berkshire'),
  (3, N'BUC', N'BUC', N'Buckinghamshire'),
  (4, N'CAM', N'CAM', N'Cambridgeshire'),
  (5, N'CHE', N'CHE', N'Cheshire'),
  (6, N'COR', N'COR', N'Cornwall'),
  (7, N'CUM', N'CUM', N'Cumbria'),
  (8, N'DER', N'DER', N'Derbyshire'),
  (9, N'DEV', N'DEV', N'Devon'),
  (10, N'DOR', N'DOR', N'Dorset'),
  (11, N'DUR', N'DUR', N'Durham'),
  (12, N'EYK', N'EYK', N'East Riding of Yorkshire'),
  (13, N'ESX', N'ESX', N'East Sussex'),
  (14, N'ESS', N'ESS', N'Essex'),
  (15, N'GLO', N'GLO', N'Gloucestershire'),
  (16, N'GMN', N'GMN', N'Greater Manchester'),
  (17, N'HAM', N'HAM', N'Hampshire'),
  (18, N'HFD', N'HFD', N'Herefordshire'),
  (19, N'HTF', N'HTF', N'Hertfordshire'),
  (20, N'IOW', N'IOW', N'Isle of Wight'),
  (21, N'KEN', N'KEN', N'Kent'),
  (22, N'LAN', N'LAN', N'Lancashire'),
  (23, N'LEI', N'LEI', N'Leicestershire'),
  (24, N'LIN', N'LIN', N'Lincolnshire'),
  (25, N'LON', N'LON', N'London'),
  (26, N'MSY', N'MSY', N'Merseyside'),
  (27, N'NOR', N'NOR', N'Norfolk'),
  (28, N'NYK', N'NYK', N'North Yorkshire'),
  (29, N'NTP', N'NTP', N'Northamptonshire'),
  (30, N'NTB', N'NTB', N'Northumberland'),
  (31, N'NTG', N'NTG', N'Nottinghamshire'),
  (32, N'OXF', N'OXF', N'Oxfordshire'),
  (33, N'SHR', N'SHR', N'Shropshire'),
  (34, N'SOM', N'SOM', N'Somerset'),
  (35, N'SYK', N'SYK', N'South Yorkshire'),
  (36, N'STA', N'STA', N'Staffordshire'),
  (37, N'SUF', N'SUF', N'Suffolk'),
  (38, N'SUR', N'SUR', N'Surrey'),
  (39, N'TAW', N'TAW', N'Tyne and Wear'),
  (40, N'WAR', N'WAR', N'Warwickshire'),
  (41, N'WMD', N'WMD', N'West Midlands'),
  (42, N'WSX', N'WSX', N'West Sussex'),
  (43, N'WYK', N'WYK', N'West Yorkshire'),
  (44, N'WIL', N'WIL', N'Wiltshire'),
  (45, N'WOR', N'WOR', N'Worcestershire'),
  (46, N'RUT', N'RUT', N'Rutland'),
  (47, N'IOS', N'IOS', N'Isles of Scilly'),
  (48, N'FLI', N'FLI', N'Flintshire')
) 
AS Source (CountyId, CodeName, ShortName, FullName) 
ON Target.CountyId = Source.CountyId 
-- update matched rows 
WHEN MATCHED THEN 
UPDATE SET CodeName = Source.CodeName, ShortName = Source.ShortName, FullName = Source.FullName
-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN 
INSERT (CountyId, CodeName, ShortName, FullName) 
VALUES (CountyId, CodeName, ShortName, FullName) 
-- delete rows that are in the target but not the source 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[County] OFF
GO
