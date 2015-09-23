[CmdletBinding()]
Param(
   [Parameter(Mandatory = $True)]
   [string]$CdnStorageName,

   [Parameter(Mandatory = $True)]
   [string]$CdnStorageAccessKey,

   [Parameter(Mandatory = $True)]
   [string]$CdnStorageContainerName
)

Function Update-AllCdnContent ()
{
	Import-Module Azure

    $StorageContext = New-AzureStorageContext -StorageAccountName $CdnStorageName -StorageAccountKey $CdnStorageAccessKey

    #Delete all existing cdn content
    #Get-AzureStorageBlob -Context $StorageContext -Container $CdnStorageContainerName | Remove-AzureStorageBlob

    $AssetsPath = "Beta\src\SFA.Apprenticeships.Web.Candidate\Content\_assets"

    Update-CdnContent -Path "$AssetsPath\css" -Filter "*.*" -StorageContext $StorageContext
    Update-CdnContent -Path "$AssetsPath\fonts" -Filter "*.*" -StorageContext $StorageContext
    Update-CdnContent -Path "$AssetsPath\img" -Filter "*.*" -StorageContext $StorageContext
    #Update-CdnContent -Path "$AssetsPath\js" -Filter "*.*" -StorageContext $StorageContext
}

Function Update-CdnContent ($Path, $Filter, $StorageContext)
{
    Get-ChildItem -Recurse -File -Path $Path -Filter $Filter | ForEach-Object {
        $currentPath = (Get-Item -Path ".\" -Verbose).FullName + "\Beta\src\SFA.Apprenticeships.Web.Candidate\Content\_assets\"
        $blobName = $_.FullName.Replace($currentPath,'')
        $contentType = switch ([System.IO.Path]::GetExtension($_.FullName)) 
            { 
                ".css" {"text/css"} 
                ".ttf" {"application/font-sfnt"} 
                ".otf" {"application/font-sfnt"} 
                ".woff" {"application/font-woff"} 
                ".eot" {"application/vnd.ms-fontobject"} 
                ".png" {"image/png"} 
                ".svg" {"image/svg+xml"} 
                ".ico" {"image/x-icon"} 
                ".js" {"text/javascript"} 
                default {$null} 
            } 

		if($contentType -ne $null) {
			Set-AzureStorageBlobContent -Context $StorageContext -File $_.FullName -Blob $blobName -Properties @{ContentType=$contentType} -Container $CdnStorageContainerName -Force -Verbose
		} else {
			Set-AzureStorageBlobContent -Context $StorageContext -File $_.FullName -Blob $blobName -Container $CdnStorageContainerName -Force -Verbose
		}
    }
}

Write-Host -Object "Updating all CDN content..."
Write-Host -Object "CdnStorageName: $CdnStorageName"
Write-Host -Object "CdnStorageAccessKey: $CdnStorageAccessKey"
Write-Host -Object "CdnStorageContainerName: $CdnStorageContainerName"

Update-AllCdnContent