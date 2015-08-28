$versionText = (Get-Content Beta\src\version.txt -ErrorAction Stop) + "%build.counter%"
Write-Host $versionText
Write-Host "##teamcity[buildNumber '$versionText']"