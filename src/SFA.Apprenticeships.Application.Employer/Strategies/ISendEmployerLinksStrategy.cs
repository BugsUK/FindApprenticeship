namespace SFA.Apprenticeships.Application.Employer.Strategies
{
    using System.Collections.Generic;

    public interface ISendEmployerLinksStrategy
    {
        void Send(IDictionary<string, string> applicationLinks, string recipientEmailAddress);
    }
}