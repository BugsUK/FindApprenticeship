[CmdletBinding()]
Param(
    [Parameter(Mandatory=$True)]
    [string]$buildConfiguration,
	
	[Parameter(Mandatory=$True)]
    [string]$configurationStorageConnectionString,
	
	[Parameter(Mandatory=$True)]
    [string]$authorizationInfo
)

$TextInfo = (Get-Culture).TextInfo
$buildConfiguration = $TextInfo.ToTitleCase($buildConfiguration)

$configPath = "$PSScriptRoot\..\..\config\$buildConfiguration\Configs"
$settingsConfigFile = "$configPath\settings.config"
$dataCacheClientFile = "$configPath\dataCacheClient.config"

$cscfgPathFormat = "$PSScriptRoot\..\..\config\{0}\ServiceConfiguration.$buildConfiguration.cscfg"
$workerRolesCscfgFile = $cscfgPathFormat -f "SFA.Apprenticeships.Infrastructure.WorkerRoles.Azure"
$webRoleCscfgFile = $cscfgPathFormat -f "SFA.Apprenticeships.Web.Candidate.Azure"
$vacancyRoleCscfgFile = $cscfgPathFormat -f "SFA.Apprenticeships.Service.Vacancy.Azure"

Write-Output "Updating $settingsConfigFile with ConfigurationStorageConnectionString: $configurationStorageConnectionString"
$configurationStorageConnectionStringAppSetting = ('<add key="ConfigurationStorageConnectionString" value="' + $configurationStorageConnectionString + '" />')
(gc $settingsConfigFile) -replace '<add key="ConfigurationStorageConnectionString" value=".*?" />', $configurationStorageConnectionStringAppSetting | sc $settingsConfigFile
Write-Output "$settingsConfigFile updated"

Write-Output "Updating $dataCacheClientFile with AuthorizationInfo: $authorizationInfo"
$messageSecurityAuthorizationInfo = ('<messageSecurity authorizationInfo="' + $authorizationInfo + '" />')
(gc $dataCacheClientFile) -replace '<messageSecurity authorizationInfo=".*?" />', $messageSecurityAuthorizationInfo | sc $dataCacheClientFile
Write-Output "$dataCacheClientFile updated"

$configurationStorageConnectionStringSetting = ('<Setting name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" value="' + $configurationStorageConnectionString + '" />')

Write-Output "Updating $workerRolesCscfgFile with ConfigurationStorageConnectionString: $configurationStorageConnectionString"
(gc $workerRolesCscfgFile) -replace '<Setting name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" value=".*?" />', $configurationStorageConnectionStringSetting | sc $workerRolesCscfgFile
Write-Output "$workerRolesCscfgFile updated"

Write-Output "Updating $webRoleCscfgFile with ConfigurationStorageConnectionString: $configurationStorageConnectionString"
(gc $webRoleCscfgFile) -replace '<Setting name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" value=".*?" />', $configurationStorageConnectionStringSetting | sc $webRoleCscfgFile
Write-Output "$webRoleCscfgFile updated"

Write-Output "Updating $vacancyRoleCscfgFile with ConfigurationStorageConnectionString: $configurationStorageConnectionString"
(gc $vacancyRoleCscfgFile) -replace '<Setting name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" value=".*?" />', $configurationStorageConnectionStringSetting | sc $vacancyRoleCscfgFile
Write-Output "$vacancyRoleCscfgFile updated"