namespace SFA.Apprenticeships.Domain.Interfaces.Configuration
{
    public interface IMongoConfiguration
    {
        string CandidatesDb { get; }

        string ApplicationsDb { get; }

        string UsersDb { get; }

        string CommunicationsDb { get; }

        string AdminDB { get; }

        string AthenticationDb { get; }
    }
}
