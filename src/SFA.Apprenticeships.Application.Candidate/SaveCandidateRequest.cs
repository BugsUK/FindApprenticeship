namespace SFA.Apprenticeships.Application.Candidate
{
    using System;
    using Interfaces.Messaging;

    public class SaveCandidateRequest : BaseRequest
    {
        public Guid CandidateId { get; set; }
    }
}