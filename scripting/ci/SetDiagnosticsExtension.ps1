[CmdletBinding()]
Param(
	[Parameter(Mandatory=$True)]
    [string]$environmentName,
	
	[Parameter(Mandatory=$True)]
    [string]$projectName,

    [Parameter(Mandatory = $True)]
    [string]$storageName,

    [Parameter(Mandatory=$True)]
    [string]$storageAccessKey,

    [Parameter(Mandatory=$True)]
    [string]$cloudServiceName,

    [Parameter(Mandatory=$True)]
    [string]$slotName,

    [Parameter(Mandatory=$True)]
    [string]$roleName
)

Import-Module Azure

$extensionPath = "Beta\src\$projectName\bin\$environmentName\app.publish\Extensions\PaaSDiagnostics.$roleName.PubConfig.xml"

$storage = New-AzureStorageContext -StorageAccountName $storageName -StorageAccountKey $storageAccessKey
Set-AzureServiceDiagnosticsExtension -ServiceName $cloudServiceName -Slot $slotName -Role $roleName -StorageContext $storage -DiagnosticsConfigurationPath $extensionPath