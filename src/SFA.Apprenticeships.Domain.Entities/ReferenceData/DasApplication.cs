namespace SFA.Apprenticeships.Domain.Entities.ReferenceData
{
    using System;

    [Flags]
    public enum DasApplication
    {
        Find = 0x01,
        Recruit = 0x02,
        Manage = 0x04
    }
}