namespace SFA.Apprenticeships.Domain.Entities.Users
{
    //todo: consider removing "Inactive" state as implementation deletes the user rather than marking as inactive
    //todo: consider removing "Dormant" as it's a derived state
    public enum UserStatuses
    {
        Unknown = 0,
        PendingActivation = 10,     // once registered and awaiting activation
        Active = 20,                // once activated
        Inactive = 30,              // when superseded by a new account (user changed their username)
        Locked = 90,                // if locked out for security reasons
        Dormant = 100,              // if the account has not been recently used
        PendingDeletion = 999       // when marked for hard deletion following being dormant or if not activated
    }
}
