SET IDENTITY_INSERT [Reference].[StandardSector] ON;

MERGE INTO [Reference].[StandardSector] AS Target 
USING (VALUES 
  (1, N'Actuarial', 1), 
  (2, N'Aerospace', 17), 
  (3, N'Automotive', 17), 
  (4, N'Automotive retail', 13), 
  (5, N'Butchery', 13), 
  (6, N'Conveyancing and probate', 1), 
  (7, N'Defence', 17),
  (8, N'Dental health', 20),
  (9, N'Digital Industries', 15),
  (10, N'Electrotechnical', 17),
  (11, N'Energy & Utilities', 17),
  (12, N'Financial Services', 1),
  (13, N'Food and Drink', 17),
  (14, N'Horticulture', 1),
  (15, N'Insurance', 1),
  (16, N'Law', 1),
  (17, N'Leadership & Management', 1),
  (18, N'Life and Industrial Sciences', 17),
  (19, N'Maritime', 17),
  (20, N'Newspaper and Broadcast Media', 3),
  (21, N'Nuclear', 17),
  (22, N'Property services', 7),
  (23, N'Public Service', 20),
  (24, N'Rail Design', 17),
  (25, N'Refrigeration Air Conditioning and Heat Pump (RACHP)', 17),
  (26, N'Surveying', 7),
  (27, N'Housing', 20),
  (28, N'Non-destructive Testing', 17),
  (29, N'Energy Management', 13),
  (30, N'Visual Effects', 15),
  (31, N'Aviation', 17),
  (32, N'Bespoke Tailoring', 13),
  (33, N'Boatbuilding', 17),
  (34, N'Hospitality', 13),
  (35, N'Engineering, Design and Draughting', 17),
  (36, N'Healthcare', 20),
  (37, N'Management Consultancy', 1),
  (38, N'Land-Based Engineering', 17),
  (39, N'Live Events', 17),
  (40, N'Advanced Manufacturing', 17),
  (41, N'Welding', 17),
  (42, N'TV Production & Broadcasting', 13),
  (43, N'Rail Engineering', 17),
  (44, N'Retail', 13),
  (45, N'Transport', 17),
  (46,N'Accounting',1),
  (47,N'Adult care',20),
  (48,N'Airworthiness',17),
  (49,N'Construction',7),
  (50,N'Customer service',13),
  (51,N'Electronic Systems',17),
  (52,N'Fire Emergency and Security Systems',17),
  (53,N'HM Armed Forces',20),
  (54,N'Logistics and Supply Chain',17),
  (55,N'Papermaking',7),
  (56,N'Project Management',1),
  (57,N'Public Sector',20),
  (58,N'Travel',13),
  (59,N'Bus, Coach and HGV',17),
  (60,N'Furniture',17),
  (61,N'Groundsmanship',1),
  (62,N'Media',1),
  (63,N'Ambulance services', 20),
  (64,N'Craft', 17),
  (65,N'Event Management', 13),
  (66,N'Hair and Beauty', 13)
) 
AS Source (StandardSectorId, FullName, ApprenticeshipOccupationId) 
ON Target.StandardSectorId = Source.StandardSectorId 
-- update matched rows 
WHEN MATCHED THEN 
UPDATE SET FullName = Source.FullName, ApprenticeshipOccupationId = Source.ApprenticeshipOccupationId
-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN 
INSERT (StandardSectorId, FullName, ApprenticeshipOccupationId) 
VALUES (StandardSectorId, FullName, ApprenticeshipOccupationId) 
-- delete rows that are in the target but not the source 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [Reference].[StandardSector] OFF;