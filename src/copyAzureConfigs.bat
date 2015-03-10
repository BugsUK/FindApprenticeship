REM Run against clean repos to ensure required azure project files are copied
SET projectroot=c:\Projects\SFA
xcopy /E /Y %projectroot%\Apprenticeships\Configuration\Applications\SFA.Apprenticeships.Web.Candidate.Azure %projectroot%\Beta\src\SFA.Apprenticeships.Web.Candidate.Azure
xcopy /E /Y %projectroot%\Apprenticeships\Configuration\Applications\SFA.Apprenticeships.Infrastructure.WorkerRoles.Azure %projectroot%\Beta\src\SFA.Apprenticeships.Infrastructure.WorkerRoles.Azure

