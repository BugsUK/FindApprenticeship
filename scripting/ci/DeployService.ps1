[CmdletBinding()]
Param(
    [Parameter(Mandatory = $True, Position=0)]
    [string]$EnvironmentName,

    [Parameter(Mandatory = $True, Position=1)]
    [string]$CloudServiceName,

    [Parameter(Mandatory = $True, Position=2)]
    [string]$FriendlyCloudServiceName,

    [Parameter(Mandatory = $True, Position=3)]
    [string]$CloudServiceVersion,

    [Parameter(Mandatory=$True, Position=4)]
    [string]$SlotName,

    [Parameter(Mandatory=$True, Position=5)]
    [string]$nasPfxFile,

    [Parameter(Mandatory=$True, Position=6)]
    [string]$nasPfxPassword,

    [Parameter(Mandatory=$False, Position=7)]
    [string]$webPfxFile,

    [Parameter(Mandatory=$False, Position=8)]
    [string]$webPfxPassword,

    [Parameter(Mandatory=$False, Position=9)]
    [string]$subscriptionName,

    [Parameter(Mandatory=$False, Position=10)]
    [string]$storageAccountName,

    [Parameter(Mandatory=$False, Position=11)]
    [string]$affinityGroup
)

Function Start-ServiceDeployment() {    

	Import-Module Azure
    Set-AzureSubscription -CurrentStorageAccount $storageAccountName -SubscriptionName $subscriptionName
	Select-AzureSubscription $subscriptionName

    $SubscriptionId = Get-AzureSubscriptionId
	$CspkgFileName = "Beta\src\$CloudServiceName\bin\$EnvironmentName\app.publish\$CloudServiceName.cspkg"
    $CscfgFileName = "Beta\src\$CloudServiceName\bin\$EnvironmentName\app.publish\ServiceConfiguration.$EnvironmentName.cscfg"

    Publish-CloudServicePackage $CspkgFileName $CscfgFileName
}

Function Get-AzureSubscriptionId {

    $SubscriptionId = Get-AzureSubscription | Where SubscriptionName -eq $subscriptionName | Select SubscriptionId

    if ($SubscriptionId -eq $null) {
        Throw "SubscriptionId not found for Subscription Name: $subscriptionName."
    }

    $SubscriptionId
}

Function Publish-CloudServicePackage ($CspkgFileName, $CscfgFileName)  {

    Write-Output "Publishing Cloud Service $FriendlyCloudServiceName"

    Ensure-CloudServiceExists
    Ensure-CloudServiceCertificateExists

    $Deployment = Get-AzureDeployment -ServiceName $FriendlyCloudServiceName -Slot $SlotName -ErrorAction SilentlyContinue 
    $Upgrading = ($? -and $Deployment -ne $null)
	$doNotStart = ($SlotName -eq "Staging" -and $CloudServiceName.Contains("WorkerRoles") -eq $true)

    $DeploymentLabel = "$CloudServiceName-$CloudServiceVersion"
    
    Write-Output "Slot Name: $SlotName"
    Write-Output "Configuration: $CscfgFileName"
    Write-Output "Label: $DeploymentLabel"
    Write-Output "Upgrading deployment: $Upgrading"
	
	if ($Upgrading) {
        Set-AzureDeployment -Slot $SlotName -Package $CspkgFileName -Configuration $CscfgFileName -Label $DeploymentLabel -ServiceName $FriendlyCloudServiceName -Upgrade -Force -Verbose
    } else {
		if ($doNotStart) {
			New-AzureDeployment -Slot $SlotName -Package $CspkgFileName -Configuration $CscfgFileName -Label $DeploymentLabel -ServiceName $FriendlyCloudServiceName -DoNotStart -Verbose
		} else {
			New-AzureDeployment -Slot $SlotName -Package $CspkgFileName -Configuration $CscfgFileName -Label $DeploymentLabel -ServiceName $FriendlyCloudServiceName -Verbose
		}
    }

    Write-Output "Published Cloud Service $FriendlyCloudServiceName"
}

Function Ensure-CloudServiceExists {
    
    $CloudService = Get-AzureService -ServiceName $FriendlyCloudServiceName -ErrorAction SilentlyContinue

    if ($? -eq $false -or $CloudService -eq $null) {
        Write-Output "Creating new Cloud Service '$FriendlyCloudServiceName'"
        $CloudService = New-AzureService -ServiceName $FriendlyCloudServiceName -AffinityGroup $affinityGroup
        Write-Output "Created new Cloud Service '$FriendlyCloudServiceName'"

    } else {
        Write-Output "Cloud Service '$FriendlyCloudServiceName' already exists - nothing to do"
    }
}

Function Ensure-CloudServiceCertificateExists {
    
    Ensure-CertificateExists $nasPfxFile $nasPfxPassword
    
    if ($webPfxPassword){
        Ensure-CertificateExists $webPfxFile $webPfxPassword
    }
}

Function Ensure-CertificateExists ($pfxPath, $pfxPassword) {
    
    Write-Output "Getting Thumbprint for Pfx: $pfxPath with Password: $pfxPassword"
    $secpasswd = ConvertTo-SecureString $pfxPassword -AsPlainText -Force
    $pfx = Get-PfxData -FilePath $pfxPath -Password $secpasswd
    $thumbprint = $pfx.EndEntityCertificates[0].Thumbprint
    Write-Output "Thumbprint: $thumbprint retrieved from Pfx: $pfxPath using Password: $pfxPassword"

    $Certificate = Get-AzureCertificate $FriendlyCloudServiceName -Thumbprint $thumbprint -ThumbprintAlgorithm sha1 -ErrorAction SilentlyContinue

    if ($? -eq $false -or $Certificate -eq $null) {
        Write-Output "Adding Cloud Service certificate '$pfxPath'"
        Add-AzureCertificate -ServiceName $FriendlyCloudServiceName -CertToDeploy $pfxPath -Password $pfxPassword
        Write-Output "Added Cloud Service certificate '$pfxPath'"

    } else {
        Write-Output "Cloud Service certificate already exists - nothing to do"
    }
}

Write-Output "About to deploy Cloud Service:"
Write-Output "EnvironmentName: $EnvironmentName"
Write-Output "StorageContainerName: $StorageContainerName"
Write-Output "StorageAccessKey: $StorageAccessKey"
Write-Output "CloudServiceName: $CloudServiceName"
Write-Output "CloudServiceVersion: $CloudServiceVersion"

Start-ServiceDeployment

Exit $error.Count
