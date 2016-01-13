namespace SFA.Infrastructure.Interfaces
{
    public interface IConfigurationService
    {
        TSettings Get<TSettings>() where TSettings : class;
    }
}
