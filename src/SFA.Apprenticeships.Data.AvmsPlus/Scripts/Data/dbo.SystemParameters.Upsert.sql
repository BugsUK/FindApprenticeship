SET IDENTITY_INSERT [dbo].[SystemParameters] ON
GO

MERGE INTO [dbo].[SystemParameters] AS Target 
USING (VALUES 
	(2,'NASSupportThresholdForUnSuccessfulCandidate','int','3','0',NULL,NULL,'NASSupportThresholdForUnSuccessfulCandidate'),
	(4,'MaxAllowableSentApplications','int','10','1','1','99','Maximum number of applications a candidate can make'),
	(5,'NumberOfDaysForFilledVacanciesWithOpenApplications','int','20','1','1','99','Alert: Number of days before a filled vacancy with open applications appears'),
	(7,'NumberOfDaysEmployerWithoutTrainingProvider','int','1','1','1','99','Alert: Number of days before an Employer not linked to Training Provider appears'),
	(9,'NoApplicationsOnly','bool','true','0',NULL,NULL,'NoApplicationsOnly'),
	(10,'DaysFromClosingDateForVacancyNotFilled','int','60','1','1','365','Alert: Number of days from closing date, that a vacancy that has not been filled appears'),
	(11,'DaysFromClosingDateFor0ApplicationVacancies','int','20','1','1','365','Alert: Number of days before the closing date that a vacancy with zero applications will appear'),
	(12,'NumberOfDaysApplicationsNotProgressed','int','60','1','1','365','Alert: Number of days that an application has not moved status before it appears'),
	(13,'SQLServerReportingServerMainReportURL','string','http://$ssrs_static.reports_url$/Pages/Folder.aspx','0',NULL,NULL,'SQLServerReportingServerMainReportURL'),
	(14,'ReportCandidateInactiveDays','integer','30','1','1','99','Number of days for an inactive candidate to appear in the count on Registered Candidate report'),
	(15,'ReportMaxNumberOfPostcodes','integer','5','1','1','99','Number of Postcodes available to search on in reports'),
	(16,'MinimumWeeklyWage','int','0','1','0','999','Minimum weekly wage (£)'),
	(17,'DatabaseVersion','string','5.02.03','0',NULL,NULL,'DatabaseVersion'),
	(19,'SQLServerReportingServerEmployerURL','string','ItemPath=%2fLSCReports+Unit+testing%2fProvider+Reports&ViewMode=List','0',NULL,NULL,'SQLServerReportingServerEmployerURL'),
	(20,'SQLServerReportingServerProviderURL','string','ItemPath=%2fLSCReports+Unit+testing%2fProvider+Reports&ViewMode=List','0',NULL,NULL,'SQLServerReportingServerProviderURL'),
	(21,'SQLServerReportingServerNasSupportURL','string','ItemPath=%2fLSCReports+Unit+testing%2fOperational+Reports&ViewMode=List','0',NULL,NULL,'SQLServerReportingServerNasSupportURL'),
	(22,'MinimumApplicantAge','int','15','1','11','18','Age below which a candidate will be prompted when applying for a vacancy, to confirm age'),
	(23,'SmsCodeTitle','string','Activation Code','0',NULL,NULL,'SmsCodeTitle'),
	(24,'SmsLandingUrl','string','the website via My Alerts','0',NULL,NULL,'SmsLandingUrl'),
	(25,'NewVacancySmsAlertMessage','string','Apprenticeship vacancies has found new jobs matching your favourite search. View these and others by logging back in and running your favourite search','0',NULL,NULL,'NewVacancySmsAlertMessage'),

	(26,'AwaitingActivationPeriod','int','30','1','1','999','Number of days before a pre-registered candidate, who has not responded to the verification email  will be deleted from the system'),
	(27,'InactivePeriod','int','365','1','1','999','Number of days before a registered candidate that has not accessed the system will be deleted'),

	(28,'ShowSuccessfulRecords','int','10','1','0','99','Vacancy Ladder: Number of Successful entries before opens in closed view'),
	(29,'ShowInProgressRecords','int','30','1','0','99','Vacancy Ladder: Number of In Progress entries before opens in closed view'),
	(30,'ShowNewApplicationRecords','int','30','1','0','99','Vacancy Ladder: Number of New Applications entries before opens in closed view'),
	(31,'ShowUnsucessfulRecords','int','0','1','0','99','Vacancy Ladder: Number of Unsuccessful entries before opens in closed view'),
	(32,'ShowWithdrawnRecords','int','0','1','0','99','Vacancy Ladder: Number of Withdrawn entries before opens in closed view'),
	(33,'DeRegisterBatchMAXRecordsToProcess','int','5000','0','1','99999','DeRegisterBatch MAX Candidate Records To Process'),
	(34,'AV Description','string','http:\\www.apprenticeshipvacancies.gov.uk\Employers.aspx','0',null,null,'If you are interested in finding out more about employing apprentices, the benefits they can bring to your business, and the benefits of Apprenticeship vacancies please visit our website and complete the online enquiry form.'),
	(35,'VacancyRSSFeedTitle','string','Vacancies','0',null,null,'This will be the title of the RSS feed that will be visible by the RSS reader'),
	(36,'VacancyRSSFeedDescription','string','The Apprenticeship Vacancies System provides a list of new vacancies','0',null,null,'This will be the description of the RSS feed that will be visible by the RSS reader'),
	(37,'VacancyRSSImageURL','string','https://apprenticeshipvacancymatchingservice.lsc.gov.uk/navms/img/master/app_logo.jpg','0',null,null,'This is the image that will be used for the Vacancy RSS Feed'),
	(38,'VacancyRSSCopyrightInformation','string','RSS feed supplied by the National Apprenticeship Service, © Chief Executive of Skills Funding','0',null,null,'This is the copyright information for the RSS feed service'),
	(39,'VacancyRSSDaysUntilVacancyExpiry','int','7','0',null,null,'This is the days until a vacancy closes for the vacancy summary RSS feed when feed type is set to 2'),
	(40,'VacancyRSSLowApplicantAmount','int','5','0',null,null,'This is the max amount of applicants limit for the vacancy summary RSS feed when feed type is set to 2'),
	(41,'VacancyRssAlternateLink','string','https://apprenticeshipvacancymatchingservice.lsc.gov.uk/navms/Forms/Candidate/VisitorLanding.aspx','0',null,null,'This is the alternate link for the RSS feed which is used when the RSS feed image is clicked'),

	(42,'WalesLink','url','http://www.careerswales.com/server.php?show=nav.home&outputLang=en','1',null,null,'Landing page - Wales link'),
	(43,'ScotlandLink','url','http://www.myworldofwork.co.uk/modernapprenticeships','1',null,null,'Landing page - Scotland Link'),
	(44,'NorthernIrelandLink','url','http://www.nidirect.gov.uk/index/information-and-services/education-and-learning/14-19/its-your-choice-options-after-16/apprenticeshipsni/apprentices.htm','1',null,null,'Landing page - Northern Ireland link'),

	(45,'MaxAgeEShotJobs','int','7','0',1,9999,'This is the number of days to keep completed Eshot jobs in the system before deletion'),
	(46,'MaxAgeVacanciesPostedInError','int','180','0',1,9999,'This is the number of days after a posted in error vacancy is considered for deletion'),
	(47,'MaxAgeVacanciesWithdrawn','int','365','0',1,9999,'This is the number of days after a withdran vacancy is considered for deletion'),
	(48,'MaxAgeVacanciesDraft','int','180','0',1,9999,'This is the number of days after a draft vacancy is considered for deletion'),
	(49,'MaxAgeVacanciesCompleted','int','730','0',1,9999,'This is the number of days after a completed vacancy is considered for deletion'),
	(50,'MaxAgeVacanciesReferred','int','365','0',1,9999,'This is the number of days after a referred vacancy is considered for deletion'),

	(51,'MaxAgeMessages','int','31','0',1,9999,'This is the number of days an external/internal message is kept before deletion'),
	(52,'MaxAgeAudit','int','7','0',1,9999,'This is the number of days an audit record is kept before deletion'),
	(53,'VacancyUrlForRssFeedToFAA','bool','false','TRUE',NULL,NULL,'Vacancy URL in the RSS Feed should point to Find an Apprenticeship? (Default is back to Apprenticeship Vacancies)'),
	(54,'VacancyUrlForWebServicesToFAA','bool','false','TRUE',NULL,NULL,'Vacancy URL in the Web Services should point to Find an Apprenticeship? (Default is back to Apprenticeship Vacancies)'),
	(55,'StopNewRegistration','bool','false','TRUE',NULL,NULL,'Stop new registrations within the system and direct new candidates to Find an Apprenticeship?'),
	(56,'StopNewApplications','bool','false','TRUE',NULL,NULL,'Stop the system from creating new Applications?'),
	(57,'RedirectVacancyDetailstoFaa','bool','false','TRUE',NULL,NULL,'Redirect Vacancy Details to Find an Apprenticeship?')
) 
AS Source (SystemParametersId, ParameterName, ParameterType, ParameterValue, Editable, LowerLimit, UpperLimit, [Description]) 
ON Target.SystemParametersId = Source.SystemParametersId 
-- update matched rows 
WHEN MATCHED THEN 
UPDATE SET ParameterName = Source.ParameterName, ParameterType = Source.ParameterType, ParameterValue = Source.ParameterValue, Editable = Source.Editable, LowerLimit = Source.LowerLimit, UpperLimit = Source.UpperLimit, [Description] = Source.[Description]
-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN 
INSERT (SystemParametersId, ParameterName, ParameterType, ParameterValue, Editable, LowerLimit, UpperLimit, [Description]) 
VALUES (SystemParametersId, ParameterName, ParameterType, ParameterValue, Editable, LowerLimit, UpperLimit, [Description]) 
-- delete rows that are in the target but not the source 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[SystemParameters] OFF
GO
