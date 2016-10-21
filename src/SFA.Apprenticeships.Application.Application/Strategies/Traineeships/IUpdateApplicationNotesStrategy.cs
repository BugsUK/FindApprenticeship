namespace SFA.Apprenticeships.Application.Application.Strategies.Traineeships
{
    using System;

    public interface IUpdateApplicationNotesStrategy
    {
        void UpdateApplicationNotes(Guid applicationId, string notes, bool publishUpdate);
    }
}