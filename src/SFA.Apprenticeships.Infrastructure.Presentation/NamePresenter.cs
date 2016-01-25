namespace SFA.Apprenticeships.Infrastructure.Presentation
{
    using Domain.Entities.Users;

    public static class NamePresenter
    {
        public static string GetDisplayText(this Name name)
        {
            return $"{name.FirstName} {name.LastName}";
        }
    }
}