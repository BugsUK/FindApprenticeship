namespace SFA.Apprenticeships.Domain.Interfaces.Configuration
{
    using System.Collections.Generic;

    public interface IReachSmsConfiguration
    {
        string Username { get; }

        string Password { get; }

        string Originator { get; }

        string Url { get; }

        string CallbackUrl { get; }

        IEnumerable<ISmsTemplate> Templates {get;} 
    }

    public interface ISmsTemplate
    {
        string Name { get; }

        string Message { get; }
    }
}
