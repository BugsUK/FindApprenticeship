namespace SFA.Apprenticeships.Application.Interfaces.Applications
{
    public interface IApplicationService
    {
        int GetApplicationCount(string vacancyReference);

        int GetApplicationCount(int vacancyId);
    }
}