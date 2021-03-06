﻿namespace SFA.Apprenticeships.Infrastructure.Elastic.Common.Entities
{
    using System.ComponentModel;

    public enum VacancyLevel
    {
        Unknown = 0,

        [Description("IntermediateLevelApprenticeship")] 
        Intermediate,

        [Description("AdvancedLevelApprenticeship")] 
        Advanced,

        [Description("HigherApprenticeship")]
        Higher
    }
}
    