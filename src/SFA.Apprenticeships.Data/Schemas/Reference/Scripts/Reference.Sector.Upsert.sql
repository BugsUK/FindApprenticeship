MERGE Reference.Sector AS dest
USING (SELECT '1', 'Actuarial') AS src (SectorId, FullName)
ON (dest.SectorId = src.SectorId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName
WHEN NOT MATCHED THEN
	INSERT (SectorId, FullName)
	VALUES (src.SectorId, src.FullName)
;

MERGE Reference.Sector AS dest
USING (SELECT '2', 'Aerospace') AS src (SectorId, FullName)
ON (dest.SectorId = src.SectorId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName
WHEN NOT MATCHED THEN
	INSERT (SectorId, FullName)
	VALUES (src.SectorId, src.FullName)
;

MERGE Reference.Sector AS dest
USING (SELECT '3', 'Automotive') AS src (SectorId, FullName)
ON (dest.SectorId = src.SectorId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName
WHEN NOT MATCHED THEN
	INSERT (SectorId, FullName)
	VALUES (src.SectorId, src.FullName)
;

MERGE Reference.Sector AS dest
USING (SELECT '4', 'Automotive retail') AS src (SectorId, FullName)
ON (dest.SectorId = src.SectorId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName
WHEN NOT MATCHED THEN
	INSERT (SectorId, FullName)
	VALUES (src.SectorId, src.FullName)
;

MERGE Reference.Sector AS dest
USING (SELECT '5', 'Butchery') AS src (SectorId, FullName)
ON (dest.SectorId = src.SectorId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName
WHEN NOT MATCHED THEN
	INSERT (SectorId, FullName)
	VALUES (src.SectorId, src.FullName)
;

MERGE Reference.Sector AS dest
USING (SELECT '6', 'Conveyancing and probate') AS src (SectorId, FullName)
ON (dest.SectorId = src.SectorId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName
WHEN NOT MATCHED THEN
	INSERT (SectorId, FullName)
	VALUES (src.SectorId, src.FullName)
;

MERGE Reference.Sector AS dest
USING (SELECT '7', 'Defence') AS src (SectorId, FullName)
ON (dest.SectorId = src.SectorId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName
WHEN NOT MATCHED THEN
	INSERT (SectorId, FullName)
	VALUES (src.SectorId, src.FullName)
;

MERGE Reference.Sector AS dest
USING (SELECT '8', 'Dental health') AS src (SectorId, FullName)
ON (dest.SectorId = src.SectorId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName
WHEN NOT MATCHED THEN
	INSERT (SectorId, FullName)
	VALUES (src.SectorId, src.FullName)
;

MERGE Reference.Sector AS dest
USING (SELECT '9', 'Digital Industries') AS src (SectorId, FullName)
ON (dest.SectorId = src.SectorId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName
WHEN NOT MATCHED THEN
	INSERT (SectorId, FullName)
	VALUES (src.SectorId, src.FullName)
;

MERGE Reference.Sector AS dest
USING (SELECT '10', 'Electrotechnical') AS src (SectorId, FullName)
ON (dest.SectorId = src.SectorId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName
WHEN NOT MATCHED THEN
	INSERT (SectorId, FullName)
	VALUES (src.SectorId, src.FullName)
;

MERGE Reference.Sector AS dest
USING (SELECT '11', 'Energy & Utilities') AS src (SectorId, FullName)
ON (dest.SectorId = src.SectorId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName
WHEN NOT MATCHED THEN
	INSERT (SectorId, FullName)
	VALUES (src.SectorId, src.FullName)
;

MERGE Reference.Sector AS dest
USING (SELECT '12', 'Financial Services') AS src (SectorId, FullName)
ON (dest.SectorId = src.SectorId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName
WHEN NOT MATCHED THEN
	INSERT (SectorId, FullName)
	VALUES (src.SectorId, src.FullName)
;

MERGE Reference.Sector AS dest
USING (SELECT '13', 'Food and Drink') AS src (SectorId, FullName)
ON (dest.SectorId = src.SectorId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName
WHEN NOT MATCHED THEN
	INSERT (SectorId, FullName)
	VALUES (src.SectorId, src.FullName)
;

MERGE Reference.Sector AS dest
USING (SELECT '14', 'Golf Greenkeeping (horticulture)') AS src (SectorId, FullName)
ON (dest.SectorId = src.SectorId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName
WHEN NOT MATCHED THEN
	INSERT (SectorId, FullName)
	VALUES (src.SectorId, src.FullName)
;

MERGE Reference.Sector AS dest
USING (SELECT '15', 'Insurance') AS src (SectorId, FullName)
ON (dest.SectorId = src.SectorId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName
WHEN NOT MATCHED THEN
	INSERT (SectorId, FullName)
	VALUES (src.SectorId, src.FullName)
;

MERGE Reference.Sector AS dest
USING (SELECT '16', 'Law') AS src (SectorId, FullName)
ON (dest.SectorId = src.SectorId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName
WHEN NOT MATCHED THEN
	INSERT (SectorId, FullName)
	VALUES (src.SectorId, src.FullName)
;

MERGE Reference.Sector AS dest
USING (SELECT '17', 'Leadership & Management') AS src (SectorId, FullName)
ON (dest.SectorId = src.SectorId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName
WHEN NOT MATCHED THEN
	INSERT (SectorId, FullName)
	VALUES (src.SectorId, src.FullName)
;

MERGE Reference.Sector AS dest
USING (SELECT '18', 'Life and Industrial Sciences') AS src (SectorId, FullName)
ON (dest.SectorId = src.SectorId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName
WHEN NOT MATCHED THEN
	INSERT (SectorId, FullName)
	VALUES (src.SectorId, src.FullName)
;

MERGE Reference.Sector AS dest
USING (SELECT '19', 'Maritime') AS src (SectorId, FullName)
ON (dest.SectorId = src.SectorId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName
WHEN NOT MATCHED THEN
	INSERT (SectorId, FullName)
	VALUES (src.SectorId, src.FullName)
;

MERGE Reference.Sector AS dest
USING (SELECT '20', 'Media') AS src (SectorId, FullName)
ON (dest.SectorId = src.SectorId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName
WHEN NOT MATCHED THEN
	INSERT (SectorId, FullName)
	VALUES (src.SectorId, src.FullName)
;

MERGE Reference.Sector AS dest
USING (SELECT '21', 'Nuclear') AS src (SectorId, FullName)
ON (dest.SectorId = src.SectorId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName
WHEN NOT MATCHED THEN
	INSERT (SectorId, FullName)
	VALUES (src.SectorId, src.FullName)
;

MERGE Reference.Sector AS dest
USING (SELECT '22', 'Property services') AS src (SectorId, FullName)
ON (dest.SectorId = src.SectorId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName
WHEN NOT MATCHED THEN
	INSERT (SectorId, FullName)
	VALUES (src.SectorId, src.FullName)
;

MERGE Reference.Sector AS dest
USING (SELECT '23', 'Public Service') AS src (SectorId, FullName)
ON (dest.SectorId = src.SectorId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName
WHEN NOT MATCHED THEN
	INSERT (SectorId, FullName)
	VALUES (src.SectorId, src.FullName)
;

MERGE Reference.Sector AS dest
USING (SELECT '24', 'Rail Design') AS src (SectorId, FullName)
ON (dest.SectorId = src.SectorId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName
WHEN NOT MATCHED THEN
	INSERT (SectorId, FullName)
	VALUES (src.SectorId, src.FullName)
;

MERGE Reference.Sector AS dest
USING (SELECT '25', 'Refrigeration air') AS src (SectorId, FullName)
ON (dest.SectorId = src.SectorId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName
WHEN NOT MATCHED THEN
	INSERT (SectorId, FullName)
	VALUES (src.SectorId, src.FullName)
;

MERGE Reference.Sector AS dest
USING (SELECT '26', 'Surveying') AS src (SectorId, FullName)
ON (dest.SectorId = src.SectorId)
WHEN MATCHED THEN 
	UPDATE SET FullName = src.FullName
WHEN NOT MATCHED THEN
	INSERT (SectorId, FullName)
	VALUES (src.SectorId, src.FullName)
;

SELECT * FROM Reference.Sector
