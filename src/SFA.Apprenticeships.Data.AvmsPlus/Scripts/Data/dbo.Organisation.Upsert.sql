SET IDENTITY_INSERT dbo.Organisation ON;
GO

MERGE INTO dbo.Organisation AS Target
USING(VALUES
     (1,
      'LA',
      'LA',
      'Local Authority'
     ),
     (2,
      'JCP',
      'JCP',
      'Jobcentre Plus'
     ),
     (3,
      'OTH',
      'OTH',
      'Other'
     ),
     (4,
      'NCS',
      'NCS',
      'National Careers Service'
     ),
     (5,
      'SCP',
      'SCP',
      'School/College/Provider'
     )) AS Source(OrganisationId, CodeName, ShortName, FullName)
ON Target.OrganisationId = Source.OrganisationId
-- update matched rows 
    WHEN MATCHED
    THEN UPDATE SET
                    CodeName = Source.CodeName,
                    ShortName = Source.ShortName,
                    FullName = Source.FullName
-- insert new rows 
    WHEN NOT MATCHED BY TARGET
    THEN INSERT(OrganisationId,
                CodeName,
                ShortName,
                FullName) VALUES
(OrganisationId,
 CodeName,
 ShortName,
 FullName
)
-- delete rows that are in the target but not the source 
    WHEN NOT MATCHED BY SOURCE
    THEN DELETE;
SET IDENTITY_INSERT dbo.Organisation OFF;
GO