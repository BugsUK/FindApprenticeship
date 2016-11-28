namespace SFA.Apprenticeships.Application.Employer.Strategies
{
    using System;
    using System.Collections.Generic;

    public interface ISendEmployerLinksStrategy
    {
        void Send(string vacancyTitle, string providerName, IDictionary<string, string> applicationLinks,
            DateTime linkExpiryDateTime, string recipientEmailAddress, string optionalMessage = null);
    }
}