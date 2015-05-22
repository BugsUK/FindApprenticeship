namespace SFA.Apprenticeships.Application.Candidate.Strategies.Traineeships
{
    using System;
    using Domain.Entities.Applications;

    public interface ISaveTraineeshipApplicationStrategy
    {
        TraineeshipApplicationDetail SaveApplication(Guid candidateId, int vacancyId, TraineeshipApplicationDetail traineeshipApplication);
    }
}