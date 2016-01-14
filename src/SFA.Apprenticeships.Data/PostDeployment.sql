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

-- Address
:r ".\Schemas\Address\Scripts\Address.ValidationSource.Upsert.sql"

-- Reference
:r ".\Schemas\Reference\Scripts\Reference.FrameworkStatus.Upsert.sql"
:r ".\Schemas\Reference\Scripts\Reference.Level.Upsert.sql"
:r ".\Schemas\Reference\Scripts\Reference.OccupationStatus.Upsert.sql"
:r ".\Schemas\Reference\Scripts\Reference.Sector.Upsert.sql"
:r ".\Schemas\Reference\Scripts\Reference.Standard.Upsert.sql"

-- Vacancy
:r ".\Schemas\Vacancy\Scripts\Vacancy.DurationType.Upsert.sql"
:r ".\Schemas\Vacancy\Scripts\Vacancy.TrainingType.Upsert.sql"
:r ".\Schemas\Vacancy\Scripts\Vacancy.VacancyLocationType.Upsert.sql"
:r ".\Schemas\Vacancy\Scripts\Vacancy.VacancyPartyRelationshipType.Upsert.sql"
:r ".\Schemas\Vacancy\Scripts\Vacancy.VacancyPartyType.Upsert.sql"
:r ".\Schemas\Vacancy\Scripts\Vacancy.VacancyStatus.Upsert.sql"
:r ".\Schemas\Vacancy\Scripts\Vacancy.VacancyType.Upsert.sql"
:r ".\Schemas\Vacancy\Scripts\Vacancy.WageType.Upsert.sql"
:r ".\Schemas\Vacancy\Scripts\Vacancy.WageInterval.Upsert.sql"

-- Web Proxy
:r ".\Schemas\WebProxy\Scripts\WebProxy.WebProxyConsumer.Upsert.sql"
