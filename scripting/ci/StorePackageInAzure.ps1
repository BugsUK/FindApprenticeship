[CmdletBinding()]
Param(
    [Parameter(Mandatory=$True)]
    [string]$environmentName,

    [Parameter(Mandatory=$True)]
    [string]$cloudServiceName,	

    [Parameter(Mandatory=$True)]
    [string]$cloudServiceVersion,	

    [Parameter(Mandatory = $True)]
    [string]$subscriptionName,

    [Parameter(Mandatory = $True)]
    [string]$storageName,

    [Parameter(Mandatory=$True)]
    [string]$storageContainerName,

    [Parameter(Mandatory=$True)]
    [string]$storageAccessKey 
)

#if ($environmentName -eq "Dev"){
#	Write-Output "Not storing package as Dev deployment"
#	Exit 0
#}

$srcPkg = "Beta\src\$cloudServiceName\bin\$environmentName\app.publish\$cloudServiceName.cspkg"
$destPkg = "$environmentName\$cloudServiceName\$cloudServiceVersion\$cloudServiceName.cspkg"

$srcCfg = "Beta\src\$cloudServiceName\bin\$environmentName\app.publish\ServiceConfiguration.$environmentName.cscfg"
$destCfg = "$environmentName\$cloudServiceName\$cloudServiceVersion\ServiceConfiguration.cscfg"

Write-Output "Storing package $srcPkg to Azure products $destPkg"
Write-Output "Storing cfg $srcCfg to Azure products $destCfg"

Import-Module Azure
Set-AzureSubscription -CurrentStorageAccount $storageName -SubscriptionName $subscriptionName
$context = New-AzureStorageContext -StorageAccountName $storageName -StorageAccountKey $storageAccessKey

Write-Host "Copying files to Azure Storage" 
Set-AzureStorageBlobContent -Blob "$destPkg" -Container $storageContainerName -File $srcPkg -Context $context -Force
Set-AzureStorageBlobContent -Blob "$destCfg" -Container $storageContainerName -File $srcCfg -Context $context -Force
Write-Host "Copied files to Azure Storage"

Exit $error.Count