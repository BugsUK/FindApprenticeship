[CmdletBinding()]
Param(
	[Parameter(Mandatory=$True)]
    [string]$environmentName,

    [Parameter(Mandatory = $True)]
    [string]$storageName,

    [Parameter(Mandatory=$True)]
    [string]$storageAccessKey,

    [Parameter(Mandatory=$True)]
    [string]$slotName
)

Import-Module Azure

$scriptPath = "$PSScriptRoot\SetDiagnosticsExtension.ps1"

& powershell -File $scriptPath -environmentName $environmentName -projectName "SFA.Apprenticeships.Web.Candidate.Azure" -storageName $storageName -storageAccessKey $storageAccessKey -cloudServiceName "sfa-apprenticeships-$environmentName" -slotName $slotName -roleName "SFA.Apprenticeships.Web.Candidate"