
$file = "C:\Windows\System32\drivers\etc\hosts"

function add-host([string]$filename, [string]$ip, [string]$hostname) {
	remove-host $filename $hostname
	$ip + "`t`t" + $hostname | Out-File -encoding ASCII -append $filename
}

function remove-host([string]$filename, [string]$hostname) {
	$c = Get-Content $filename
	$newLines = @()
	
	foreach ($line in $c) {
		$bits = [regex]::Split($line, "\t+")
		if ($bits.count -eq 2) {
			if ($bits[1] -ne $hostname) {
				$newLines += $line
			}
		} else {
			$newLines += $line
		}
	}
	
	# Write file
	Clear-Content $filename
	foreach ($line in $newLines) {
		$line | Out-File -encoding ASCII -append $filename
	}
}

function print-hosts([string]$filename) {
	$c = Get-Content $filename
	
	foreach ($line in $c) {
		$bits = [regex]::Split($line, "\t+")
		if ($bits.count -eq 2) {
			Write-Host $bits[0] `t`t $bits[1]
		}
	}
}

print-hosts $file
Write-Host "Adding findapprenticeship websites"
add-host $file "127.0.0.1" "local.findapprenticeship.service.gov.uk"
print-hosts $file

C:\Windows\System32\inetsrv\appcmd delete site local.findapprenticeship.service.gov.uk
C:\Windows\System32\inetsrv\appcmd delete apppool FAA
C:\Windows\System32\inetsrv\appcmd add apppool /name:FAA /managedRuntimeVersion:v4.0 /managedPipelineMode:Integrated
C:\Windows\System32\inetsrv\appcmd add site /name:local.findapprenticeship.service.gov.uk /id:100 /physicalPath:C:\Projects\SFA\Beta\src\SFA.Apprenticeships.Web.Candidate\ /bindings:https/*:443:local.findapprenticeship.service.gov.uk
C:\Windows\System32\inetsrv\appcmd set app "local.findapprenticeship.service.gov.uk/" /applicationPool:FAA