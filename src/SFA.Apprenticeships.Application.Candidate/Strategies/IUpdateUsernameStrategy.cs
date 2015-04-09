namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;

    public interface IUpdateUsernameStrategy
    {
        void UpdateUsername(Guid userId, string verfiyCode, string password);
    }
}
