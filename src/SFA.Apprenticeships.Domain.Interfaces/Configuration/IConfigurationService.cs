namespace SFA.Apprenticeships.Domain.Interfaces.Configuration
{
    public interface IConfigurationService
    {
        TSettings Get<TSettings>() where TSettings : class;
    }
}
