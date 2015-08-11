[CmdletBinding()]
Param(
    [Parameter(Mandatory=$True)]
    [string]$zipPassword,

    [Parameter(Mandatory=$True)]
    [string]$to,

    [Parameter(Mandatory=$True)]
    [string]$cc
)

Function Email-EShotData(){
	& ..\SFA.Apprenticeships.Metrics.Candidate.exe

    $filename = "FAA-candidates-" + (Get-Date -format yyyyMMdd)

    & .\7z a -tzip -p"$zipPassword" -mx9 "$filename.zip" "$filename.csv"

    & .\sendEmail.exe -f "noreply@findapprenticeship.service.gov.uk" -t $to -cc $cc -u "FAA E-Shot Data" -m "Attached is an export of all the active Candidates using the FAA service who could be contacted via an e-shot campaign. Check the AllowMarketingEmails and AllowMarketingTexts for allowed communication channels" -s "smtp.sendgrid.net:587" -xu "findapprenticeshipservice" -xp "k9qP35sa" -a "$filename.zip"
    & .\sendEmail.exe -f "noreply@findapprenticeship.service.gov.uk" -t $to -cc $cc -u "FAA E-Shot Data Password" -m "Attachment Password: $zipPassword" -s "smtp.sendgrid.net:587" -xu "findapprenticeshipservice" -xp "k9qP35sa"
    
    Remove-Item "$filename.zip"
    Remove-Item "$filename.csv"
}

Email-EShotData

#.\e-shot.ps1 "test" "test1@test.com,test2@test.com" "test3@test.com,test4@test.com"