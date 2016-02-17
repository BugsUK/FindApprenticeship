﻿/*
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

-- Data
:r ".\Scripts\Data\dbo.County.Upsert.sql"
:r ".\Scripts\Data\dbo.EmployerTrainingProviderStatus.Upsert.sql"
:r ".\Scripts\Data\dbo.LocalAuthority.Upsert.sql"
:r ".\Scripts\Data\dbo.ProviderSiteRelationshipType.Upsert.sql"
:r ".\Scripts\Data\Provider.ProviderUserStatus.Upsert.sql"
:r ".\Scripts\Data\dbo.VacancyLocationType.Upsert.sql"
:r ".\Scripts\Data\dbo.WageUnit.Upsert.sql"
:r ".\Scripts\Data\dbo.DurationType.Upsert.sql"
:r ".\Scripts\Data\dbo.ApprenticeshipType.Upsert.sql"
:r ".\Scripts\Data\Sync.SyncParams.Upsert.sql"

-- Test Data
:r ".\Scripts\TestData\dbo.TestProviders.Upsert.sql"
