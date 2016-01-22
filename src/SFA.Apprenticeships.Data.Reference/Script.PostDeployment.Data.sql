/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
MERGE INTO [dbo].[ApprenticeshipSector] AS Target 
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
AS Source (Id, Name) 
ON Target.Id = Source.Id 
-- update matched rows 
WHEN MATCHED THEN 
UPDATE SET Name = Source.Name 
-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN 
INSERT (Id, Name) 
VALUES (Id, Name) 
-- delete rows that are in the target but not the source 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;


MERGE INTO [dbo].[ApprenticeshipStandard] AS Target 
USING (VALUES 
  (1, 1, 17, N'Actuarial Technician', 4),
  (2, 2, 3, N'Aerospace Manufacturing Fitter', 3),
  (3, 2, 37, N'Aerospace Engineer', 6),
  (4, 2, 38, N'Aerospace Software Development Engineer', 6),
  (5, 2, 56, N'Aerospace Manufacturing Electrical, Mechanical and Systems Fitter', 3),
  (6, 3, 4, N'Mechatronics Maintenance Technician', 3),
  (7, 3, 9, N'Control/Technical Support Engineer', 6),
  (8, 3, 10, N'Electrical/Electronic Technical Support Engineer', 6),
  (9, 3, 11, N'Manufacturing Engineer', 6),
  (10, 3, 12, N'Product Design and Development Engineer', 6),
  (11, 3, 13, N'Product Design and Development Technician', 3),
  (12, 4, 59, N'Motor Vehicle Service and Maintenance Technician [light vehicle]', 3),
  (13, 5, 55, N'Butcher', 2),
  (14, 6, 39, N'Conveyancing Technician', 4),
  (15, 6, 40, N'Licensed Conveyancer', 6),
  (16, 7, 52, N'Systems Engineering Masters Level', 7),
  (17, 8, 18, N'Dental Technician', 5),
  (18, 8, 19, N'Dental Laboratory Assistant', 3),
  (19, 8, 20, N'Dental Practice Manager', 4),
  (20, 9, 1, N'Network Engineer', 4),
  (21, 9, 2, N'Software Developer', 4),
  (22, 9, 25, N'Digital & Technology Solutions Professional – degree apprenticeship', 6),
  (23, 10, 5, N'Installation Electrician/Maintenance Electrician', 3),
  (24, 11, 6, N'Power Network Craftsperson', 3),
  (25, 11, 26, N'Dual Fuel Smart Meter Installer', 2),
  (26, 11, 27, N'Water Process Technician', 3),
  (27, 11, 53, N'Utilities Engineering Technician', 3),
  (28, 11, 57, N'Gas Network Craftsperson', 3),
  (29, 11, 58, N'Gas Network Team Leader', 2),
  (30, 12, 7, N'Relationship Manager (Banking)', 6),
  (31, 12, 8, N'Financial Services Administrator', 3),
  (32, 12, 28, N'Financial Services Customer Adviser', 2),
  (33, 12, 29, N'Investment Operations Administrator', 2),
  (34, 12, 33, N'Investment Operations Technician', 3),
  (35, 12, 30, N'Investment Operations Specialist', 4),
  (36, 12, 48, N'Paraplanner', 4),
  (37, 12, 31, N'Senior Financial Services Customer Adviser', 3),
  (38, 12, 32, N'Workplace Pensions (Administrator or Consultant)', 3),
  (39, 13, 16, N'Food and Drink Maintenance Engineer', 3),
  (40, 14, 21, N'Golf Greenkeeper', 2),
  (41, 15, 60, N'Insurance Practitioner', 3),
  (42, 16, 41, N'Chartered Legal Executive', 6),
  (43, 16, 42, N'Paralegal', 3),
  (44, 16, 43, N'Solicitor', 7),
  (45, 17, 55, N'Chartered Manager Degree Apprenticeship', 6),
  (46, 18, 14, N'Laboratory Technician', 3),
  (47, 18, 15, N'Science Manufacturing Technician', 3),
  (48, 18, 44, N'Laboratory Scientist', 5),
  (49, 18, 45, N'Science Industry Maintenance Technician', 3),
  (50, 19, 34, N'Able Seafarer (Deck)', 2),
  (51, 20, 22, N'Junior Journalist', 3),
  (52, 21, 35, N'Nuclear Welding Inspection Technician', 4),
  (53, 21, 46, N'Nuclear Health Physics Monitor', 2),
  (54, 21, 47, N'Nuclear Scientist and Nuclear Engineer', 6),
  (55, 22, 23, N'Property Maintenance Operative', 2),
  (56, 23, 36, N'Public Service Operational Delivery Officer', 3),
  (57, 24, 24, N'Railway Engineering Design Technician', 3),
  (58, 25, 49, N'Refrigeration Air Conditioning & Heat Pump Engineering Technician', 3),
  (59, 26, 50, N'Chartered Surveyor', 6),
  (60, 26, 51, N'Surveying Technician', 3),
  (61, 8, 61, N'Dental Nurse', 3),
  (62, 12, 62, N'Mortgage Advisor', 3),
  (63, 12, 63, N'Insurance Professional', 4),
  (64, 27, 64, N'Housing Property Management Assistant', 2),
  (65, 27, 65, N'Housing Property Management', 3),
  (66, 27, 66, N'Senior Housing Property Management', 4),
  (67, 28, 67, N'Non-destructive Testing Engineering Technician', 3),
  (68, 29, 68, N'Junior Energy Manager', 3)
) 
AS Source (Id, ApprenticeshipSectorId, Code, Name, ApprenticeshipLevel) 
ON Target.Id = Source.Id 
-- update matched rows 
WHEN MATCHED THEN 
UPDATE SET ApprenticeshipSectorId = Source.ApprenticeshipSectorId, Code = Source.Code, Name = Source.Name, ApprenticeshipLevel = Source.ApprenticeshipLevel
-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN 
INSERT (Id, ApprenticeshipSectorId, Code, Name, ApprenticeshipLevel) 
VALUES (Id, ApprenticeshipSectorId, Code, Name, ApprenticeshipLevel) 
-- delete rows that are in the target but not the source 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;