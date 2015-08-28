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

& powershell -File $scriptPath -environmentName $environmentName -projectName "SFA.Apprenticeships.Infrastructure.WorkerRoles.Azure" -storageName $storageName -storageAccessKey $storageAccessKey -cloudServiceName "sfa-workerroles-$environmentName" -slotName $slotName -roleName "SFA.Apprenticeships.Infrastructure.Monitor"
& powershell -File $scriptPath -environmentName $environmentName -projectName "SFA.Apprenticeships.Infrastructure.WorkerRoles.Azure" -storageName $storageName -storageAccessKey $storageAccessKey -cloudServiceName "sfa-workerroles-$environmentName" -slotName $slotName -roleName "SFA.Apprenticeships.Infrastructure.Processes"
& powershell -File $scriptPath -environmentName $environmentName -projectName "SFA.Apprenticeships.Infrastructure.WorkerRoles.Azure" -storageName $storageName -storageAccessKey $storageAccessKey -cloudServiceName "sfa-workerroles-$environmentName" -slotName $slotName -roleName "SFA.Apprenticeships.Infrastructure.ScheduledJobs"