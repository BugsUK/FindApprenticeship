MERGE INTO [Reference].[Sector] AS Target 
USING (VALUES 
  (1, N'Actuarial'), 
  (2, N'Aerospace'), 
  (3, N'Automotive'), 
  (4, N'Automotive retail'), 
  (5, N'Butchery'), 
  (6, N'Conveyancing and probate'), 
  (7, N'Defence'),
  (8, N'Dental health'),
  (9, N'Digital Industries'),
  (10, N'Electrotechnical'),
  (11, N'Energy & Utilities'),
  (12, N'Financial Services'),
  (13, N'Food and Drink'),
  (14, N'Horticulture'),
  (15, N'Insurance'),
  (16, N'Law'),
  (17, N'Leadership & Management'),
  (18, N'Life and Industrial Sciences'),
  (19, N'Maritime'),
  (20, N'Newspaper and Broadcast Media'),
  (21, N'Nuclear'),
  (22, N'Property services'),
  (23, N'Public Service'),
  (24, N'Rail Design'),
  (25, N'Refrigeration Air Conditioning and Heat Pump (RACHP)'),
  (26, N'Surveying'),
  (27, N'Housing'),
  (28, N'Non-destructive Testing'),
  (29, N'Energy Management')
) 
AS Source (SectorId, FullName) 
ON Target.SectorId = Source.SectorId 
-- update matched rows 
WHEN MATCHED THEN 
UPDATE SET FullName = Source.FullName 
-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN 
INSERT (SectorId, FullName) 
VALUES (SectorId, FullName) 
-- delete rows that are in the target but not the source 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SELECT * FROM Reference.Sector
