namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    public interface IRequestEmailReminderStrategy
    {
        void RequestEmailReminder(string phoneNumber);
    }
}