[CmdletBinding()]
Param(
    [Parameter(Mandatory=$True)]
    [string]$buildCounter
)

$versionText = (Get-Content src\version.txt -ErrorAction Stop) + $buildCounter
"##teamcity[buildNumber '$versionText']"