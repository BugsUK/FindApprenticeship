namespace SFA.Apprenticeships.Application.Application.Strategies.Apprenticeships
{
    using System;

    public interface IUpdateApplicationNotesStrategy
    {
        void UpdateApplicationNotes(Guid applicationId, string notes);
    }
}