[CmdletBinding()]
Param(
    [Parameter(Mandatory=$True)]
    [string]$buildConfiguration,
	
	[Parameter(Mandatory=$True)]
    [string]$configurationStorageConnectionString,
	
	[Parameter(Mandatory=$True)]
    [string]$authorizationInfo,
	
	[Parameter(Mandatory=$True)]
    [string]$configurationEventHubLogConnectionString,
	
	[Parameter(Mandatory=$True)]
    [string]$decryptionKey,
	
	[Parameter(Mandatory=$True)]
    [string]$validationKey
)

$TextInfo = (Get-Culture).TextInfo
$buildConfiguration = $TextInfo.ToTitleCase($buildConfiguration)

$configPath = "$PSScriptRoot\..\..\config\$buildConfiguration\Configs"
$settingsConfigFile = "$configPath\settings.config"
$dataCacheClientFile = "$configPath\dataCacheClient.config"
$machineKeyFile = "$configPath\machineKey.config"

$cscfgPathFormat = "$PSScriptRoot\..\..\config\{0}\ServiceConfiguration.$buildConfiguration.cscfg"
$workerRolesCscfgFile = $cscfgPathFormat -f "SFA.Apprenticeships.Infrastructure.WorkerRoles.Azure"
$migrateRolesCsfgFile = $csfgPathFormat -f "SFA.Apprenticeships.Infrastructure.MigrateRole.Azure"
$candidateWebRoleCscfgFile = $cscfgPathFormat -f "SFA.Apprenticeships.Web.Candidate.Azure"
$recruitWebRoleCscfgFile = $cscfgPathFormat -f "SFA.Apprenticeships.Web.Recruit.Azure"
$manageWebRoleCscfgFile = $cscfgPathFormat -f "SFA.Apprenticeships.Web.Manage.Azure"
$vacancyRoleCscfgFile = $cscfgPathFormat -f "SFA.Apprenticeships.Service.Vacancy.Azure"
$avmsCompatibilityServiceRoleCscfgFile = $cscfgPathFormat -f "SFA.Apprenticeship.Api.AvService.Azure"

Write-Output "Updating $settingsConfigFile with ConfigurationStorageConnectionString: $configurationStorageConnectionString"
$configurationStorageConnectionStringAppSetting = ('<add key="ConfigurationStorageConnectionString" value="' + $configurationStorageConnectionString + '" />')
(gc $settingsConfigFile) -replace '<add key="ConfigurationStorageConnectionString" value=".*?" />', $configurationStorageConnectionStringAppSetting | sc $settingsConfigFile
Write-Output "$settingsConfigFile updated"

Write-Output "Updating $settingsConfigFile with ConfigurationEventHubLogConnectionString: $configurationEventHubLogConnectionString"
$configurationEventHubLogConnectionStringAppSetting = ('<add key="ConfigurationEventHubLogConnectionString" value="' + $configurationEventHubLogConnectionString + '" />')
(gc $settingsConfigFile) -replace '<add key="ConfigurationEventHubLogConnectionString" value=".*?" />', $configurationEventHubLogConnectionStringAppSetting | sc $settingsConfigFile
Write-Output "$settingsConfigFile updated"

Write-Output "Updating $dataCacheClientFile with AuthorizationInfo: $authorizationInfo"
$messageSecurityAuthorizationInfo = ('<messageSecurity authorizationInfo="' + $authorizationInfo + '" />')
(gc $dataCacheClientFile) -replace '<messageSecurity authorizationInfo=".*?" />', $messageSecurityAuthorizationInfo | sc $dataCacheClientFile
Write-Output "$dataCacheClientFile updated"

Write-Output "Updating $machineKeyFile with DecryptionKey: $decryptionKey ValidationKey: $validationKey"
$machineKey = ('<machineKey decryption="Auto" decryptionKey="' + $decryptionKey + '" validation="SHA1" validationKey="' + $validationKey + '" />')
(gc $machineKeyFile) -replace '<machineKey .*? />', $machineKey | sc $machineKeyFile
Write-Output "$machineKeyFile updated"

$configurationStorageConnectionStringSetting = ('<Setting name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" value="' + $configurationStorageConnectionString + '" />')

Write-Output "Updating $workerRolesCscfgFile with ConfigurationStorageConnectionString: $configurationStorageConnectionString"
(gc $workerRolesCscfgFile) -replace '<Setting name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" value=".*?" />', $configurationStorageConnectionStringSetting | sc $workerRolesCscfgFile
Write-Output "$workerRolesCscfgFile updated"

Write-Output "Updating $migrateRolesCsfgFile with ConfigurationStorageConnectionString: $configurationStorageConnectionString"
(gc $migrateRolesCsfgFile) -replace '<Setting name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" value=".*?" />', $configurationStorageConnectionStringSetting | sc $migrateRolesCsfgFile
Write-Output "$migrateRolesCsfgFile updated"

Write-Output "Updating $candidateWebRoleCscfgFile with ConfigurationStorageConnectionString: $configurationStorageConnectionString"
(gc $candidateWebRoleCscfgFile) -replace '<Setting name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" value=".*?" />', $configurationStorageConnectionStringSetting | sc $candidateWebRoleCscfgFile
Write-Output "$candidateWebRoleCscfgFile updated"

Write-Output "Updating $recruitWebRoleCscfgFile with ConfigurationStorageConnectionString: $configurationStorageConnectionString"
(gc $recruitWebRoleCscfgFile) -replace '<Setting name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" value=".*?" />', $configurationStorageConnectionStringSetting | sc $recruitWebRoleCscfgFile
Write-Output "$recruitWebRoleCscfgFile updated"

Write-Output "Updating $manageWebRoleCscfgFile with ConfigurationStorageConnectionString: $configurationStorageConnectionString"
(gc $manageWebRoleCscfgFile) -replace '<Setting name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" value=".*?" />', $configurationStorageConnectionStringSetting | sc $manageWebRoleCscfgFile
Write-Output "$manageWebRoleCscfgFile updated"

Write-Output "Updating $vacancyRoleCscfgFile with ConfigurationStorageConnectionString: $configurationStorageConnectionString"
(gc $vacancyRoleCscfgFile) -replace '<Setting name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" value=".*?" />', $configurationStorageConnectionStringSetting | sc $vacancyRoleCscfgFile
Write-Output "$vacancyRoleCscfgFile updated"

Write-Output "Updating $avmsCompatibilityServiceRoleCscfgFile with ConfigurationStorageConnectionString: $configurationStorageConnectionString"
(gc $avmsCompatibilityServiceRoleCscfgFile) -replace '<Setting name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" value=".*?" />', $configurationStorageConnectionStringSetting | sc $avmsCompatibilityServiceRoleCscfgFile
Write-Output "$avmsCompatibilityServiceRoleCscfgFile updated"