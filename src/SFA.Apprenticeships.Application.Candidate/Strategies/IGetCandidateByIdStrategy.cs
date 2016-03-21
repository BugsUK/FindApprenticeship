﻿namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;
    using Domain.Entities.Candidates;

    public interface IGetCandidateByIdStrategy
    {
        Candidate GetCandidate(Guid candidateId);
    }
}