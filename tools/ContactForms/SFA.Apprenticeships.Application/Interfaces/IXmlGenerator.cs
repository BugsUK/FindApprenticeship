
namespace SFA.Apprenticeships.Application.Interfaces
{
    using System.Collections.Generic;
    using Communications;
    public interface IXmlGenerator
    {
        string SerializeToXmlFile(MessageTypes messageTypes, IEnumerable<CommunicationToken> tokens);
    }
}