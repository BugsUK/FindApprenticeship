namespace SFA.Apprenticeships.Domain.Interfaces.Configuration
{
    using System;
    using System.Collections.Generic;

    public interface ISendGridConfiguration
    {
        string Username { get; }

        string Password { get; }

        string Originator { get; }

        string Url { get; }

        string CallbackUrl { get; }

        IEnumerable<IEmailTemplate> Templates {get;} 
    }

    public interface IEmailTemplate
    {
        Guid Id { get; }

        string Name { get; }

        string FromEmail { get; }
    }
}
