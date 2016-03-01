MERGE INTO [Reference].[Standard] AS Target 
USING (VALUES 
  (1, 1, 17, N'Actuarial Technician', 13),
  (2, 2, 3, N'Aerospace Manufacturing Fitter', 12),
  (3, 2, 37, N'Aerospace Engineer', 15),
  (4, 2, 38, N'Aerospace Software Development Engineer', 15),
  (5, 2, 56, N'Aerospace Manufacturing Electrical, Mechanical and Systems Fitter', 12),
  (6, 3, 4, N'Mechatronics Maintenance Technician', 12),
  (7, 3, 9, N'Control/Technical Support Engineer', 15),
  (8, 3, 10, N'Electrical/Electronic Technical Support Engineer', 15),
  (9, 3, 11, N'Manufacturing Engineer', 15),
  (10, 3, 12, N'Product Design and Development Engineer', 15),
  (11, 3, 13, N'Product Design and Development Technician', 12),
  (12, 4, 59, N'Motor Vehicle Service and Maintenance Technician [light vehicle]', 12),
  (13, 5, 55, N'Butcher', 11),
  (14, 6, 39, N'Conveyancing Technician', 13),
  (15, 6, 40, N'Licensed Conveyancer', 15),
  (16, 7, 52, N'Systems Engineering Masters Level', 16),
  (17, 8, 18, N'Dental Technician', 14),
  (18, 8, 19, N'Dental Laboratory Assistant', 12),
  (19, 8, 20, N'Dental Practice Manager', 13),
  (20, 9, 1, N'Network Engineer', 13),
  (21, 9, 2, N'Software Developer', 13),
  (22, 9, 25, N'Digital & Technology Solutions Professional – degree apprenticeship', 15),
  (23, 10, 5, N'Installation Electrician/Maintenance Electrician', 12),
  (24, 11, 6, N'Power Network Craftsperson', 12),
  (25, 11, 26, N'Dual Fuel Smart Meter Installer', 11),
  (26, 11, 27, N'Water Process Technician', 12),
  (27, 11, 53, N'Utilities Engineering Technician', 12),
  (28, 11, 57, N'Gas Network Craftsperson', 12),
  (29, 11, 58, N'Gas Network Team Leader', 11),
  (30, 12, 7, N'Relationship Manager (Banking)', 15),
  (31, 12, 8, N'Financial Services Administrator', 12),
  (32, 12, 28, N'Financial Services Customer Adviser', 11),
  (33, 12, 29, N'Investment Operations Administrator', 11),
  (34, 12, 33, N'Investment Operations Technician', 12),
  (35, 12, 30, N'Investment Operations Specialist', 13),
  (36, 12, 48, N'Paraplanner', 13),
  (37, 12, 31, N'Senior Financial Services Customer Adviser', 12),
  (38, 12, 32, N'Workplace Pensions (Administrator or Consultant)', 12),
  (39, 13, 16, N'Food and Drink Maintenance Engineer', 12),
  (40, 14, 21, N'Golf Greenkeeper', 11),
  (41, 15, 60, N'Insurance Practitioner', 12),
  (42, 16, 41, N'Chartered Legal Executive', 15),
  (43, 16, 42, N'Paralegal', 12),
  (44, 16, 43, N'Solicitor', 16),
  (45, 17, 55, N'Chartered Manager Degree Apprenticeship', 15),
  (46, 18, 14, N'Laboratory Technician', 12),
  (47, 18, 15, N'Science Manufacturing Technician', 12),
  (48, 18, 44, N'Laboratory Scientist', 14),
  (49, 18, 45, N'Science Industry Maintenance Technician', 12),
  (50, 19, 34, N'Able Seafarer (Deck)', 11),
  (51, 20, 22, N'Junior Journalist', 12),
  (52, 21, 35, N'Nuclear Welding Inspection Technician', 13),
  (53, 21, 46, N'Nuclear Health Physics Monitor', 11),
  (54, 21, 47, N'Nuclear Scientist and Nuclear Engineer', 15),
  (55, 22, 23, N'Property Maintenance Operative', 11),
  (56, 23, 36, N'Public Service Operational Delivery Officer', 12),
  (57, 24, 24, N'Railway Engineering Design Technician', 12),
  (58, 25, 49, N'Refrigeration Air Conditioning & Heat Pump Engineering Technician', 12),
  (59, 26, 50, N'Chartered Surveyor', 15),
  (60, 26, 51, N'Surveying Technician', 12),
  (61, 8, 61, N'Dental Nurse', 12),
  (62, 12, 62, N'Mortgage Advisor', 12),
  (63, 12, 63, N'Insurance Professional', 13),
  (64, 27, 64, N'Housing Property Management Assistant', 11),
  (65, 27, 65, N'Housing Property Management', 12),
  (66, 27, 66, N'Senior Housing Property Management', 13),
  (67, 28, 67, N'Non-destructive Testing Engineering Technician', 12),
  (68, 29, 68, N'Junior Energy Manager', 12)
) 
AS Source (StandardId, StandardSectorId, LarsCode, FullName, EducationLevelId) 
ON Target.StandardId = Source.StandardId 
-- update matched rows 
WHEN MATCHED THEN 
UPDATE SET StandardSectorId = Source.StandardSectorId, LarsCode = Source.LarsCode, FullName = Source.FullName, EducationLevelId = Source.EducationLevelId
-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN 
INSERT (StandardId, StandardSectorId, LarsCode, FullName, EducationLevelId) 
VALUES (StandardId, StandardSectorId, LarsCode, FullName, EducationLevelId) 
-- delete rows that are in the target but not the source 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SELECT * FROM Reference.[Standard]
