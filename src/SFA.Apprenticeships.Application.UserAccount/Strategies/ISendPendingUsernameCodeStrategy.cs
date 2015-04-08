namespace SFA.Apprenticeships.Application.UserAccount.Strategies
{
    // TODO: AG: US711: inject into UserAccountService.
    public interface ISendPendingUsernameCodeStrategy
    {
        void SendPendingUsernameCode(string username);
    }
}