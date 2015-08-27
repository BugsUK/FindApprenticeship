$p = Get-Process chrome -ErrorAction SilentlyContinue
if($p -ne $null)
{
    Stop-Process -processname chrome -ErrorAction SilentlyContinue
}
$p = Get-Process chrome -ErrorAction SilentlyContinue
if($p -ne $null)
{
    Stop-Process -processname chromedriver -ErrorAction SilentlyContinue
}
$p = Get-Process firefox -ErrorAction SilentlyContinue
if($p -ne $null)
{
    Stop-Process -processname firefox -ErrorAction SilentlyContinue
}