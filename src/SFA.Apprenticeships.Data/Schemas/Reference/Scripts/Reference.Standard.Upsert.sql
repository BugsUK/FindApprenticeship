MERGE Reference.[Standard] AS dest
USING (SELECT 1, 'Actuarial Technician', 1, '4') AS src (StandardId, FullName, SectorId, LevelCode)
ON (dest.StandardId = src.StandardId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName, SectorId = src.SectorId, LevelCode = src.LevelCode
WHEN NOT MATCHED THEN
	INSERT (StandardId, FullName, SectorId, LevelCode)
	VALUES (src.StandardId, src.FullName, src.SectorId, src.LevelCode)
;

MERGE Reference.[Standard] AS dest
USING (SELECT 2, 'Aerospace Manufacturing Fitter', 2, '3') AS src (StandardId, FullName, SectorId, LevelCode)
ON (dest.StandardId = src.StandardId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName, SectorId = src.SectorId, LevelCode = src.LevelCode
WHEN NOT MATCHED THEN
	INSERT (StandardId, FullName, SectorId, LevelCode)
	VALUES (src.StandardId, src.FullName, src.SectorId, src.LevelCode)
;

MERGE Reference.[Standard] AS dest
USING (SELECT 3, 'Aerospace Engineer', 2, '6') AS src (StandardId, FullName, SectorId, LevelCode)
ON (dest.StandardId = src.StandardId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName, SectorId = src.SectorId, LevelCode = src.LevelCode
WHEN NOT MATCHED THEN
	INSERT (StandardId, FullName, SectorId, LevelCode)
	VALUES (src.StandardId, src.FullName, src.SectorId, src.LevelCode)
;

MERGE Reference.[Standard] AS dest
USING (SELECT 4, 'Aerospace Software Development Engineer', 2, '6') AS src (StandardId, FullName, SectorId, LevelCode)
ON (dest.StandardId = src.StandardId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName, SectorId = src.SectorId, LevelCode = src.LevelCode
WHEN NOT MATCHED THEN
	INSERT (StandardId, FullName, SectorId, LevelCode)
	VALUES (src.StandardId, src.FullName, src.SectorId, src.LevelCode)
;

MERGE Reference.[Standard] AS dest
USING (SELECT 5, 'Aerospace Manufacturing Electrical, Mechanical and Systems Fitter', 2, '3') AS src (StandardId, FullName, SectorId, LevelCode)
ON (dest.StandardId = src.StandardId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName, SectorId = src.SectorId, LevelCode = src.LevelCode
WHEN NOT MATCHED THEN
	INSERT (StandardId, FullName, SectorId, LevelCode)
	VALUES (src.StandardId, src.FullName, src.SectorId, src.LevelCode)
;

MERGE Reference.[Standard] AS dest
USING (SELECT 6, 'Mechatronics Maintenance Technician', 3, '3') AS src (StandardId, FullName, SectorId, LevelCode)
ON (dest.StandardId = src.StandardId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName, SectorId = src.SectorId, LevelCode = src.LevelCode
WHEN NOT MATCHED THEN
	INSERT (StandardId, FullName, SectorId, LevelCode)
	VALUES (src.StandardId, src.FullName, src.SectorId, src.LevelCode)
;

MERGE Reference.[Standard] AS dest
USING (SELECT 7, 'Control/Technical Support Engineer', 3, '6') AS src (StandardId, FullName, SectorId, LevelCode)
ON (dest.StandardId = src.StandardId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName, SectorId = src.SectorId, LevelCode = src.LevelCode
WHEN NOT MATCHED THEN
	INSERT (StandardId, FullName, SectorId, LevelCode)
	VALUES (src.StandardId, src.FullName, src.SectorId, src.LevelCode)
;

MERGE Reference.[Standard] AS dest
USING (SELECT 8, 'Electrical/Electronic Technical Support Engineer', 3, '6') AS src (StandardId, FullName, SectorId, LevelCode)
ON (dest.StandardId = src.StandardId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName, SectorId = src.SectorId, LevelCode = src.LevelCode
WHEN NOT MATCHED THEN
	INSERT (StandardId, FullName, SectorId, LevelCode)
	VALUES (src.StandardId, src.FullName, src.SectorId, src.LevelCode)
;

MERGE Reference.[Standard] AS dest
USING (SELECT 9, 'Manufacturing Engineer', 3, '6') AS src (StandardId, FullName, SectorId, LevelCode)
ON (dest.StandardId = src.StandardId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName, SectorId = src.SectorId, LevelCode = src.LevelCode
WHEN NOT MATCHED THEN
	INSERT (StandardId, FullName, SectorId, LevelCode)
	VALUES (src.StandardId, src.FullName, src.SectorId, src.LevelCode)
;

MERGE Reference.[Standard] AS dest
USING (SELECT 10, 'Product Design and Development Engineer', 3, '6') AS src (StandardId, FullName, SectorId, LevelCode)
ON (dest.StandardId = src.StandardId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName, SectorId = src.SectorId, LevelCode = src.LevelCode
WHEN NOT MATCHED THEN
	INSERT (StandardId, FullName, SectorId, LevelCode)
	VALUES (src.StandardId, src.FullName, src.SectorId, src.LevelCode)
;

MERGE Reference.[Standard] AS dest
USING (SELECT 11, 'Product Design and Development Technician', 3, '3') AS src (StandardId, FullName, SectorId, LevelCode)
ON (dest.StandardId = src.StandardId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName, SectorId = src.SectorId, LevelCode = src.LevelCode
WHEN NOT MATCHED THEN
	INSERT (StandardId, FullName, SectorId, LevelCode)
	VALUES (src.StandardId, src.FullName, src.SectorId, src.LevelCode)
;

MERGE Reference.[Standard] AS dest
USING (SELECT 12, 'Motor Vehicle Service and Maintenance Technician [light vehicle]', 4, '3') AS src (StandardId, FullName, SectorId, LevelCode)
ON (dest.StandardId = src.StandardId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName, SectorId = src.SectorId, LevelCode = src.LevelCode
WHEN NOT MATCHED THEN
	INSERT (StandardId, FullName, SectorId, LevelCode)
	VALUES (src.StandardId, src.FullName, src.SectorId, src.LevelCode)
;

MERGE Reference.[Standard] AS dest
USING (SELECT 13, 'Butcher', 5, '2') AS src (StandardId, FullName, SectorId, LevelCode)
ON (dest.StandardId = src.StandardId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName, SectorId = src.SectorId, LevelCode = src.LevelCode
WHEN NOT MATCHED THEN
	INSERT (StandardId, FullName, SectorId, LevelCode)
	VALUES (src.StandardId, src.FullName, src.SectorId, src.LevelCode)
;

MERGE Reference.[Standard] AS dest
USING (SELECT 14, 'Conveyancing Technician', 6, '4') AS src (StandardId, FullName, SectorId, LevelCode)
ON (dest.StandardId = src.StandardId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName, SectorId = src.SectorId, LevelCode = src.LevelCode
WHEN NOT MATCHED THEN
	INSERT (StandardId, FullName, SectorId, LevelCode)
	VALUES (src.StandardId, src.FullName, src.SectorId, src.LevelCode)
;

MERGE Reference.[Standard] AS dest
USING (SELECT 15, 'Licensed Conveyancer', 6, '6') AS src (StandardId, FullName, SectorId, LevelCode)
ON (dest.StandardId = src.StandardId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName, SectorId = src.SectorId, LevelCode = src.LevelCode
WHEN NOT MATCHED THEN
	INSERT (StandardId, FullName, SectorId, LevelCode)
	VALUES (src.StandardId, src.FullName, src.SectorId, src.LevelCode)
;

MERGE Reference.[Standard] AS dest
USING (SELECT 16, 'Systems Engineering Masters Level', 7, '7') AS src (StandardId, FullName, SectorId, LevelCode)
ON (dest.StandardId = src.StandardId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName, SectorId = src.SectorId, LevelCode = src.LevelCode
WHEN NOT MATCHED THEN
	INSERT (StandardId, FullName, SectorId, LevelCode)
	VALUES (src.StandardId, src.FullName, src.SectorId, src.LevelCode)
;

MERGE Reference.[Standard] AS dest
USING (SELECT 17, 'Dental Technician', 8, '5') AS src (StandardId, FullName, SectorId, LevelCode)
ON (dest.StandardId = src.StandardId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName, SectorId = src.SectorId, LevelCode = src.LevelCode
WHEN NOT MATCHED THEN
	INSERT (StandardId, FullName, SectorId, LevelCode)
	VALUES (src.StandardId, src.FullName, src.SectorId, src.LevelCode)
;

MERGE Reference.[Standard] AS dest
USING (SELECT 18, 'Dental Laboratory Assistant', 8, '3') AS src (StandardId, FullName, SectorId, LevelCode)
ON (dest.StandardId = src.StandardId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName, SectorId = src.SectorId, LevelCode = src.LevelCode
WHEN NOT MATCHED THEN
	INSERT (StandardId, FullName, SectorId, LevelCode)
	VALUES (src.StandardId, src.FullName, src.SectorId, src.LevelCode)
;

MERGE Reference.[Standard] AS dest
USING (SELECT 19, 'Dental Practice Manager', 8, '4') AS src (StandardId, FullName, SectorId, LevelCode)
ON (dest.StandardId = src.StandardId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName, SectorId = src.SectorId, LevelCode = src.LevelCode
WHEN NOT MATCHED THEN
	INSERT (StandardId, FullName, SectorId, LevelCode)
	VALUES (src.StandardId, src.FullName, src.SectorId, src.LevelCode)
;

MERGE Reference.[Standard] AS dest
USING (SELECT 20, 'Network Engineer', 9, '4') AS src (StandardId, FullName, SectorId, LevelCode)
ON (dest.StandardId = src.StandardId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName, SectorId = src.SectorId, LevelCode = src.LevelCode
WHEN NOT MATCHED THEN
	INSERT (StandardId, FullName, SectorId, LevelCode)
	VALUES (src.StandardId, src.FullName, src.SectorId, src.LevelCode)
;

MERGE Reference.[Standard] AS dest
USING (SELECT 21, 'Software Developer', 9, '4') AS src (StandardId, FullName, SectorId, LevelCode)
ON (dest.StandardId = src.StandardId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName, SectorId = src.SectorId, LevelCode = src.LevelCode
WHEN NOT MATCHED THEN
	INSERT (StandardId, FullName, SectorId, LevelCode)
	VALUES (src.StandardId, src.FullName, src.SectorId, src.LevelCode)
;

MERGE Reference.[Standard] AS dest
USING (SELECT 22, 'Digital & Technology Solutions Professional – degree apprenticeship', 9, '6') AS src (StandardId, FullName, SectorId, LevelCode)
ON (dest.StandardId = src.StandardId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName, SectorId = src.SectorId, LevelCode = src.LevelCode
WHEN NOT MATCHED THEN
	INSERT (StandardId, FullName, SectorId, LevelCode)
	VALUES (src.StandardId, src.FullName, src.SectorId, src.LevelCode)
;

MERGE Reference.[Standard] AS dest
USING (SELECT 23, 'Installation Electrician/Maintenance Electrician', 10, '3') AS src (StandardId, FullName, SectorId, LevelCode)
ON (dest.StandardId = src.StandardId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName, SectorId = src.SectorId, LevelCode = src.LevelCode
WHEN NOT MATCHED THEN
	INSERT (StandardId, FullName, SectorId, LevelCode)
	VALUES (src.StandardId, src.FullName, src.SectorId, src.LevelCode)
;

MERGE Reference.[Standard] AS dest
USING (SELECT 24, 'Power Network Craftsperson', 11, '3') AS src (StandardId, FullName, SectorId, LevelCode)
ON (dest.StandardId = src.StandardId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName, SectorId = src.SectorId, LevelCode = src.LevelCode
WHEN NOT MATCHED THEN
	INSERT (StandardId, FullName, SectorId, LevelCode)
	VALUES (src.StandardId, src.FullName, src.SectorId, src.LevelCode)
;

MERGE Reference.[Standard] AS dest
USING (SELECT 25, 'Dual Fuel Smart Meter Installer', 11, '2') AS src (StandardId, FullName, SectorId, LevelCode)
ON (dest.StandardId = src.StandardId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName, SectorId = src.SectorId, LevelCode = src.LevelCode
WHEN NOT MATCHED THEN
	INSERT (StandardId, FullName, SectorId, LevelCode)
	VALUES (src.StandardId, src.FullName, src.SectorId, src.LevelCode)
;

MERGE Reference.[Standard] AS dest
USING (SELECT 26, 'Water Process Technician', 11, '3') AS src (StandardId, FullName, SectorId, LevelCode)
ON (dest.StandardId = src.StandardId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName, SectorId = src.SectorId, LevelCode = src.LevelCode
WHEN NOT MATCHED THEN
	INSERT (StandardId, FullName, SectorId, LevelCode)
	VALUES (src.StandardId, src.FullName, src.SectorId, src.LevelCode)
;

MERGE Reference.[Standard] AS dest
USING (SELECT 27, 'Utilities Engineering Technician', 11, '3') AS src (StandardId, FullName, SectorId, LevelCode)
ON (dest.StandardId = src.StandardId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName, SectorId = src.SectorId, LevelCode = src.LevelCode
WHEN NOT MATCHED THEN
	INSERT (StandardId, FullName, SectorId, LevelCode)
	VALUES (src.StandardId, src.FullName, src.SectorId, src.LevelCode)
;

MERGE Reference.[Standard] AS dest
USING (SELECT 28, 'Gas Network Craftsperson', 11, '3') AS src (StandardId, FullName, SectorId, LevelCode)
ON (dest.StandardId = src.StandardId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName, SectorId = src.SectorId, LevelCode = src.LevelCode
WHEN NOT MATCHED THEN
	INSERT (StandardId, FullName, SectorId, LevelCode)
	VALUES (src.StandardId, src.FullName, src.SectorId, src.LevelCode)
;

MERGE Reference.[Standard] AS dest
USING (SELECT 29, 'Gas Network Team Leader', 11, '2') AS src (StandardId, FullName, SectorId, LevelCode)
ON (dest.StandardId = src.StandardId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName, SectorId = src.SectorId, LevelCode = src.LevelCode
WHEN NOT MATCHED THEN
	INSERT (StandardId, FullName, SectorId, LevelCode)
	VALUES (src.StandardId, src.FullName, src.SectorId, src.LevelCode)
;

MERGE Reference.[Standard] AS dest
USING (SELECT 30, 'Relationship Manager (Banking)', 12, '6') AS src (StandardId, FullName, SectorId, LevelCode)
ON (dest.StandardId = src.StandardId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName, SectorId = src.SectorId, LevelCode = src.LevelCode
WHEN NOT MATCHED THEN
	INSERT (StandardId, FullName, SectorId, LevelCode)
	VALUES (src.StandardId, src.FullName, src.SectorId, src.LevelCode)
;

MERGE Reference.[Standard] AS dest
USING (SELECT 31, 'Financial Services Administrator', 12, '3') AS src (StandardId, FullName, SectorId, LevelCode)
ON (dest.StandardId = src.StandardId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName, SectorId = src.SectorId, LevelCode = src.LevelCode
WHEN NOT MATCHED THEN
	INSERT (StandardId, FullName, SectorId, LevelCode)
	VALUES (src.StandardId, src.FullName, src.SectorId, src.LevelCode)
;

MERGE Reference.[Standard] AS dest
USING (SELECT 32, 'Financial Services Customer Adviser', 12, '2') AS src (StandardId, FullName, SectorId, LevelCode)
ON (dest.StandardId = src.StandardId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName, SectorId = src.SectorId, LevelCode = src.LevelCode
WHEN NOT MATCHED THEN
	INSERT (StandardId, FullName, SectorId, LevelCode)
	VALUES (src.StandardId, src.FullName, src.SectorId, src.LevelCode)
;

MERGE Reference.[Standard] AS dest
USING (SELECT 33, 'Investment Operations Administrator', 12, '2') AS src (StandardId, FullName, SectorId, LevelCode)
ON (dest.StandardId = src.StandardId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName, SectorId = src.SectorId, LevelCode = src.LevelCode
WHEN NOT MATCHED THEN
	INSERT (StandardId, FullName, SectorId, LevelCode)
	VALUES (src.StandardId, src.FullName, src.SectorId, src.LevelCode)
;

MERGE Reference.[Standard] AS dest
USING (SELECT 34, 'Investment Operations Technician', 12, '3') AS src (StandardId, FullName, SectorId, LevelCode)
ON (dest.StandardId = src.StandardId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName, SectorId = src.SectorId, LevelCode = src.LevelCode
WHEN NOT MATCHED THEN
	INSERT (StandardId, FullName, SectorId, LevelCode)
	VALUES (src.StandardId, src.FullName, src.SectorId, src.LevelCode)
;

MERGE Reference.[Standard] AS dest
USING (SELECT 35, 'Investment Operations Specialist', 12, '4') AS src (StandardId, FullName, SectorId, LevelCode)
ON (dest.StandardId = src.StandardId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName, SectorId = src.SectorId, LevelCode = src.LevelCode
WHEN NOT MATCHED THEN
	INSERT (StandardId, FullName, SectorId, LevelCode)
	VALUES (src.StandardId, src.FullName, src.SectorId, src.LevelCode)
;

MERGE Reference.[Standard] AS dest
USING (SELECT 36, 'Paraplanner', 12, '4') AS src (StandardId, FullName, SectorId, LevelCode)
ON (dest.StandardId = src.StandardId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName, SectorId = src.SectorId, LevelCode = src.LevelCode
WHEN NOT MATCHED THEN
	INSERT (StandardId, FullName, SectorId, LevelCode)
	VALUES (src.StandardId, src.FullName, src.SectorId, src.LevelCode)
;

MERGE Reference.[Standard] AS dest
USING (SELECT 37, 'Senior Financial Services Customer Adviser', 12, '3') AS src (StandardId, FullName, SectorId, LevelCode)
ON (dest.StandardId = src.StandardId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName, SectorId = src.SectorId, LevelCode = src.LevelCode
WHEN NOT MATCHED THEN
	INSERT (StandardId, FullName, SectorId, LevelCode)
	VALUES (src.StandardId, src.FullName, src.SectorId, src.LevelCode)
;

MERGE Reference.[Standard] AS dest
USING (SELECT 38, 'Workplace Pensions (Administrator or Consultant)', 12, '3') AS src (StandardId, FullName, SectorId, LevelCode)
ON (dest.StandardId = src.StandardId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName, SectorId = src.SectorId, LevelCode = src.LevelCode
WHEN NOT MATCHED THEN
	INSERT (StandardId, FullName, SectorId, LevelCode)
	VALUES (src.StandardId, src.FullName, src.SectorId, src.LevelCode)
;

MERGE Reference.[Standard] AS dest
USING (SELECT 39, 'Food and Drink Maintenance Engineer', 13, '3') AS src (StandardId, FullName, SectorId, LevelCode)
ON (dest.StandardId = src.StandardId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName, SectorId = src.SectorId, LevelCode = src.LevelCode
WHEN NOT MATCHED THEN
	INSERT (StandardId, FullName, SectorId, LevelCode)
	VALUES (src.StandardId, src.FullName, src.SectorId, src.LevelCode)
;

MERGE Reference.[Standard] AS dest
USING (SELECT 40, 'Golf Greenkeeper', 14, '2') AS src (StandardId, FullName, SectorId, LevelCode)
ON (dest.StandardId = src.StandardId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName, SectorId = src.SectorId, LevelCode = src.LevelCode
WHEN NOT MATCHED THEN
	INSERT (StandardId, FullName, SectorId, LevelCode)
	VALUES (src.StandardId, src.FullName, src.SectorId, src.LevelCode)
;

MERGE Reference.[Standard] AS dest
USING (SELECT 41, 'Insurance Practitioner', 15, '3') AS src (StandardId, FullName, SectorId, LevelCode)
ON (dest.StandardId = src.StandardId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName, SectorId = src.SectorId, LevelCode = src.LevelCode
WHEN NOT MATCHED THEN
	INSERT (StandardId, FullName, SectorId, LevelCode)
	VALUES (src.StandardId, src.FullName, src.SectorId, src.LevelCode)
;

MERGE Reference.[Standard] AS dest
USING (SELECT 42, 'Chartered Legal Executive', 16, '6') AS src (StandardId, FullName, SectorId, LevelCode)
ON (dest.StandardId = src.StandardId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName, SectorId = src.SectorId, LevelCode = src.LevelCode
WHEN NOT MATCHED THEN
	INSERT (StandardId, FullName, SectorId, LevelCode)
	VALUES (src.StandardId, src.FullName, src.SectorId, src.LevelCode)
;

MERGE Reference.[Standard] AS dest
USING (SELECT 43, 'Paralegal', 16, '3') AS src (StandardId, FullName, SectorId, LevelCode)
ON (dest.StandardId = src.StandardId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName, SectorId = src.SectorId, LevelCode = src.LevelCode
WHEN NOT MATCHED THEN
	INSERT (StandardId, FullName, SectorId, LevelCode)
	VALUES (src.StandardId, src.FullName, src.SectorId, src.LevelCode)
;

MERGE Reference.[Standard] AS dest
USING (SELECT 44, 'Solicitor', 17, '7') AS src (StandardId, FullName, SectorId, LevelCode)
ON (dest.StandardId = src.StandardId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName, SectorId = src.SectorId, LevelCode = src.LevelCode
WHEN NOT MATCHED THEN
	INSERT (StandardId, FullName, SectorId, LevelCode)
	VALUES (src.StandardId, src.FullName, src.SectorId, src.LevelCode)
;

MERGE Reference.[Standard] AS dest
USING (SELECT 45, 'Chartered Manager Degree Apprenticeship', 17, '6') AS src (StandardId, FullName, SectorId, LevelCode)
ON (dest.StandardId = src.StandardId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName, SectorId = src.SectorId, LevelCode = src.LevelCode
WHEN NOT MATCHED THEN
	INSERT (StandardId, FullName, SectorId, LevelCode)
	VALUES (src.StandardId, src.FullName, src.SectorId, src.LevelCode)
;

MERGE Reference.[Standard] AS dest
USING (SELECT 46, 'Laboratory Technician', 18, '3') AS src (StandardId, FullName, SectorId, LevelCode)
ON (dest.StandardId = src.StandardId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName, SectorId = src.SectorId, LevelCode = src.LevelCode
WHEN NOT MATCHED THEN
	INSERT (StandardId, FullName, SectorId, LevelCode)
	VALUES (src.StandardId, src.FullName, src.SectorId, src.LevelCode)
;

MERGE Reference.[Standard] AS dest
USING (SELECT 47, 'Science Manufacturing Technician', 18, '3') AS src (StandardId, FullName, SectorId, LevelCode)
ON (dest.StandardId = src.StandardId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName, SectorId = src.SectorId, LevelCode = src.LevelCode
WHEN NOT MATCHED THEN
	INSERT (StandardId, FullName, SectorId, LevelCode)
	VALUES (src.StandardId, src.FullName, src.SectorId, src.LevelCode)
;

MERGE Reference.[Standard] AS dest
USING (SELECT 48, 'Laboratory Scientist', 18, '5') AS src (StandardId, FullName, SectorId, LevelCode)
ON (dest.StandardId = src.StandardId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName, SectorId = src.SectorId, LevelCode = src.LevelCode
WHEN NOT MATCHED THEN
	INSERT (StandardId, FullName, SectorId, LevelCode)
	VALUES (src.StandardId, src.FullName, src.SectorId, src.LevelCode)
;

MERGE Reference.[Standard] AS dest
USING (SELECT 49, 'Science Industry Maintenance Technician', 18, '3') AS src (StandardId, FullName, SectorId, LevelCode)
ON (dest.StandardId = src.StandardId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName, SectorId = src.SectorId, LevelCode = src.LevelCode
WHEN NOT MATCHED THEN
	INSERT (StandardId, FullName, SectorId, LevelCode)
	VALUES (src.StandardId, src.FullName, src.SectorId, src.LevelCode)
;

MERGE Reference.[Standard] AS dest
USING (SELECT 50, 'Able Seafarer (Deck)', 19, '2') AS src (StandardId, FullName, SectorId, LevelCode)
ON (dest.StandardId = src.StandardId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName, SectorId = src.SectorId, LevelCode = src.LevelCode
WHEN NOT MATCHED THEN
	INSERT (StandardId, FullName, SectorId, LevelCode)
	VALUES (src.StandardId, src.FullName, src.SectorId, src.LevelCode)
;

MERGE Reference.[Standard] AS dest
USING (SELECT 51, 'Junior Journalist', 20, '3') AS src (StandardId, FullName, SectorId, LevelCode)
ON (dest.StandardId = src.StandardId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName, SectorId = src.SectorId, LevelCode = src.LevelCode
WHEN NOT MATCHED THEN
	INSERT (StandardId, FullName, SectorId, LevelCode)
	VALUES (src.StandardId, src.FullName, src.SectorId, src.LevelCode)
;

MERGE Reference.[Standard] AS dest
USING (SELECT 52, 'Nuclear Welding Inspection Technician', 21, '4') AS src (StandardId, FullName, SectorId, LevelCode)
ON (dest.StandardId = src.StandardId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName, SectorId = src.SectorId, LevelCode = src.LevelCode
WHEN NOT MATCHED THEN
	INSERT (StandardId, FullName, SectorId, LevelCode)
	VALUES (src.StandardId, src.FullName, src.SectorId, src.LevelCode)
;

MERGE Reference.[Standard] AS dest
USING (SELECT 53, 'Nuclear Health Physics Monitor', 21, '2') AS src (StandardId, FullName, SectorId, LevelCode)
ON (dest.StandardId = src.StandardId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName, SectorId = src.SectorId, LevelCode = src.LevelCode
WHEN NOT MATCHED THEN
	INSERT (StandardId, FullName, SectorId, LevelCode)
	VALUES (src.StandardId, src.FullName, src.SectorId, src.LevelCode)
;

MERGE Reference.[Standard] AS dest
USING (SELECT 54, 'Nuclear Scientist and Nuclear Engineer', 21, '6') AS src (StandardId, FullName, SectorId, LevelCode)
ON (dest.StandardId = src.StandardId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName, SectorId = src.SectorId, LevelCode = src.LevelCode
WHEN NOT MATCHED THEN
	INSERT (StandardId, FullName, SectorId, LevelCode)
	VALUES (src.StandardId, src.FullName, src.SectorId, src.LevelCode)
;

MERGE Reference.[Standard] AS dest
USING (SELECT 55, 'Property Maintenance Operative', 22, '2') AS src (StandardId, FullName, SectorId, LevelCode)
ON (dest.StandardId = src.StandardId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName, SectorId = src.SectorId, LevelCode = src.LevelCode
WHEN NOT MATCHED THEN
	INSERT (StandardId, FullName, SectorId, LevelCode)
	VALUES (src.StandardId, src.FullName, src.SectorId, src.LevelCode)
;

MERGE Reference.[Standard] AS dest
USING (SELECT 56, 'Public Service Operational Delivery Officer', 23, '3') AS src (StandardId, FullName, SectorId, LevelCode)
ON (dest.StandardId = src.StandardId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName, SectorId = src.SectorId, LevelCode = src.LevelCode
WHEN NOT MATCHED THEN
	INSERT (StandardId, FullName, SectorId, LevelCode)
	VALUES (src.StandardId, src.FullName, src.SectorId, src.LevelCode)
;

MERGE Reference.[Standard] AS dest
USING (SELECT 57, 'Railway Engineering Design Technician', 24, '3') AS src (StandardId, FullName, SectorId, LevelCode)
ON (dest.StandardId = src.StandardId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName, SectorId = src.SectorId, LevelCode = src.LevelCode
WHEN NOT MATCHED THEN
	INSERT (StandardId, FullName, SectorId, LevelCode)
	VALUES (src.StandardId, src.FullName, src.SectorId, src.LevelCode)
;

MERGE Reference.[Standard] AS dest
USING (SELECT 58, 'Refrigeration Air Conditioning & Heat Pump Engineering Technician', 25, '3') AS src (StandardId, FullName, SectorId, LevelCode)
ON (dest.StandardId = src.StandardId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName, SectorId = src.SectorId, LevelCode = src.LevelCode
WHEN NOT MATCHED THEN
	INSERT (StandardId, FullName, SectorId, LevelCode)
	VALUES (src.StandardId, src.FullName, src.SectorId, src.LevelCode)
;

MERGE Reference.[Standard] AS dest
USING (SELECT 59, 'Chartered Surveyor', 26, '6') AS src (StandardId, FullName, SectorId, LevelCode)
ON (dest.StandardId = src.StandardId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName, SectorId = src.SectorId, LevelCode = src.LevelCode
WHEN NOT MATCHED THEN
	INSERT (StandardId, FullName, SectorId, LevelCode)
	VALUES (src.StandardId, src.FullName, src.SectorId, src.LevelCode)
;

MERGE Reference.[Standard] AS dest
USING (SELECT 60, 'Surveying Technician', 26, '3') AS src (StandardId, FullName, SectorId, LevelCode)
ON (dest.StandardId = src.StandardId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName, SectorId = src.SectorId, LevelCode = src.LevelCode
WHEN NOT MATCHED THEN
	INSERT (StandardId, FullName, SectorId, LevelCode)
	VALUES (src.StandardId, src.FullName, src.SectorId, src.LevelCode)
;

SELECT * FROM Reference.[Standard]
