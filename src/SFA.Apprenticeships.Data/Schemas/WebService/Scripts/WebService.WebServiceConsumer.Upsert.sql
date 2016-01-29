MERGE INTO [WebService].[WebServiceConsumer] AS TARGET
USING (VALUES
  ('P', '00000000-0000-0000-0000-000000000000', N'Test Web Service Consumer (Fake 0)', N'N1i1h7PyTdcbPgKl', 0, 0, 0, 0),
  ('E', 'bbd33d6d-fc94-4be3-9e52-282bf7293356', N'Test Web Service Consumer (Fake 1)', N'password', 1, 1, 1, 1),
  ('T', '63c545db-d08f-4a9b-b05b-33727432e987', N'Test Web Service Consumer (Fake 2)', N'letmein', 1, 0, 0, 0)
)
AS SOURCE (
	WebServiceConsumerTypeCode,
	ExternalSystemId,
	ExternalSystemName,
	ExternalSystemPassword,
	AllowReferenceDataService,
	AllowVacancyUploadService,
	AllowVacancySummaryService,
	AllowVacancyDetailService
)
ON [Target].[ExternalSystemId] = [Source].[ExternalSystemId] 
WHEN MATCHED THEN 
UPDATE SET
	WebServiceConsumerTypeCode = [Source].[WebServiceConsumerTypeCode],
	ExternalSystemId = [Source].[ExternalSystemId],
	ExternalSystemName = [Source].[ExternalSystemName],
	ExternalSystemPassword = [Source].[ExternalSystemPassword],
	AllowReferenceDataService = [Source].[AllowReferenceDataService],
	AllowVacancyUploadService = [Source].[AllowVacancyUploadService],
	AllowVacancySummaryService = [Source].[AllowVacancySummaryService],
	AllowVacancyDetailService = [Source].[AllowVacancyDetailService]
WHEN NOT MATCHED BY TARGET THEN 
INSERT (
	WebServiceConsumerTypeCode,
	ExternalSystemId,
	ExternalSystemName,
	ExternalSystemPassword,
	AllowReferenceDataService,
	AllowVacancyUploadService,
	AllowVacancySummaryService,
	AllowVacancyDetailService
)
VALUES (
	WebServiceConsumerTypeCode,
	ExternalSystemId,
	ExternalSystemName,
	ExternalSystemPassword,
	AllowReferenceDataService,
	AllowVacancyUploadService,
	AllowVacancySummaryService,
	AllowVacancyDetailService
);

SELECT * FROM [WebService].[WebServiceConsumer]
